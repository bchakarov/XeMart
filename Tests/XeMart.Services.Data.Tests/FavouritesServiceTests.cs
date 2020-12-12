namespace XeMart.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using Moq;

    using XeMart.Data.Common.Repositories;
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;
    using XeMart.Web.ViewModels;
    using XeMart.Web.ViewModels.Favourites;
    using Xunit;

    public class FavouritesServiceTests
    {
        [Fact]
        public void GetCountShouldWorkCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IRepository<UserFavouriteProduct>>();

            var productsList = new List<UserFavouriteProduct>
            {
                new UserFavouriteProduct { Id = "TestId1", UserId = "TestUserId1" },
                new UserFavouriteProduct { Id = "TestId2", UserId = "TestUserId2" },
                new UserFavouriteProduct { Id = "TestId3", UserId = "TestUserId1" },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(productsList.AsQueryable());

            var service = new FavouritesService(repository.Object, null, null);
            Assert.Equal(2, service.GetCount("TestUserId1"));

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetCountShouldReturnEmptyCollectionWithNonExistingUserIdUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IRepository<UserFavouriteProduct>>();

            var productsList = new List<UserFavouriteProduct>
            {
                new UserFavouriteProduct { Id = "TestId1", UserId = "TestUserId1" },
                new UserFavouriteProduct { Id = "TestId2", UserId = "TestUserId2" },
                new UserFavouriteProduct { Id = "TestId3", UserId = "TestUserId3" },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(productsList.AsQueryable());

            var service = new FavouritesService(repository.Object, null, null);
            Assert.Equal(0, service.GetCount("TestUserId4"));

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetAllGenericShouldReturnCorrectCountUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IRepository<UserFavouriteProduct>>();

            var productImages = new List<ProductImage>
            {
                new ProductImage { ImageUrl = "TestImageUrl1" },
                new ProductImage { ImageUrl = "TestImageUrl2" },
            };

            var product = new Product
            {
                Id = "TestProductId",
                Name = "TestProductName",
                Price = 42,
                Images = productImages,
                Reviews = new List<UserProductReview>(),
            };

            var productsList = new List<UserFavouriteProduct>
            {
                new UserFavouriteProduct { Id = "TestId1", UserId = "TestUserId1", Product = product, ProductId = "TestProductId" },
                new UserFavouriteProduct { Id = "TestId2", UserId = "TestUserId1", Product = product, ProductId = "TestProductId" },
                new UserFavouriteProduct { Id = "TestId3", UserId = "TestUserId3" },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(productsList.AsQueryable());

            var service = new FavouritesService(repository.Object, null, null);
            Assert.Equal(2, service.GetAll<FavouriteProductViewModel>("TestUserId1").Count());

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetAllGenericShouldMapCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IRepository<UserFavouriteProduct>>();

            var productImages = new List<ProductImage>
            {
                new ProductImage { ImageUrl = "TestImageUrl1" },
                new ProductImage { ImageUrl = "TestImageUrl2" },
            };

            var productReviews = new List<UserProductReview>
            {
                new UserProductReview { Rating = 5 },
                new UserProductReview { Rating = 2 },
                new UserProductReview { Rating = 1 },
            };

            var product = new Product
            {
                Id = "TestProductId",
                Name = "TestProductName",
                Price = 42,
                Images = productImages,
                Reviews = productReviews,
            };

            var productsList = new List<UserFavouriteProduct>
            {
                new UserFavouriteProduct { Id = "TestId1", UserId = "TestUserId1", Product = product, ProductId = "TestProductId" },
                new UserFavouriteProduct { Id = "TestId2", UserId = "TestUserId2" },
                new UserFavouriteProduct { Id = "TestId3", UserId = "TestUserId3" },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(productsList.AsQueryable());

            var service = new FavouritesService(repository.Object, null, null);
            Assert.Equal("TestProductId", service.GetAll<FavouriteProductViewModel>("TestUserId1").FirstOrDefault().ProductId);
            Assert.Equal("TestProductName", service.GetAll<FavouriteProductViewModel>("TestUserId1").FirstOrDefault().ProductName);
            Assert.Equal(42, service.GetAll<FavouriteProductViewModel>("TestUserId1").FirstOrDefault().ProductPrice);
            Assert.Equal("TestImageUrl1", service.GetAll<FavouriteProductViewModel>("TestUserId1").FirstOrDefault().ImageUrl);
            Assert.Equal(2.67, service.GetAll<FavouriteProductViewModel>("TestUserId1").FirstOrDefault().AverageRating);

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(5));
        }

        [Fact]
        public void GetAllGenericShouldMapCorrectlyWhenThereAreNoReviewsUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IRepository<UserFavouriteProduct>>();

            var productImages = new List<ProductImage>
            {
                new ProductImage { ImageUrl = "TestImageUrl1" },
                new ProductImage { ImageUrl = "TestImageUrl2" },
            };

            var product = new Product
            {
                Id = "TestProductId",
                Name = "TestProductName",
                Price = 42,
                Images = productImages,
                Reviews = new List<UserProductReview>(),
            };

            var productsList = new List<UserFavouriteProduct>
            {
                new UserFavouriteProduct { Id = "TestId1", UserId = "TestUserId1", Product = product, ProductId = "TestProductId" },
                new UserFavouriteProduct { Id = "TestId2", UserId = "TestUserId2" },
                new UserFavouriteProduct { Id = "TestId3", UserId = "TestUserId3" },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(productsList.AsQueryable());

            var service = new FavouritesService(repository.Object, null, null);
            Assert.Equal("TestProductId", service.GetAll<FavouriteProductViewModel>("TestUserId1").FirstOrDefault().ProductId);
            Assert.Equal("TestProductName", service.GetAll<FavouriteProductViewModel>("TestUserId1").FirstOrDefault().ProductName);
            Assert.Equal(42, service.GetAll<FavouriteProductViewModel>("TestUserId1").FirstOrDefault().ProductPrice);
            Assert.Equal("TestImageUrl1", service.GetAll<FavouriteProductViewModel>("TestUserId1").FirstOrDefault().ImageUrl);
            Assert.Equal(0.00, service.GetAll<FavouriteProductViewModel>("TestUserId1").FirstOrDefault().AverageRating);

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(5));
        }

        [Fact]
        public async Task DeleteAsyncShouldReturnFalseWhenProductIdDoesNotExistUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var favouriteProductsRepository = new Mock<IRepository<UserFavouriteProduct>>();
            var productsRepository = new Mock<IDeletableEntityRepository<Product>>();

            var productImages = new List<ProductImage>
            {
                new ProductImage { ImageUrl = "TestImageUrl1" },
                new ProductImage { ImageUrl = "TestImageUrl2" },
            };

            var productsList = new List<Product>
            {
                new Product { Id = "TestProductId", Name = "TestProductName", Price = 42, Images = productImages, Reviews = new List<UserProductReview>() },
            };

            var favouriteProductsList = new List<UserFavouriteProduct>
            {
                new UserFavouriteProduct { Id = "TestId1", UserId = "TestUserId1" },
                new UserFavouriteProduct { Id = "TestId2", UserId = "TestUserId2" },
                new UserFavouriteProduct { Id = "TestId3", UserId = "TestUserId3" },
            };

            favouriteProductsRepository.Setup(r => r.AllAsNoTracking()).Returns(favouriteProductsList.AsQueryable());
            productsRepository.Setup(r => r.AllAsNoTracking()).Returns(productsList.AsQueryable());

            var service = new FavouritesService(favouriteProductsRepository.Object, productsRepository.Object, null);
            Assert.False(await service.DeleteAsync("InvalidId", "TestUserId"));

            productsRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsyncShouldReturnFalseWhenUserIdDoesNotExistUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var favouriteProductsRepository = new Mock<IRepository<UserFavouriteProduct>>();
            var productsRepository = new Mock<IDeletableEntityRepository<Product>>();

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            var productImages = new List<ProductImage>
            {
                new ProductImage { ImageUrl = "TestImageUrl1" },
                new ProductImage { ImageUrl = "TestImageUrl2" },
            };

            var productsList = new List<Product>
            {
                new Product { Id = "TestProductId", Name = "TestProductName", Price = 42, Images = productImages, Reviews = new List<UserProductReview>() },
            };

            var favouriteProductsList = new List<UserFavouriteProduct>
            {
                new UserFavouriteProduct { Id = "TestId1", UserId = "TestUserId1", Product = productsList.ElementAt(0), ProductId = "TestProductId" },
                new UserFavouriteProduct { Id = "TestId2", UserId = "TestUserId2" },
                new UserFavouriteProduct { Id = "TestId3", UserId = "TestUserId3" },
            };

            favouriteProductsRepository.Setup(r => r.AllAsNoTracking()).Returns(favouriteProductsList.AsQueryable());
            productsRepository.Setup(r => r.AllAsNoTracking()).Returns(productsList.AsQueryable());
            userManager.Setup(r => r.FindByIdAsync(It.IsAny<string>())).Returns(async (string userId) => await Task.FromResult<ApplicationUser>(null));

            var service = new FavouritesService(favouriteProductsRepository.Object, productsRepository.Object, userManager.Object);
            Assert.False(await service.DeleteAsync("TestProductId", "IvalidUserId"));

            productsRepository.Verify(x => x.AllAsNoTracking(), Times.Once);

            userManager.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsyncShouldReturnFalseWhenFavouriteProductIdDoesNotExistsUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var favouriteProductsRepository = new Mock<IRepository<UserFavouriteProduct>>();
            var productsRepository = new Mock<IDeletableEntityRepository<Product>>();

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            var productImages = new List<ProductImage>
            {
                new ProductImage { ImageUrl = "TestImageUrl1" },
                new ProductImage { ImageUrl = "TestImageUrl2" },
            };

            var productsList = new List<Product>
            {
                new Product { Id = "TestProductId", Name = "TestProductName", Price = 42, Images = productImages, Reviews = new List<UserProductReview>() },
                new Product { Id = "TestProductId2", Name = "TestProductName", Price = 42, Images = productImages, Reviews = new List<UserProductReview>() },
            };

            var favouriteProductsList = new List<UserFavouriteProduct>
            {
                new UserFavouriteProduct { Id = "TestId1", UserId = "TestUserId1", Product = productsList.ElementAt(0), ProductId = "TestProductId" },
                new UserFavouriteProduct { Id = "TestId2", UserId = "TestUserId2" },
                new UserFavouriteProduct { Id = "TestId3", UserId = "TestUserId3" },
            };

            favouriteProductsRepository.Setup(r => r.AllAsNoTracking()).Returns(favouriteProductsList.AsQueryable());
            productsRepository.Setup(r => r.AllAsNoTracking()).Returns(productsList.AsQueryable());
            userManager.Setup(r => r.FindByIdAsync(It.IsAny<string>()))
                .Returns(async (string userId) => await Task.FromResult<ApplicationUser>(new ApplicationUser { Id = "TestUserId1" }));

            var service = new FavouritesService(favouriteProductsRepository.Object, productsRepository.Object, userManager.Object);
            Assert.False(await service.DeleteAsync("TestProductId2", "TestUserId1"));

            productsRepository.Verify(x => x.AllAsNoTracking(), Times.Once);

            userManager.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Once);

            favouriteProductsRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsyncShouldWorkCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var favouriteProductsRepository = new Mock<IRepository<UserFavouriteProduct>>();
            var productsRepository = new Mock<IDeletableEntityRepository<Product>>();

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            var productImages = new List<ProductImage>
            {
                new ProductImage { ImageUrl = "TestImageUrl1" },
                new ProductImage { ImageUrl = "TestImageUrl2" },
            };

            var productsList = new List<Product>
            {
                new Product { Id = "TestProductId", Name = "TestProductName", Price = 42, Images = productImages, Reviews = new List<UserProductReview>() },
            };

            var favouriteProductsList = new List<UserFavouriteProduct>
            {
                new UserFavouriteProduct { Id = "TestId1", UserId = "TestUserId1", Product = productsList.ElementAt(0), ProductId = "TestProductId" },
                new UserFavouriteProduct { Id = "TestId2", UserId = "TestUserId2" },
                new UserFavouriteProduct { Id = "TestId3", UserId = "TestUserId3" },
            };

            favouriteProductsRepository.Setup(r => r.AllAsNoTracking()).Returns(favouriteProductsList.AsQueryable());
            favouriteProductsRepository.Setup(r => r.SaveChangesAsync()).Verifiable();
            favouriteProductsRepository.Setup(r => r.Delete(It.IsAny<UserFavouriteProduct>()))
                .Callback((UserFavouriteProduct item) => favouriteProductsList.Remove(item));

            productsRepository.Setup(r => r.AllAsNoTracking()).Returns(productsList.AsQueryable());

            userManager.Setup(r => r.FindByIdAsync(It.IsAny<string>()))
                .Returns(async (string userId) => await Task.FromResult<ApplicationUser>(new ApplicationUser { Id = "TestUserId1" }));

            var service = new FavouritesService(favouriteProductsRepository.Object, productsRepository.Object, userManager.Object);
            Assert.True(await service.DeleteAsync("TestProductId", "TestUserId1"));
            Assert.Equal(2, favouriteProductsList.Count);

            productsRepository.Verify(x => x.AllAsNoTracking(), Times.Once);

            userManager.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Once);

            favouriteProductsRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
            favouriteProductsRepository.Verify(x => x.Delete(It.IsAny<UserFavouriteProduct>()), Times.Once);
            favouriteProductsRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task AddAsyncShouldReturnFalseWhenProductIdDoesNotExistUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var favouriteProductsRepository = new Mock<IRepository<UserFavouriteProduct>>();
            var productsRepository = new Mock<IDeletableEntityRepository<Product>>();

            var productImages = new List<ProductImage>
            {
                new ProductImage { ImageUrl = "TestImageUrl1" },
                new ProductImage { ImageUrl = "TestImageUrl2" },
            };

            var productsList = new List<Product>
            {
                new Product { Id = "TestProductId", Name = "TestProductName", Price = 42, Images = productImages, Reviews = new List<UserProductReview>() },
            };

            productsRepository.Setup(r => r.AllAsNoTracking()).Returns(productsList.AsQueryable());

            var service = new FavouritesService(favouriteProductsRepository.Object, productsRepository.Object, null);
            Assert.False(await service.AddAsync("InvalidId", "TestUserId"));

            productsRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task AddAsyncShouldReturnFalseWhenUserIdDoesNotExistUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var favouriteProductsRepository = new Mock<IRepository<UserFavouriteProduct>>();
            var productsRepository = new Mock<IDeletableEntityRepository<Product>>();

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            var productImages = new List<ProductImage>
            {
                new ProductImage { ImageUrl = "TestImageUrl1" },
                new ProductImage { ImageUrl = "TestImageUrl2" },
            };

            var productsList = new List<Product>
            {
                new Product { Id = "TestProductId", Name = "TestProductName", Price = 42, Images = productImages, Reviews = new List<UserProductReview>() },
            };

            productsRepository.Setup(r => r.AllAsNoTracking()).Returns(productsList.AsQueryable());
            userManager.Setup(r => r.FindByIdAsync(It.IsAny<string>())).Returns(async (string userId) => await Task.FromResult<ApplicationUser>(null));

            var service = new FavouritesService(favouriteProductsRepository.Object, productsRepository.Object, userManager.Object);
            Assert.False(await service.AddAsync("TestProductId", "IvalidUserId"));

            productsRepository.Verify(x => x.AllAsNoTracking(), Times.Once);

            userManager.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task AddAsyncShouldWorkCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var favouriteProductsRepository = new Mock<IRepository<UserFavouriteProduct>>();
            var productsRepository = new Mock<IDeletableEntityRepository<Product>>();

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            var productImages = new List<ProductImage>
            {
                new ProductImage { ImageUrl = "TestImageUrl1" },
                new ProductImage { ImageUrl = "TestImageUrl2" },
            };

            var productsList = new List<Product>
            {
                new Product { Id = "TestProductId", Name = "TestProductName", Price = 42, Images = productImages, Reviews = new List<UserProductReview>() },
            };

            var favouriteProductsList = new List<UserFavouriteProduct>();

            favouriteProductsRepository.Setup(r => r.SaveChangesAsync()).Verifiable();
            favouriteProductsRepository.Setup(r => r.AddAsync(It.IsAny<UserFavouriteProduct>()))
                .Callback((UserFavouriteProduct item) => favouriteProductsList.Add(item));

            productsRepository.Setup(r => r.AllAsNoTracking()).Returns(productsList.AsQueryable());

            userManager.Setup(r => r.FindByIdAsync(It.IsAny<string>()))
                .Returns(async (string userId) => await Task.FromResult<ApplicationUser>(new ApplicationUser { Id = "TestUserId1" }));

            var service = new FavouritesService(favouriteProductsRepository.Object, productsRepository.Object, userManager.Object);
            Assert.True(await service.AddAsync("TestProductId", "TestUserId1"));
            Assert.Single(favouriteProductsList);
            Assert.Equal("TestProductId", favouriteProductsList.FirstOrDefault().ProductId);
            Assert.Equal("TestUserId1", favouriteProductsList.FirstOrDefault().UserId);

            productsRepository.Verify(x => x.AllAsNoTracking(), Times.Once);

            userManager.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Once);

            favouriteProductsRepository.Verify(x => x.AddAsync(It.IsAny<UserFavouriteProduct>()), Times.Once);
            favouriteProductsRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
