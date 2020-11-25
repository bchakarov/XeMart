namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    using XeMart.Data.Common.Repositories;
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class ProductsService : IProductsService
    {
        private readonly IDeletableEntityRepository<Product> productsRepository;
        private readonly IImagesService imagesService;
        private readonly IDeletableEntityRepository<ProductImage> productImagesRepository;

        public ProductsService(
            IDeletableEntityRepository<Product> productsRepository,
            IImagesService imagesService,
            IDeletableEntityRepository<ProductImage> productImagesRepository)
        {
            this.productsRepository = productsRepository;
            this.imagesService = imagesService;
            this.productImagesRepository = productImagesRepository;
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

        public IEnumerable<T> GetAll<T>() =>
            this.productsRepository.AllAsNoTracking()
            .To<T>().ToList();

        public IEnumerable<T> GetAllDeleted<T>() =>
            this.productsRepository.AllAsNoTrackingWithDeleted()
            .Where(x => x.IsDeleted)
            .To<T>().ToList();

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

        public T GetById<T>(string id) =>
             this.productsRepository.AllAsNoTracking()
            .Where(x => x.Id == id)
            .To<T>()
            .FirstOrDefault();

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
    }
}
