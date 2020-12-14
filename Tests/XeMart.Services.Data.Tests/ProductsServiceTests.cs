namespace XeMart.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Internal;

    using Moq;

    using XeMart.Common;
    using XeMart.Data.Common.Repositories;
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;
    using XeMart.Web.ViewModels;
    using XeMart.Web.ViewModels.Administration.Products;
    using XeMart.Web.ViewModels.Products;

    using Xunit;

    using ProductViewModel = XeMart.Web.ViewModels.Products.ProductViewModel;

    [Collection("Sequential")]
    public class ProductsServiceTests
    {
        [Fact]
        public void GetAllGenericShouldReturnCorrectCountUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Product>>();

            var mainCategory = new MainCategory { Id = 1, Name = "TestMainCategory" };
            var subcategory = new Subcategory { Id = 1, Name = "TestSubcategory", MainCategory = mainCategory, MainCategoryId = mainCategory.Id };
            var products = new List<Product>
            {
                new Product { Id = "TestId1", Subcategory = subcategory },
                new Product { Id = "TestId2", Subcategory = subcategory },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(products.AsQueryable());

            var service = new ProductsService(repository.Object, null, null, null);
            Assert.Equal(2, service.GetAll<ProductViewModel>().Count());

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetAllGenericShouldMapCorrectlyWithNoImagesAndNoReviewsUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Product>>();

            var mainCategory = new MainCategory { Id = 1, Name = "TestMainCategory" };
            var subcategory = new Subcategory { Id = 1, Name = "TestSubcategory", MainCategory = mainCategory, MainCategoryId = mainCategory.Id };
            var products = new List<Product>
            {
                new Product { Id = "TestId1", Name = "TestProduct1", Subcategory = subcategory, SubcategoryId = subcategory.Id, Description = "TestDescription", Price = 42 },
                new Product { Id = "TestId2", Name = "TestProduct2", Subcategory = subcategory },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(products.AsQueryable());

            var service = new ProductsService(repository.Object, null, null, null);
            Assert.Equal("TestId1", service.GetAll<ProductViewModel>().FirstOrDefault().Id);
            Assert.Equal("TestProduct1", service.GetAll<ProductViewModel>().FirstOrDefault().Name);
            Assert.Equal("TestDescription", service.GetAll<ProductViewModel>().FirstOrDefault().Description);
            Assert.Equal(42, service.GetAll<ProductViewModel>().FirstOrDefault().Price);
            Assert.Equal(GlobalConstants.ImageNotFoundPath, service.GetAll<ProductViewModel>().FirstOrDefault().ImageUrl);
            Assert.Equal(0, service.GetAll<ProductViewModel>().FirstOrDefault().AverageRating);
            Assert.Equal(1, service.GetAll<ProductViewModel>().FirstOrDefault().SubcategoryId);
            Assert.Equal("TestSubcategory", service.GetAll<ProductViewModel>().FirstOrDefault().SubcategoryName);
            Assert.Equal(1, service.GetAll<ProductViewModel>().FirstOrDefault().SubcategoryMaincategoryId);
            Assert.Equal("TestMainCategory", service.GetAll<ProductViewModel>().FirstOrDefault().SubcategoryMaincategoryName);

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(10));
        }

        [Fact]
        public void GetAllGenericShouldMapCorrectlyWithImagesAndReviewsUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Product>>();

            var mainCategory = new MainCategory { Id = 1, Name = "TestMainCategory" };
            var subcategory = new Subcategory { Id = 1, Name = "TestSubcategory", MainCategory = mainCategory, MainCategoryId = mainCategory.Id };
            var images = new List<ProductImage>
            {
                new ProductImage { ImageUrl = "TestImageUrl1" },
                new ProductImage { ImageUrl = "TestImageUrl2" },
            };
            var reviews = new List<UserProductReview>
            {
                new UserProductReview { Rating = 5 },
                new UserProductReview { Rating = 2 },
                new UserProductReview { Rating = 1 },
            };
            var products = new List<Product>
            {
                new Product { Id = "TestId1", Name = "TestProduct1", Images = images, Reviews = reviews, Subcategory = subcategory, SubcategoryId = subcategory.Id, Description = "TestDescription", Price = 42 },
                new Product { Id = "TestId2", Name = "TestProduct2", Subcategory = subcategory },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(products.AsQueryable());

            var service = new ProductsService(repository.Object, null, null, null);
            Assert.Equal("TestId1", service.GetAll<ProductViewModel>().FirstOrDefault().Id);
            Assert.Equal("TestProduct1", service.GetAll<ProductViewModel>().FirstOrDefault().Name);
            Assert.Equal("TestDescription", service.GetAll<ProductViewModel>().FirstOrDefault().Description);
            Assert.Equal(42, service.GetAll<ProductViewModel>().FirstOrDefault().Price);
            Assert.Equal("TestImageUrl1", service.GetAll<ProductViewModel>().FirstOrDefault().ImageUrl);
            Assert.Equal(2.67, service.GetAll<ProductViewModel>().FirstOrDefault().AverageRating);
            Assert.Equal(1, service.GetAll<ProductViewModel>().FirstOrDefault().SubcategoryId);
            Assert.Equal("TestSubcategory", service.GetAll<ProductViewModel>().FirstOrDefault().SubcategoryName);
            Assert.Equal(1, service.GetAll<ProductViewModel>().FirstOrDefault().SubcategoryMaincategoryId);
            Assert.Equal("TestMainCategory", service.GetAll<ProductViewModel>().FirstOrDefault().SubcategoryMaincategoryName);

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(10));
        }

        [Fact]
        public void GetAllDeletedGenericShouldReturnCorrectCountUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Product>>();

            var mainCategory = new MainCategory { Id = 1, Name = "TestMainCategory" };
            var subcategory = new Subcategory { Id = 1, Name = "TestSubcategory", MainCategory = mainCategory, MainCategoryId = mainCategory.Id };
            var products = new List<Product>
            {
                new Product { Id = "TestId1", Subcategory = subcategory, IsDeleted = true, DeletedOn = new DateTime(2020, 12, 31, 12, 12, 12) },
                new Product { Id = "TestId2", Subcategory = subcategory, IsDeleted = true, DeletedOn = new DateTime(2020, 12, 31, 12, 12, 12) },
                new Product { Id = "TestId3", Subcategory = subcategory },
            };

            repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(products.AsQueryable());

            var service = new ProductsService(repository.Object, null, null, null);
            Assert.Equal(2, service.GetAllDeleted<ProductViewModel>().Count());

            repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Once);
        }

        [Fact]
        public void GetAllDeletedGenericShouldMapCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Product>>();

            var subcategory = new Subcategory { Id = 1, Name = "TestSubcategory" };
            var images = new List<ProductImage>
            {
                new ProductImage { ImageUrl = "TestImageUrl1" },
                new ProductImage { ImageUrl = "TestImageUrl2" },
            };
            var products = new List<Product>
            {
                new Product { Id = "TestId1", Name = "TestProduct1", IsDeleted = true, DeletedOn = new DateTime(2020, 12, 31, 12, 12, 12), Images = images, Subcategory = subcategory, SubcategoryId = subcategory.Id, Price = 42 },
                new Product { Id = "TestId2", Name = "TestProduct2", Subcategory = subcategory },
            };

            repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(products.AsQueryable());

            var service = new ProductsService(repository.Object, null, null, null);
            Assert.Equal("TestId1", service.GetAllDeleted<DeletedProductViewModel>().FirstOrDefault().Id);
            Assert.Equal("TestProduct1", service.GetAllDeleted<DeletedProductViewModel>().FirstOrDefault().Name);
            Assert.Equal("31-Dec-2020 12:12", service.GetAllDeleted<DeletedProductViewModel>().FirstOrDefault().DeletedOn);
            Assert.Equal(42, service.GetAllDeleted<DeletedProductViewModel>().FirstOrDefault().Price);
            Assert.Equal("TestImageUrl1", service.GetAllDeleted<DeletedProductViewModel>().FirstOrDefault().ImageUrl);
            Assert.Equal("TestSubcategory", service.GetAllDeleted<DeletedProductViewModel>().FirstOrDefault().SubcategoryName);

            repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Exactly(6));
        }

        [Fact]
        public void GetNewestGenericShouldReturnCorrectCountUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Product>>();

            var mainCategory = new MainCategory { Id = 1, Name = "TestMainCategory" };
            var subcategory = new Subcategory { Id = 1, Name = "TestSubcategory", MainCategory = mainCategory, MainCategoryId = mainCategory.Id };
            var products = new List<Product>
            {
                new Product { Id = "TestId1", Subcategory = subcategory },
                new Product { Id = "TestId2", Subcategory = subcategory },
                new Product { Id = "TestId3", Subcategory = subcategory },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(products.AsQueryable());

            var service = new ProductsService(repository.Object, null, null, null);
            Assert.Equal(2, service.GetNewest<ProductViewModel>(2).Count());

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetNewestGenericShouldMapCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Product>>();

            var images = new List<ProductImage>
            {
                new ProductImage { ImageUrl = "TestImageUrl1" },
                new ProductImage { ImageUrl = "TestImageUrl2" },
            };
            var reviews = new List<UserProductReview>
            {
                new UserProductReview { Rating = 5 },
                new UserProductReview { Rating = 2 },
                new UserProductReview { Rating = 1 },
            };
            var mainCategory = new MainCategory { Id = 1, Name = "TestMainCategory" };
            var subcategory = new Subcategory { Id = 1, Name = "TestSubcategory", MainCategory = mainCategory, MainCategoryId = mainCategory.Id };
            var products = new List<Product>
            {
                new Product { Id = "TestId1", Name = "TestProduct1", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 12), Subcategory = subcategory },
                new Product { Id = "TestId2", Name = "TestProduct2", Images = images, Reviews = reviews, CreatedOn = new DateTime(2020, 12, 31, 12, 12, 13), Subcategory = subcategory, SubcategoryId = subcategory.Id, Description = "TestDescription", Price = 42 },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(products.AsQueryable());

            var service = new ProductsService(repository.Object, null, null, null);
            Assert.Equal("TestId2", service.GetNewest<ProductViewModel>(2).FirstOrDefault().Id);
            Assert.Equal("TestProduct2", service.GetNewest<ProductViewModel>(2).FirstOrDefault().Name);
            Assert.Equal("TestDescription", service.GetNewest<ProductViewModel>(2).FirstOrDefault().Description);
            Assert.Equal(42, service.GetNewest<ProductViewModel>(2).FirstOrDefault().Price);
            Assert.Equal("TestImageUrl1", service.GetNewest<ProductViewModel>(2).FirstOrDefault().ImageUrl);
            Assert.Equal(2.67, service.GetNewest<ProductViewModel>(2).FirstOrDefault().AverageRating);
            Assert.Equal(1, service.GetNewest<ProductViewModel>(2).FirstOrDefault().SubcategoryId);
            Assert.Equal("TestSubcategory", service.GetNewest<ProductViewModel>(2).FirstOrDefault().SubcategoryName);
            Assert.Equal(1, service.GetNewest<ProductViewModel>(2).FirstOrDefault().SubcategoryMaincategoryId);
            Assert.Equal("TestMainCategory", service.GetNewest<ProductViewModel>(2).FirstOrDefault().SubcategoryMaincategoryName);

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(10));
        }

        [Fact]
        public void GetNewestBySubcategoryIdGenericShouldReturnCorrectCountUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Product>>();

            var mainCategory = new MainCategory { Id = 1, Name = "TestMainCategory" };
            var subcategory = new Subcategory { Id = 1, Name = "TestSubcategory", MainCategory = mainCategory, MainCategoryId = mainCategory.Id };
            var products = new List<Product>
            {
                new Product { Id = "TestId1", Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId2", Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId3", Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId4" },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(products.AsQueryable());

            var service = new ProductsService(repository.Object, null, null, null);
            Assert.Equal(3, service.GetNewestBySubcategoryId<ProductViewModel>(subcategory.Id, 4).Count());

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetNewestBySubcategoryIdGenericShouldMapCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Product>>();

            var images = new List<ProductImage>
            {
                new ProductImage { ImageUrl = "TestImageUrl1" },
                new ProductImage { ImageUrl = "TestImageUrl2" },
            };
            var reviews = new List<UserProductReview>
            {
                new UserProductReview { Rating = 5 },
                new UserProductReview { Rating = 2 },
                new UserProductReview { Rating = 1 },
            };
            var mainCategory = new MainCategory { Id = 1, Name = "TestMainCategory" };
            var subcategory = new Subcategory { Id = 1, Name = "TestSubcategory", MainCategory = mainCategory, MainCategoryId = mainCategory.Id };
            var products = new List<Product>
            {
                new Product { Id = "TestId1", Name = "TestProduct1", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 12), Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId2", Name = "TestProduct2", Images = images, Reviews = reviews, CreatedOn = new DateTime(2020, 12, 31, 12, 12, 13), Subcategory = subcategory, SubcategoryId = subcategory.Id, Description = "TestDescription", Price = 42 },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(products.AsQueryable());

            var service = new ProductsService(repository.Object, null, null, null);
            Assert.Equal("TestId2", service.GetNewestBySubcategoryId<ProductViewModel>(subcategory.Id, 2).FirstOrDefault().Id);
            Assert.Equal("TestProduct2", service.GetNewestBySubcategoryId<ProductViewModel>(subcategory.Id, 2).FirstOrDefault().Name);
            Assert.Equal("TestDescription", service.GetNewestBySubcategoryId<ProductViewModel>(subcategory.Id, 2).FirstOrDefault().Description);
            Assert.Equal(42, service.GetNewestBySubcategoryId<ProductViewModel>(subcategory.Id, 2).FirstOrDefault().Price);
            Assert.Equal("TestImageUrl1", service.GetNewestBySubcategoryId<ProductViewModel>(subcategory.Id, 2).FirstOrDefault().ImageUrl);
            Assert.Equal(2.67, service.GetNewestBySubcategoryId<ProductViewModel>(subcategory.Id, 2).FirstOrDefault().AverageRating);
            Assert.Equal(1, service.GetNewestBySubcategoryId<ProductViewModel>(subcategory.Id, 2).FirstOrDefault().SubcategoryId);
            Assert.Equal("TestSubcategory", service.GetNewestBySubcategoryId<ProductViewModel>(subcategory.Id, 2).FirstOrDefault().SubcategoryName);
            Assert.Equal(1, service.GetNewestBySubcategoryId<ProductViewModel>(subcategory.Id, 2).FirstOrDefault().SubcategoryMaincategoryId);
            Assert.Equal("TestMainCategory", service.GetNewestBySubcategoryId<ProductViewModel>(subcategory.Id, 2).FirstOrDefault().SubcategoryMaincategoryName);

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(10));
        }

        [Fact]
        public void GetByIdGenericShouldMapCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Product>>();

            var images = new List<ProductImage>
            {
                new ProductImage { ImageUrl = "TestImageUrl1" },
                new ProductImage { ImageUrl = "TestImageUrl2" },
            };
            var reviews = new List<UserProductReview>
            {
                new UserProductReview { Rating = 5 },
                new UserProductReview { Rating = 2 },
                new UserProductReview { Rating = 1 },
            };
            var mainCategory = new MainCategory { Id = 1, Name = "TestMainCategory" };
            var subcategory = new Subcategory { Id = 1, Name = "TestSubcategory", MainCategory = mainCategory, MainCategoryId = mainCategory.Id };
            var products = new List<Product>
            {
                new Product { Id = "TestId1", Name = "TestProduct1", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 12), Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId2", Name = "TestProduct2", Images = images, Reviews = reviews, CreatedOn = new DateTime(2020, 12, 31, 12, 12, 13), Subcategory = subcategory, SubcategoryId = subcategory.Id, Description = "TestDescription", Price = 42 },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(products.AsQueryable());

            var service = new ProductsService(repository.Object, null, null, null);
            Assert.Equal("TestId2", service.GetById<ProductViewModel>("TestId2").Id);
            Assert.Equal("TestProduct2", service.GetById<ProductViewModel>("TestId2").Name);
            Assert.Equal("TestDescription", service.GetById<ProductViewModel>("TestId2").Description);
            Assert.Equal(42, service.GetById<ProductViewModel>("TestId2").Price);
            Assert.Equal("TestImageUrl1", service.GetById<ProductViewModel>("TestId2").ImageUrl);
            Assert.Equal(2.67, service.GetById<ProductViewModel>("TestId2").AverageRating);
            Assert.Equal(1, service.GetById<ProductViewModel>("TestId2").SubcategoryId);
            Assert.Equal("TestSubcategory", service.GetById<ProductViewModel>("TestId2").SubcategoryName);
            Assert.Equal(1, service.GetById<ProductViewModel>("TestId2").SubcategoryMaincategoryId);
            Assert.Equal("TestMainCategory", service.GetById<ProductViewModel>("TestId2").SubcategoryMaincategoryName);

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(10));
        }

        [Fact]
        public void HasProductShouldWorkCorrectlyUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<Product>>();

            var products = new List<Product>
            {
                new Product { Id = "TestId1" },
                new Product { Id = "TestId2" },
                new Product { Id = "TestId3" },
                new Product { Id = "TestId4" },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(products.AsQueryable());

            var service = new ProductsService(repository.Object, null, null, null);
            Assert.True(service.HasProduct("TestId4"));
            Assert.False(service.HasProduct("TestId5"));

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(2));
        }

        [Fact]
        public void GetTopRatedGenericShouldWorkCorrectlyWithDifferentRatingsUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var productRepository = new Mock<IDeletableEntityRepository<Product>>();
            var productReviewRepository = new Mock<IRepository<UserProductReview>>();

            var reviews = new List<UserProductReview>
            {
                new UserProductReview { ProductId = "TestId1", Rating = 5 },
                new UserProductReview { ProductId = "TestId1", Rating = 3 },
                new UserProductReview { ProductId = "TestId2", Rating = 5 },
                new UserProductReview { ProductId = "TestId2", Rating = 2 },
                new UserProductReview { ProductId = "TestId3", Rating = 3 },
            };
            var images = new List<ProductImage>
            {
                new ProductImage { ImageUrl = "TestImageUrl1" },
                new ProductImage { ImageUrl = "TestImageUrl2" },
            };

            var products = new List<Product>
            {
                new Product { Id = "TestId1", Name = "TestProduct1", Price = 25, Images = images, Reviews = reviews.Take(2).ToList() },
                new Product { Id = "TestId2", Name = "TestProduct2", Price = 26, Images = images, Reviews = reviews.Skip(2).Take(2).ToList() },
                new Product { Id = "TestId3", Name = "TestProduct3", Price = 27 },
                new Product { Id = "TestId4", Name = "TestProduct4", Price = 28 },
            };

            productRepository.Setup(r => r.AllAsNoTracking()).Returns(products.AsQueryable());
            productReviewRepository.Setup(r => r.AllAsNoTracking()).Returns(reviews.AsQueryable());

            var service = new ProductsService(productRepository.Object, null, null, productReviewRepository.Object);
            var result = service.GetTopRated<ProductSidebarViewModel>(2);
            Assert.Equal(2, result.Count());
            Assert.Equal("TestId1", result.FirstOrDefault().Id);
            Assert.Equal("TestProduct1", result.FirstOrDefault().Name);
            Assert.Equal(25, result.FirstOrDefault().Price);
            Assert.Equal(4, result.FirstOrDefault().AverageRating);
            Assert.Equal("TestImageUrl1", result.FirstOrDefault().ImageUrl);

            Assert.Equal("TestId2", result.LastOrDefault().Id);
            Assert.Equal("TestProduct2", result.LastOrDefault().Name);
            Assert.Equal(26, result.LastOrDefault().Price);
            Assert.Equal(3.5, result.LastOrDefault().AverageRating);
            Assert.Equal("TestImageUrl1", result.LastOrDefault().ImageUrl);

            productRepository.Verify(x => x.AllAsNoTracking(), Times.Exactly(2));
            productReviewRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetTopRatedGenericShouldWorkCorrectlyWithSameRatingsUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var productRepository = new Mock<IDeletableEntityRepository<Product>>();
            var productReviewRepository = new Mock<IRepository<UserProductReview>>();

            var reviews = new List<UserProductReview>
            {
                new UserProductReview { ProductId = "TestId1", Rating = 5 },
                new UserProductReview { ProductId = "TestId1", Rating = 5 },
                new UserProductReview { ProductId = "TestId2", Rating = 5 },
                new UserProductReview { ProductId = "TestId3", Rating = 3 },
            };
            var images = new List<ProductImage>
            {
                new ProductImage { ImageUrl = "TestImageUrl1" },
                new ProductImage { ImageUrl = "TestImageUrl2" },
            };

            var products = new List<Product>
            {
                new Product { Id = "TestId1", Name = "TestProduct1", Price = 25, Images = images, Reviews = reviews.Take(2).ToList() },
                new Product { Id = "TestId2", Name = "TestProduct2", Price = 26, Images = images, Reviews = reviews.Skip(2).Take(1).ToList() },
                new Product { Id = "TestId3", Name = "TestProduct3", Price = 27 },
                new Product { Id = "TestId4", Name = "TestProduct4", Price = 28 },
            };

            productRepository.Setup(r => r.AllAsNoTracking()).Returns(products.AsQueryable());
            productReviewRepository.Setup(r => r.AllAsNoTracking()).Returns(reviews.AsQueryable());

            var service = new ProductsService(productRepository.Object, null, null, productReviewRepository.Object);
            var result = service.GetTopRated<ProductSidebarViewModel>(2);
            Assert.Equal(2, result.Count());
            Assert.Equal("TestId1", result.FirstOrDefault().Id);
            Assert.Equal("TestProduct1", result.FirstOrDefault().Name);
            Assert.Equal(25, result.FirstOrDefault().Price);
            Assert.Equal(5, result.FirstOrDefault().AverageRating);
            Assert.Equal("TestImageUrl1", result.FirstOrDefault().ImageUrl);

            Assert.Equal("TestId2", result.LastOrDefault().Id);
            Assert.Equal("TestProduct2", result.LastOrDefault().Name);
            Assert.Equal(26, result.LastOrDefault().Price);
            Assert.Equal(5, result.LastOrDefault().AverageRating);
            Assert.Equal("TestImageUrl1", result.LastOrDefault().ImageUrl);

            productRepository.Verify(x => x.AllAsNoTracking(), Times.Exactly(2));
            productReviewRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsyncShouldReturnFalseWithInvalidProductIdUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<Product>>();

            var products = new List<Product>
            {
                new Product { Id = "TestId1" },
                new Product { Id = "TestId2" },
                new Product { Id = "TestId3" },
                new Product { Id = "TestId4" },
            };

            repository.Setup(r => r.All()).Returns(products.AsQueryable());

            var service = new ProductsService(repository.Object, null, null, null);
            Assert.False(await service.DeleteAsync("TestId5"));

            repository.Verify(x => x.All(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsyncShouldWorkCorrectlyUsingMoq()
        {
            var productsRepository = new Mock<IDeletableEntityRepository<Product>>();
            var productImagesRepository = new Mock<IDeletableEntityRepository<ProductImage>>();

            var images = new List<ProductImage>
            {
                new ProductImage { Id = "TestImageId1", IsDeleted = false, DeletedOn = null },
                new ProductImage { Id = "TestImageId2", IsDeleted = false, DeletedOn = null },
                new ProductImage { Id = "TestImageId3", IsDeleted = false, DeletedOn = null },
            };

            var products = new List<Product>
            {
                new Product { Id = "TestId1", Images = images, IsDeleted = false, DeletedOn = null },
                new Product { Id = "TestId2" },
                new Product { Id = "TestId3" },
                new Product { Id = "TestId4" },
            };

            productsRepository.Setup(r => r.All()).Returns(products.AsQueryable());
            productsRepository.Setup(r => r.Delete(It.IsAny<Product>())).Callback((Product item) =>
            {
                var foundProduct = products.FirstOrDefault(x => x.Id == item.Id);
                foundProduct.IsDeleted = true;
                foundProduct.DeletedOn = DateTime.UtcNow;
            });
            productsRepository.Setup(r => r.SaveChangesAsync()).Verifiable();

            productImagesRepository.Setup(r => r.Delete(It.IsAny<ProductImage>())).Callback((ProductImage item) =>
            {
                var foundImage = images.FirstOrDefault(x => x.Id == item.Id);
                foundImage.IsDeleted = true;
                foundImage.DeletedOn = DateTime.UtcNow;
            });
            productImagesRepository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new ProductsService(productsRepository.Object, null, productImagesRepository.Object, null);
            Assert.True(await service.DeleteAsync("TestId1"));
            Assert.True(products.FirstOrDefault(x => x.Id == "TestId1").IsDeleted);
            Assert.NotNull(products.FirstOrDefault(x => x.Id == "TestId1").DeletedOn);

            Assert.True(images.FirstOrDefault(x => x.Id == "TestImageId1").IsDeleted);
            Assert.NotNull(images.FirstOrDefault(x => x.Id == "TestImageId1").DeletedOn);
            Assert.True(images.FirstOrDefault(x => x.Id == "TestImageId2").IsDeleted);
            Assert.NotNull(images.FirstOrDefault(x => x.Id == "TestImageId2").DeletedOn);
            Assert.True(images.FirstOrDefault(x => x.Id == "TestImageId3").IsDeleted);
            Assert.NotNull(images.FirstOrDefault(x => x.Id == "TestImageId3").DeletedOn);

            productsRepository.Verify(x => x.All(), Times.Once);
            productsRepository.Verify(x => x.Delete(It.IsAny<Product>()), Times.Once);
            productsRepository.Verify(x => x.SaveChangesAsync());

            productImagesRepository.Verify(x => x.Delete(It.IsAny<ProductImage>()), Times.Exactly(3));
            productImagesRepository.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task UndeleteAsyncShouldReturnFalseWithInvalidProductIdUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<Product>>();

            var products = new List<Product>
            {
                new Product { Id = "TestId1" },
                new Product { Id = "TestId2" },
                new Product { Id = "TestId3" },
                new Product { Id = "TestId4" },
            };

            repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(products.AsQueryable());

            var service = new ProductsService(repository.Object, null, null, null);
            Assert.False(await service.UndeleteAsync("TestId5"));

            repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Once);
        }

        [Fact]
        public async Task UndeleteAsyncShouldWorkCorrectlyUsingMoq()
        {
            var productsRepository = new Mock<IDeletableEntityRepository<Product>>();

            var products = new List<Product>
            {
                new Product { Id = "TestId1", IsDeleted = true, DeletedOn = DateTime.UtcNow },
                new Product { Id = "TestId2" },
                new Product { Id = "TestId3" },
                new Product { Id = "TestId4" },
            };

            productsRepository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(products.AsQueryable());
            productsRepository.Setup(r => r.Undelete(It.IsAny<Product>())).Callback((Product item) =>
            {
                var foundProduct = products.FirstOrDefault(x => x.Id == item.Id);
                foundProduct.IsDeleted = false;
                foundProduct.DeletedOn = null;
            });
            productsRepository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new ProductsService(productsRepository.Object, null, null, null);
            Assert.True(await service.UndeleteAsync("TestId1"));
            Assert.False(products.FirstOrDefault(x => x.Id == "TestId1").IsDeleted);
            Assert.Null(products.FirstOrDefault(x => x.Id == "TestId1").DeletedOn);

            productsRepository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Once);
            productsRepository.Verify(x => x.Undelete(It.IsAny<Product>()), Times.Once);
            productsRepository.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task DeleteImageAsyncShouldReturnFalseWithInvalidImageIdUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<ProductImage>>();

            var images = new List<ProductImage>
            {
                new ProductImage { Id = "TestId1" },
                new ProductImage { Id = "TestId2" },
                new ProductImage { Id = "TestId3" },
                new ProductImage { Id = "TestId4" },
            };

            repository.Setup(r => r.All()).Returns(images.AsQueryable());

            var service = new ProductsService(null, null, repository.Object, null);
            Assert.False(await service.DeleteImageAsync("TestId5"));

            repository.Verify(x => x.All(), Times.Once);
        }

        [Fact]
        public async Task DeleteImageAsyncShouldWorkCorrectlyUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<ProductImage>>();

            var images = new List<ProductImage>
            {
                new ProductImage { Id = "TestId1" },
                new ProductImage { Id = "TestId2" },
                new ProductImage { Id = "TestId3" },
                new ProductImage { Id = "TestId4" },
            };

            repository.Setup(r => r.All()).Returns(images.AsQueryable());
            repository.Setup(r => r.Delete(It.IsAny<ProductImage>())).Callback((ProductImage item) =>
            {
                var foundImage = images.FirstOrDefault(x => x.Id == item.Id);
                foundImage.IsDeleted = true;
                foundImage.DeletedOn = DateTime.UtcNow;
            });
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new ProductsService(null, null, repository.Object, null);
            Assert.True(await service.DeleteImageAsync("TestId1"));
            Assert.True(images.FirstOrDefault(x => x.Id == "TestId1").IsDeleted);
            Assert.NotNull(images.FirstOrDefault(x => x.Id == "TestId1").DeletedOn);

            repository.Verify(x => x.All(), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task DeleteReviewAsyncShouldReturnFalseWithInvalidReviewIdUsingMoq()
        {
            var repository = new Mock<IRepository<UserProductReview>>();

            var reviews = new List<UserProductReview>
            {
                new UserProductReview { Id = "TestId1" },
                new UserProductReview { Id = "TestId2" },
                new UserProductReview { Id = "TestId3" },
                new UserProductReview { Id = "TestId4" },
            };

            repository.Setup(r => r.All()).Returns(reviews.AsQueryable());

            var service = new ProductsService(null, null, null, repository.Object);
            Assert.False(await service.DeleteReviewAsync("TestId5"));

            repository.Verify(x => x.All(), Times.Once);
        }

        [Fact]
        public async Task DeleteReviewAsyncShouldWorkCorrectlyUsingMoq()
        {
            var repository = new Mock<IRepository<UserProductReview>>();

            var reviews = new List<UserProductReview>
            {
                new UserProductReview { Id = "TestId1" },
                new UserProductReview { Id = "TestId2" },
                new UserProductReview { Id = "TestId3" },
                new UserProductReview { Id = "TestId4" },
            };

            repository.Setup(r => r.All()).Returns(reviews.AsQueryable());
            repository.Setup(r => r.Delete(It.IsAny<UserProductReview>())).Callback((UserProductReview item) => reviews.Remove(item));
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new ProductsService(null, null, null, repository.Object);
            Assert.True(await service.DeleteReviewAsync("TestId1"));
            Assert.Equal(3, reviews.Count);

            repository.Verify(x => x.All(), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task CreateAsyncShouldIncreaseCountUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Product>>();

            var products = new List<Product>
            {
                new Product { Id = "TestId1" },
                new Product { Id = "TestId2" },
            };

            repository.Setup(r => r.AddAsync(It.IsAny<Product>())).Callback((Product item) => products.Add(item));
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new ProductsService(repository.Object, null, null, null);
            var model = new CreateProductInputViewModel { };
            await service.CreateAsync(model, null, null, null);
            Assert.Equal(3, products.Count);

            repository.Verify(x => x.AddAsync(It.IsAny<Product>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task CreateAsyncShouldMapCorrectlyWithNoImagesUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Product>>();

            var products = new List<Product>
            {
                new Product { Id = "TestId1" },
                new Product { Id = "TestId2" },
            };

            repository.Setup(r => r.AddAsync(It.IsAny<Product>())).Callback((Product item) => products.Add(item));
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new ProductsService(repository.Object, null, null, null);
            var model = new CreateProductInputViewModel
            {
                Name = "TestProductName",
                Description = "TestProductDescription",
                Price = 42,
                SubcategoryId = 1,
            };
            await service.CreateAsync(model, null, null, null);
            Assert.Equal("TestProductName", products.LastOrDefault().Name);
            Assert.Equal("TestProductDescription", products.LastOrDefault().Description);
            Assert.Equal(42, products.LastOrDefault().Price);
            Assert.Equal(1, products.LastOrDefault().SubcategoryId);

            repository.Verify(x => x.AddAsync(It.IsAny<Product>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task CreateAsyncShouldMapCorrectlyWithImagesUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Product>>();
            var imageService = new Mock<IImagesService>();

            var products = new List<Product>
            {
                new Product { Id = "TestId1" },
                new Product { Id = "TestId2" },
            };

            repository.Setup(r => r.AddAsync(It.IsAny<Product>())).Callback((Product item) => products.Add(item));
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            imageService.Setup(r => r.UploadLocalImageAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
                .Returns(async (IFormFile image, string path) => await Task.FromResult(path + image.FileName));

            var service = new ProductsService(repository.Object, imageService.Object, null, null);
            var model = new CreateProductInputViewModel
            {
                Name = "TestProductName",
                Description = "TestProductDescription",
                Price = 42,
                SubcategoryId = 1,
                UploadedImages = new List<IFormFile>
                {
                    new FormFile(null, 0, 0, "test", "test1.png"),
                    new FormFile(null, 0, 0, "test", "test2.png"),
                },
            };
            await service.CreateAsync(model, model.UploadedImages, "directoryPath\\", "webRootPath\\");
            Assert.Equal("TestProductName", products.LastOrDefault().Name);
            Assert.Equal("TestProductDescription", products.LastOrDefault().Description);
            Assert.Equal(42, products.LastOrDefault().Price);
            Assert.Equal(1, products.LastOrDefault().SubcategoryId);
            Assert.Equal(2, products.LastOrDefault().Images.Count);
            Assert.Equal("directoryPath/test1.png", products.LastOrDefault().Images.FirstOrDefault().ImageUrl);

            repository.Verify(x => x.AddAsync(It.IsAny<Product>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());

            imageService.Verify(x => x.UploadLocalImageAsync(It.IsAny<IFormFile>(), It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task CreateReviewAsyncShouldReturnFalseWithInvalidProductIdUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var productsRepository = new Mock<IDeletableEntityRepository<Product>>();

            var products = new List<Product>
            {
                new Product { Id = "TestId1" },
                new Product { Id = "TestId2" },
            };

            productsRepository.Setup(r => r.All()).Returns(products.AsQueryable());

            var service = new ProductsService(productsRepository.Object, null, null, null);

            var model = new ProductReviewInputViewModel { ProductId = "InvalidId" };
            Assert.False(await service.CreateReviewAsync(model));

            productsRepository.Verify(x => x.All(), Times.Once);
        }

        [Fact]
        public async Task CreateReviewAsyncShouldReturnFalseWithExistingProductReviewUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var productsRepository = new Mock<IDeletableEntityRepository<Product>>();
            var reviewsRepository = new Mock<IRepository<UserProductReview>>();

            var products = new List<Product>
            {
                new Product { Id = "TestProductId1" },
                new Product { Id = "TestProductId2" },
            };

            var reviews = new List<UserProductReview>
            {
                new UserProductReview { UserId = "TestUserId1", ProductId = "TestProductId1" },
                new UserProductReview { UserId = "TestUserId1", ProductId = "TestProductId2" },
            };

            productsRepository.Setup(r => r.All()).Returns(products.AsQueryable());
            reviewsRepository.Setup(r => r.AllAsNoTracking()).Returns(reviews.AsQueryable());

            var service = new ProductsService(productsRepository.Object, null, null, reviewsRepository.Object);

            var model = new ProductReviewInputViewModel { UserId = "TestUserId1", ProductId = "TestProductId1" };
            Assert.False(await service.CreateReviewAsync(model));

            productsRepository.Verify(x => x.All(), Times.Once);
            reviewsRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task CreateReviewAsyncShouldIncreaseCountUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var productsRepository = new Mock<IDeletableEntityRepository<Product>>();
            var reviewsRepository = new Mock<IRepository<UserProductReview>>();

            var products = new List<Product>
            {
                new Product { Id = "TestProductId1" },
                new Product { Id = "TestProductId2" },
            };

            var reviews = new List<UserProductReview>
            {
                new UserProductReview { UserId = "TestUserId1", ProductId = "TestProductId1" },
                new UserProductReview { UserId = "TestUserId1", ProductId = "TestProductId2" },
            };

            productsRepository.Setup(r => r.All()).Returns(products.AsQueryable());
            reviewsRepository.Setup(r => r.AllAsNoTracking()).Returns(reviews.AsQueryable());
            reviewsRepository.Setup(r => r.AddAsync(It.IsAny<UserProductReview>())).Callback((UserProductReview item) => reviews.Add(item));
            reviewsRepository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new ProductsService(productsRepository.Object, null, null, reviewsRepository.Object);

            var model = new ProductReviewInputViewModel
            {
                UserId = "TestUserId2",
                ProductId = "TestProductId1",
                Content = "TestContent",
                Name = "TestName",
                Rating = 5,
            };
            Assert.True(await service.CreateReviewAsync(model));
            Assert.Equal(3, reviews.Count);

            productsRepository.Verify(x => x.All(), Times.Once);
            reviewsRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
            reviewsRepository.Verify(x => x.AddAsync(It.IsAny<UserProductReview>()), Times.Once);
            reviewsRepository.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task CreateReviewAsyncShouldWorkCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var productsRepository = new Mock<IDeletableEntityRepository<Product>>();
            var reviewsRepository = new Mock<IRepository<UserProductReview>>();

            var products = new List<Product>
            {
                new Product { Id = "TestProductId1" },
                new Product { Id = "TestProductId2" },
            };

            var reviews = new List<UserProductReview>
            {
                new UserProductReview { UserId = "TestUserId1", ProductId = "TestProductId1" },
                new UserProductReview { UserId = "TestUserId1", ProductId = "TestProductId2" },
            };

            productsRepository.Setup(r => r.All()).Returns(products.AsQueryable());
            reviewsRepository.Setup(r => r.AllAsNoTracking()).Returns(reviews.AsQueryable());
            reviewsRepository.Setup(r => r.AddAsync(It.IsAny<UserProductReview>())).Callback((UserProductReview item) => reviews.Add(item));
            reviewsRepository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new ProductsService(productsRepository.Object, null, null, reviewsRepository.Object);

            var model = new ProductReviewInputViewModel
            {
                UserId = "TestUserId2",
                ProductId = "TestProductId1",
                Content = "TestContent",
                Name = "TestName",
                Rating = 5,
            };
            Assert.True(await service.CreateReviewAsync(model));
            Assert.Equal("TestUserId2", reviews.LastOrDefault().UserId);
            Assert.Equal("TestProductId1", reviews.LastOrDefault().ProductId);
            Assert.Equal("TestContent", reviews.LastOrDefault().Content);
            Assert.Equal("TestName", reviews.LastOrDefault().Name);
            Assert.Equal(5, reviews.LastOrDefault().Rating);

            productsRepository.Verify(x => x.All(), Times.Once);
            reviewsRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
            reviewsRepository.Verify(x => x.AddAsync(It.IsAny<UserProductReview>()), Times.Once);
            reviewsRepository.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task EditAsyncShouldReturnFalseWithInvalidProductIdUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Product>>();

            var products = new List<Product>
            {
                new Product { Id = "TestId1" },
                new Product { Id = "TestId2" },
            };

            repository.Setup(r => r.All()).Returns(products.AsQueryable());

            var service = new ProductsService(repository.Object, null, null, null);
            var model = new EditProductViewModel { Id = "InvalidId" };
            Assert.False(await service.EditAsync(model, null, null, null));

            repository.Verify(x => x.All(), Times.Once);
        }

        [Fact]
        public async Task EditAsyncShouldWorkCorrectlyWithNoImagesUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Product>>();

            var products = new List<Product>
            {
                new Product { Id = "TestId1" },
                new Product { Id = "TestId2" },
            };

            repository.Setup(r => r.All()).Returns(products.AsQueryable());
            repository.Setup(r => r.Update(It.IsAny<Product>())).Callback((Product item) =>
            {
                var foundProduct = products.FirstOrDefault(x => x.Id == item.Id);
                foundProduct.Name = item.Name;
                foundProduct.Description = item.Description;
                foundProduct.Price = item.Price;
                foundProduct.SubcategoryId = item.SubcategoryId;
            });
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new ProductsService(repository.Object, null, null, null);
            var model = new EditProductViewModel
            {
                Id = "TestId1",
                Name = "TestNameEdited",
                Description = "TestDescriptionEdited",
                Price = 42,
                SubcategoryId = 1,
            };
            Assert.True(await service.EditAsync(model, null, null, null));
            Assert.Equal("TestNameEdited", products.FirstOrDefault(x => x.Id == model.Id).Name);
            Assert.Equal("TestDescriptionEdited", products.FirstOrDefault(x => x.Id == model.Id).Description);
            Assert.Equal(42, products.FirstOrDefault(x => x.Id == model.Id).Price);
            Assert.Equal(1, products.FirstOrDefault(x => x.Id == model.Id).SubcategoryId);

            repository.Verify(x => x.All(), Times.Once);
            repository.Verify(x => x.Update(It.IsAny<Product>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task EditAsyncShouldWorkCorrectlyWithImagesUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Product>>();
            var imageService = new Mock<IImagesService>();

            var images = new List<ProductImage>
            {
                new ProductImage { ImageUrl = "TestImageUrl1" },
            };
            var products = new List<Product>
            {
                new Product { Id = "TestId1", Images = images },
                new Product { Id = "TestId2" },
            };

            repository.Setup(r => r.All()).Returns(products.AsQueryable());
            repository.Setup(r => r.Update(It.IsAny<Product>())).Callback((Product item) =>
            {
                var foundProduct = products.FirstOrDefault(x => x.Id == item.Id);
                foundProduct.Name = item.Name;
                foundProduct.Description = item.Description;
                foundProduct.Price = item.Price;
                foundProduct.SubcategoryId = item.SubcategoryId;
            });
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            imageService.Setup(r => r.UploadLocalImageAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
                .Returns(async (IFormFile image, string path) => await Task.FromResult(path + image.FileName));

            var service = new ProductsService(repository.Object, imageService.Object, null, null);
            var model = new EditProductViewModel
            {
                Id = "TestId1",
                Name = "TestNameEdited",
                Description = "TestDescriptionEdited",
                Price = 42,
                SubcategoryId = 1,
                UploadedImages = new List<IFormFile>
                {
                    new FormFile(null, 0, 0, "test", "test1.png"),
                    new FormFile(null, 0, 0, "test", "test2.png"),
                },
            };
            Assert.True(await service.EditAsync(model, model.UploadedImages, "directoryPath\\", "webRootPath\\"));
            Assert.Equal("TestNameEdited", products.FirstOrDefault(x => x.Id == model.Id).Name);
            Assert.Equal("TestDescriptionEdited", products.FirstOrDefault(x => x.Id == model.Id).Description);
            Assert.Equal(42, products.FirstOrDefault(x => x.Id == model.Id).Price);
            Assert.Equal(1, products.FirstOrDefault(x => x.Id == model.Id).SubcategoryId);
            Assert.Equal(3, images.Count);
            Assert.Equal("directoryPath/test1.png", images.ElementAt(1).ImageUrl);
            Assert.Equal("directoryPath/test2.png", images.ElementAt(2).ImageUrl);

            repository.Verify(x => x.All(), Times.Once);
            repository.Verify(x => x.Update(It.IsAny<Product>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public void GetProductsCountBySearchStringAndMainCategoryIdShouldWorkCorrectlyOnlyWithSearchStringUsingMoq()
        {
            var productsRepository = new Mock<IDeletableEntityRepository<Product>>();

            var products = new List<Product>
            {
                new Product { Id = "TestId1", Name = "Test Name 1" },
                new Product { Id = "TestId2", Name = "Test Name 2" },
            };

            productsRepository.Setup(r => r.AllAsNoTracking()).Returns(products.AsQueryable());

            var service = new ProductsService(productsRepository.Object, null, null, null);

            Assert.Equal(2, service.GetProductsCountBySearchStringAndMainCategoryId("name", null));

            productsRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetProductsCountBySearchStringAndMainCategoryIdShouldWorkCorrectlyWithSearchStringAndMainCategoryIdUsingMoq()
        {
            var productsRepository = new Mock<IDeletableEntityRepository<Product>>();

            var subcategory = new Subcategory { Id = 1, MainCategoryId = 1 };
            var products = new List<Product>
            {
                new Product { Id = "TestId1", Name = "Test Name 1", Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId2", Name = "Test Name 2", Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId3", Name = "Test Name 3", Subcategory = new Subcategory { } },
            };

            productsRepository.Setup(r => r.AllAsNoTracking()).Returns(products.AsQueryable());

            var service = new ProductsService(productsRepository.Object, null, null, null);

            Assert.Equal(2, service.GetProductsCountBySearchStringAndMainCategoryId("name", subcategory.MainCategoryId));

            productsRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void TakeProductsBySubcategoryIdGenericShouldWorkCorrectlyWithSubcategoryIdUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var productsRepository = new Mock<IDeletableEntityRepository<Product>>();

            var mainCategory = new MainCategory { Id = 1, Name = "TestMainCategory" };
            var subcategory = new Subcategory { Id = 1, Name = "TestSubcategory", MainCategory = mainCategory, MainCategoryId = 1 };
            var products = new List<Product>
            {
                new Product { Id = "TestId1", Name = "Test Name 1", Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId2", Name = "Test Name 2", Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId3", Name = "Test Name 3", Subcategory = new Subcategory { } },
            };

            productsRepository.Setup(r => r.AllAsNoTracking()).Returns(products.AsQueryable());

            var service = new ProductsService(productsRepository.Object, null, null, null);

            Assert.Equal(2, service.TakeProductsBySubcategoryId<ProductViewModel>(subcategory.Id, 1, 3, "price asc").Count());

            productsRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Theory]
        [InlineData(1, 2, 2)]
        [InlineData(1, 7, 6)]
        [InlineData(2, 3, 3)]
        [InlineData(6, 1, 1)]
        [InlineData(7, 1, 0)]
        public void TakeProductsBySubcategoryIdGenericShouldWorkCorrectlyWithPagingUsingMoq(int page, int productsToTake, int count)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var productsRepository = new Mock<IDeletableEntityRepository<Product>>();

            var mainCategory = new MainCategory { Id = 1, Name = "TestMainCategory" };
            var subcategory = new Subcategory { Id = 1, Name = "TestSubcategory", MainCategory = mainCategory, MainCategoryId = 1 };
            var products = new List<Product>
            {
                new Product { Id = "TestId1", Name = "Test Name 1", Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId2", Name = "Test Name 2", Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId3", Name = "Test Name 3", Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId4", Name = "Test Name 4", Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId5", Name = "Test Name 5", Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId6", Name = "Test Name 6", Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId7", Name = "Test Name 7", Subcategory = new Subcategory { } },
            };

            productsRepository.Setup(r => r.AllAsNoTracking()).Returns(products.AsQueryable());

            var service = new ProductsService(productsRepository.Object, null, null, null);

            Assert.Equal(count, service.TakeProductsBySubcategoryId<ProductViewModel>(subcategory.Id, page, productsToTake, "price asc").Count());

            productsRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Theory]
        [InlineData("price asc", "TestId4", "TestId6")]
        [InlineData("price desc", "TestId6", "TestId4")]
        [InlineData("newest", "TestId2", "TestId5")]
        [InlineData("oldest", "TestId5", "TestId2")]
        public void TakeProductsBySubcategoryIdGenericShouldWorkCorrectlyWithSortingUsingMoq(string sorting, string firstId, string lastId)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var productsRepository = new Mock<IDeletableEntityRepository<Product>>();

            var mainCategory = new MainCategory { Id = 1, Name = "TestMainCategory" };
            var subcategory = new Subcategory { Id = 1, Name = "TestSubcategory", MainCategory = mainCategory, MainCategoryId = 1 };
            var products = new List<Product>
            {
                new Product { Id = "TestId1", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 12), Price = 42, Name = "Test Name 1", Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId2", CreatedOn = new DateTime(2020, 12, 31, 15, 12, 13), Price = 123, Name = "Test Name 2", Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId3", CreatedOn = new DateTime(2020, 12, 31, 12, 16, 14), Price = 111, Name = "Test Name 3", Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId4", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 15), Price = 23, Name = "Test Name 4", Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId5", CreatedOn = new DateTime(2020, 12, 31, 12, 11, 12), Price = 56, Name = "Test Name 5", Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId6", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 17), Price = 142, Name = "Test Name 6", Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId7", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 18), Price = 1500, Name = "Test Name 7", Subcategory = new Subcategory { } },
            };

            productsRepository.Setup(r => r.AllAsNoTracking()).Returns(products.AsQueryable());

            var service = new ProductsService(productsRepository.Object, null, null, null);

            Assert.Equal(firstId, service.TakeProductsBySubcategoryId<ProductViewModel>(subcategory.Id, 1, 10, sorting).FirstOrDefault().Id);
            Assert.Equal(lastId, service.TakeProductsBySubcategoryId<ProductViewModel>(subcategory.Id, 1, 10, sorting).LastOrDefault().Id);

            productsRepository.Verify(x => x.AllAsNoTracking(), Times.Exactly(2));
        }

        [Fact]
        public void TakeProductsBySearchStringAndMainCategoryIdGenericShouldWorkCorrectlyWithSearchStringUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var productsRepository = new Mock<IDeletableEntityRepository<Product>>();

            var mainCategory = new MainCategory { Id = 1, Name = "TestMainCategory" };
            var subcategory = new Subcategory { Id = 1, Name = "TestSubcategory", MainCategory = mainCategory, MainCategoryId = 1 };
            var products = new List<Product>
            {
                new Product { Id = "TestId1", Name = "Test Name 1", Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId2", Name = "Test Name 2", Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId3", Name = "Test Name 3", Subcategory = subcategory },
                new Product { Id = "TestId3", Name = "Other Name 4", Subcategory = subcategory },
            };

            productsRepository.Setup(r => r.AllAsNoTracking()).Returns(products.AsQueryable());

            var service = new ProductsService(productsRepository.Object, null, null, null);

            Assert.Equal(3, service.TakeProductsBySearchStringAndMainCategoryId<ProductViewModel>("test", null, 1, 3, "price asc").Count());

            productsRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void TakeProductsBySearchStringAndMainCategoryIdGenericShouldWorkCorrectlyWithSearchStringAndMainCategoryIdUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var productsRepository = new Mock<IDeletableEntityRepository<Product>>();

            var mainCategory = new MainCategory { Id = 1, Name = "TestMainCategory" };
            var subcategory = new Subcategory { Id = 1, Name = "TestSubcategory", MainCategory = mainCategory, MainCategoryId = 1 };
            var subcategory2 = new Subcategory { Id = 1, Name = "TestSubcategory", MainCategory = mainCategory, MainCategoryId = 2 };
            var products = new List<Product>
            {
                new Product { Id = "TestId1", Name = "Test Name 1", Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId2", Name = "Test Name 2", Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId3", Name = "Test Name 3", Subcategory = subcategory2 },
                new Product { Id = "TestId3", Name = "Other Name 4", Subcategory = subcategory2 },
            };

            productsRepository.Setup(r => r.AllAsNoTracking()).Returns(products.AsQueryable());

            var service = new ProductsService(productsRepository.Object, null, null, null);

            Assert.Equal(2, service.TakeProductsBySearchStringAndMainCategoryId<ProductViewModel>("test", mainCategory.Id, 1, 3, "price asc").Count());

            productsRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Theory]
        [InlineData(1, 2, 2)]
        [InlineData(1, 7, 6)]
        [InlineData(2, 3, 3)]
        [InlineData(6, 1, 1)]
        [InlineData(7, 1, 0)]
        public void TakeProductsBySearchStringAndMainCategoryIdGenericShouldWorkCorrectlyWithPagingUsingMoq(int page, int productsToTake, int count)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var productsRepository = new Mock<IDeletableEntityRepository<Product>>();

            var mainCategory = new MainCategory { Id = 1, Name = "TestMainCategory" };
            var subcategory = new Subcategory { Id = 1, Name = "TestSubcategory", MainCategory = mainCategory, MainCategoryId = 1 };
            var products = new List<Product>
            {
                new Product { Id = "TestId1", Name = "Test Name 1", Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId2", Name = "Test Name 2", Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId3", Name = "Test Name 3", Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId4", Name = "Test Name 4", Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId5", Name = "Test Name 5", Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId6", Name = "Test Name 6", Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId7", Name = "Test Name 7", Subcategory = new Subcategory { } },
            };

            productsRepository.Setup(r => r.AllAsNoTracking()).Returns(products.AsQueryable());

            var service = new ProductsService(productsRepository.Object, null, null, null);

            Assert.Equal(count, service.TakeProductsBySearchStringAndMainCategoryId<ProductViewModel>("test", mainCategory.Id, page, productsToTake, "price asc").Count());

            productsRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Theory]
        [InlineData("price asc", "TestId4", "TestId6")]
        [InlineData("price desc", "TestId6", "TestId4")]
        [InlineData("newest", "TestId2", "TestId5")]
        [InlineData("oldest", "TestId5", "TestId2")]
        public void TakeProductsBySearchStringAndMainCategoryIdGenericShouldWorkCorrectlyWithSortingUsingMoq(string sorting, string firstId, string lastId)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var productsRepository = new Mock<IDeletableEntityRepository<Product>>();

            var mainCategory = new MainCategory { Id = 1, Name = "TestMainCategory" };
            var subcategory = new Subcategory { Id = 1, Name = "TestSubcategory", MainCategory = mainCategory, MainCategoryId = 1 };
            var products = new List<Product>
            {
                new Product { Id = "TestId1", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 12), Price = 42, Name = "Test Name 1", Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId2", CreatedOn = new DateTime(2020, 12, 31, 15, 12, 13), Price = 123, Name = "Test Name 2", Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId3", CreatedOn = new DateTime(2020, 12, 31, 12, 16, 14), Price = 111, Name = "Test Name 3", Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId4", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 15), Price = 23, Name = "Test Name 4", Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId5", CreatedOn = new DateTime(2020, 12, 31, 12, 11, 12), Price = 56, Name = "Test Name 5", Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId6", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 17), Price = 142, Name = "Test Name 6", Subcategory = subcategory, SubcategoryId = subcategory.Id },
                new Product { Id = "TestId7", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 18), Price = 1500, Name = "Test Name 7", Subcategory = new Subcategory { } },
            };

            productsRepository.Setup(r => r.AllAsNoTracking()).Returns(products.AsQueryable());

            var service = new ProductsService(productsRepository.Object, null, null, null);

            Assert.Equal(firstId, service.TakeProductsBySearchStringAndMainCategoryId<ProductViewModel>("test", mainCategory.Id, 1, 10, sorting).FirstOrDefault().Id);
            Assert.Equal(lastId, service.TakeProductsBySearchStringAndMainCategoryId<ProductViewModel>("test", mainCategory.Id, 1, 10, sorting).LastOrDefault().Id);

            productsRepository.Verify(x => x.AllAsNoTracking(), Times.Exactly(2));
        }
    }
}
