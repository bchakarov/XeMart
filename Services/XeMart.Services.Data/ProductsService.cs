namespace XeMart.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    using XeMart.Data.Common.Repositories;
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;
    using XeMart.Web.Infrastructure.ExtensionMethods;

    public class ProductsService : IProductsService
    {
        private readonly IDeletableEntityRepository<Product> productsRepository;
        private readonly IImagesService imagesService;
        private readonly IDeletableEntityRepository<ProductImage> productImagesRepository;
        private readonly IRepository<UserProductReview> userProductReviewsRepository;

        public ProductsService(
            IDeletableEntityRepository<Product> productsRepository,
            IImagesService imagesService,
            IDeletableEntityRepository<ProductImage> productImagesRepository,
            IRepository<UserProductReview> userProductReviewsRepository)
        {
            this.productsRepository = productsRepository;
            this.imagesService = imagesService;
            this.productImagesRepository = productImagesRepository;
            this.userProductReviewsRepository = userProductReviewsRepository;
        }

        public async Task CreateAsync<T>(T model, IEnumerable<IFormFile> images, string fullDirectoryPath, string webRootPath)
        {
            var product = AutoMapperConfig.MapperInstance.Map<Product>(model);

            if (images != null && images.Count() > 0)
            {
                foreach (var image in images)
                {
                    var imageUrl = await this.imagesService.UploadLocalImageAsync(image, fullDirectoryPath);
                    product.Images.Add(new ProductImage
                    {
                        ImageUrl = imageUrl.Replace(webRootPath, string.Empty).Replace("\\", "/"),
                    });
                }
            }

            await this.productsRepository.AddAsync(product);
            await this.productsRepository.SaveChangesAsync();
        }

        public async Task<bool> CreateReviewAsync<T>(T model)
        {
            var productReview = AutoMapperConfig.MapperInstance.Map<UserProductReview>(model);
            var product = this.GetById(productReview.ProductId);

            if (product == null || this.userProductReviewsRepository.AllAsNoTracking().Any(x => x.ProductId == productReview.ProductId && x.UserId == productReview.UserId))
            {
                return false;
            }

            await this.userProductReviewsRepository.AddAsync(productReview);
            await this.userProductReviewsRepository.SaveChangesAsync();

            return true;
        }

        public IEnumerable<T> GetAll<T>() =>
            this.productsRepository.AllAsNoTracking()
            .To<T>().ToList();

        public IEnumerable<T> TakeProductsBySubcategoryId<T>(int subcategoryId, int page, int productsToTake, string sorting)
        {
            var columnName = string.Empty;
            var isAscending = true;

            sorting = sorting.ToLower();

            if (sorting == "price desc")
            {
                columnName = "Price";
                isAscending = false;
            }
            else if (sorting == "price asc")
            {
                columnName = "Price";
            }
            else if (sorting == "newest")
            {
                columnName = "CreatedOn";
                isAscending = false;
            }
            else if (sorting == "oldest")
            {
                columnName = "CreatedOn";
            }

            return this.productsRepository.AllAsNoTracking()
                .Where(x => x.SubcategoryId == subcategoryId)
                .MyOrderBy(columnName, isAscending)
                .Skip((page - 1) * productsToTake)
                .Take(productsToTake)
                .To<T>().ToList();
        }

        public IEnumerable<T> TakeProductsBySearchStringAndMainCategoryId<T>(string search, int? mainCategoryId, int page, int productsToTake, string sorting)
        {
            var predicateExpression = this.BuildSearchPredicateExpression(search, mainCategoryId);

            var columnName = string.Empty;
            var isAscending = true;

            sorting = sorting.ToLower();

            if (sorting == "price desc")
            {
                columnName = "Price";
                isAscending = false;
            }
            else if (sorting == "price asc")
            {
                columnName = "Price";
            }
            else if (sorting == "newest")
            {
                columnName = "CreatedOn";
                isAscending = false;
            }
            else if (sorting == "oldest")
            {
                columnName = "CreatedOn";
            }

            return this.productsRepository.AllAsNoTracking()
                .Where(predicateExpression)
                .MyOrderBy(columnName, isAscending)
                .Skip((page - 1) * productsToTake)
                .Take(productsToTake)
                .To<T>().ToList();
        }

        public int GetProductsCountBySearchStringAndMainCategoryId(string search, int? mainCategoryId)
        {
            var predicateExpression = this.BuildSearchPredicateExpression(search, mainCategoryId);

            return this.productsRepository.AllAsNoTracking()
                .Count(predicateExpression);
        }

        public IEnumerable<T> GetAllDeleted<T>() =>
            this.productsRepository.AllAsNoTrackingWithDeleted()
            .Where(x => x.IsDeleted)
            .To<T>().ToList();

        public IEnumerable<T> GetNewestBySubcategoryId<T>(int subcategoryId, int productsToTake) =>
            this.productsRepository.AllAsNoTracking()
            .Where(x => x.SubcategoryId == subcategoryId)
            .OrderByDescending(x => x.CreatedOn)
            .Take(productsToTake)
            .To<T>().ToList();

        public IEnumerable<T> GetNewest<T>(int productsToTake) =>
            this.productsRepository.AllAsNoTracking()
            .OrderByDescending(x => x.CreatedOn)
            .Take(productsToTake)
            .To<T>().ToList();

        public IEnumerable<T> GetTopRated<T>(int productsToTake)
        {
            var productIds = this.userProductReviewsRepository.AllAsNoTracking()
                .GroupBy(x => x.ProductId)
                .Select(x => new { ProductId = x.Key, Total = x.Count(), AvgRating = x.Average(r => r.Rating) })
                .OrderByDescending(x => x.AvgRating)
                .ThenByDescending(x => x.Total)
                .Take(productsToTake)
                .ToList();

            var products = new List<T>();
            foreach (var product in productIds)
            {
                var mappedProduct = this.GetById<T>(product.ProductId);
                products.Add(mappedProduct);
            }

            return products;
        }

        public async Task<bool> EditAsync<T>(T model, IEnumerable<IFormFile> images, string fullDirectoryPath, string webRootPath)
        {
            var newProduct = AutoMapperConfig.MapperInstance.Map<Product>(model);

            var foundProduct = this.GetById(newProduct.Id);
            if (foundProduct == null)
            {
                return false;
            }

            foundProduct.Name = newProduct.Name;
            foundProduct.Description = newProduct.Description;
            foundProduct.Price = newProduct.Price;
            foundProduct.SubcategoryId = newProduct.SubcategoryId;

            if (images != null && images.Count() > 0)
            {
                foreach (var image in images)
                {
                    var imageUrl = await this.imagesService.UploadLocalImageAsync(image, fullDirectoryPath);
                    foundProduct.Images.Add(new ProductImage
                    {
                        ImageUrl = imageUrl.Replace(webRootPath, string.Empty).Replace("\\", "/"),
                    });
                }
            }

            this.productsRepository.Update(foundProduct);
            await this.productsRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var product = this.GetById(id);
            if (product == null)
            {
                return false;
            }

            this.productsRepository.Delete(product);

            foreach (var image in product.Images)
            {
                this.productImagesRepository.Delete(image);
            }

            await this.productsRepository.SaveChangesAsync();
            await this.productImagesRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UndeleteAsync(string id)
        {
            var product = this.GetDeletedById(id);
            if (product == null)
            {
                return false;
            }

            this.productsRepository.Undelete(product);
            await this.productsRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteImageAsync(string id)
        {
            var image = this.GetImageById(id);
            if (image == null)
            {
                return false;
            }

            this.productImagesRepository.Delete(image);
            await this.productImagesRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteReviewAsync(string id)
        {
            var review = this.GetReviewById(id);
            if (review == null)
            {
                return false;
            }

            this.userProductReviewsRepository.Delete(review);
            await this.userProductReviewsRepository.SaveChangesAsync();

            return true;
        }

        public T GetById<T>(string id) =>
             this.productsRepository.AllAsNoTracking()
            .Where(x => x.Id == id)
            .To<T>()
            .FirstOrDefault();

        public bool HasProduct(string id) =>
            this.productsRepository.AllAsNoTracking()
            .Any(x => x.Id == id);

        private Product GetById(string id) =>
             this.productsRepository.All().Include(x => x.Images)
            .FirstOrDefault(x => x.Id == id);

        private Product GetDeletedById(string id) =>
            this.productsRepository.AllAsNoTrackingWithDeleted()
            .Where(x => x.IsDeleted && x.Id == id)
            .FirstOrDefault();

        private ProductImage GetImageById(string id) =>
            this.productImagesRepository.All()
            .FirstOrDefault(x => x.Id == id);

        private UserProductReview GetReviewById(string id) =>
            this.userProductReviewsRepository.All()
            .FirstOrDefault(x => x.Id == id);

        private Expression<Func<Product, bool>> BuildSearchPredicateExpression(string search, int? mainCategoryId)
        {
            Expression<Func<Product, bool>> predicateExpression = x => x.Name.ToLower().Contains(search.ToLower());
            if (mainCategoryId != null)
            {
                predicateExpression = x => x.Name.ToLower().Contains(search.ToLower()) && x.Subcategory.MainCategoryId == mainCategoryId;
            }

            return predicateExpression;
        }
    }
}
