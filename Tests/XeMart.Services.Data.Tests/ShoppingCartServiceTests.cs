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
    using XeMart.Web.ViewModels.ShoppingCart;

    using Xunit;

    [Collection("Sequential")]
    public class ShoppingCartServiceTests
    {
        [Fact]
        public async Task AddProductAsyncUserAuthenticatedShouldReturnFalseWithExistingShoppingCartIdAndProductIdUsingMoq()
        {
            var repository = new Mock<IRepository<ShoppingCartProduct>>();

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "TestUserId1", ShoppingCartId = "TestShoppingCartId1" },
            };

            var shoppingCartProducts = new List<ShoppingCartProduct>
            {
                new ShoppingCartProduct { Id = "TestId1", ProductId = "TestProductId1", ShoppingCartId = "TestShoppingCartId1" },
            };

            repository.Setup(r => r.All()).Returns(shoppingCartProducts.AsQueryable());

            userManager.Setup(r => r.FindByIdAsync(It.IsAny<string>()))
                .Returns(async (string userId) => await Task.FromResult<ApplicationUser>(users.FirstOrDefault(x => x.Id == userId)));

            var service = new ShoppingCartService(repository.Object, userManager.Object, null);

            Assert.False(await service.AddProductAsync(true, null, "TestUserId1", "TestProductId1"));

            repository.Verify(x => x.All(), Times.Once);

            userManager.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task AddProductAsyncUserAuthenticatedShouldReturnFalseWithInvalidProductIdUsingMoq()
        {
            var shoppingCartRepository = new Mock<IRepository<ShoppingCartProduct>>();
            var productsService = new Mock<IProductsService>();

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "TestUserId1", ShoppingCartId = "TestShoppingCartId1" },
            };

            var shoppingCartProducts = new List<ShoppingCartProduct>
            {
                new ShoppingCartProduct { Id = "TestId1", ProductId = "TestProductId1", ShoppingCartId = "TestShoppingCartId1" },
            };

            var products = new List<Product>
            {
                new Product { Id = "TestProductId1" },
            };

            shoppingCartRepository.Setup(r => r.All()).Returns(shoppingCartProducts.AsQueryable());

            productsService.Setup(r => r.HasProduct(It.IsAny<string>())).Returns((string id) => products.Any(x => x.Id == id));

            userManager.Setup(r => r.FindByIdAsync(It.IsAny<string>()))
                .Returns(async (string userId) => await Task.FromResult<ApplicationUser>(users.FirstOrDefault(x => x.Id == userId)));

            var service = new ShoppingCartService(shoppingCartRepository.Object, userManager.Object, productsService.Object);

            Assert.False(await service.AddProductAsync(true, null, "TestUserId1", "TestProductId2"));

            shoppingCartRepository.Verify(x => x.All(), Times.Once);

            productsService.Verify(x => x.HasProduct(It.IsAny<string>()), Times.Once);

            userManager.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task AddProductAsyncUserAuthenticatedShouldWorkCorrectlyUsingMoq()
        {
            var shoppingCartRepository = new Mock<IRepository<ShoppingCartProduct>>();
            var productsService = new Mock<IProductsService>();

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "TestUserId1", ShoppingCartId = "TestShoppingCartId1" },
            };

            var shoppingCartProducts = new List<ShoppingCartProduct>
            {
                new ShoppingCartProduct { Id = "TestId1", ProductId = "TestProductId1", ShoppingCartId = "TestShoppingCartId1" },
            };

            var products = new List<Product>
            {
                new Product { Id = "TestProductId1" },
                new Product { Id = "TestProductId2" },
            };

            shoppingCartRepository.Setup(r => r.All()).Returns(shoppingCartProducts.AsQueryable());
            shoppingCartRepository.Setup(r => r.AddAsync(It.IsAny<ShoppingCartProduct>())).Callback((ShoppingCartProduct item) => shoppingCartProducts.Add(item));
            shoppingCartRepository.Setup(r => r.SaveChangesAsync()).Verifiable();

            productsService.Setup(r => r.HasProduct(It.IsAny<string>())).Returns((string id) => products.Any(x => x.Id == id));

            userManager.Setup(r => r.FindByIdAsync(It.IsAny<string>()))
                .Returns(async (string userId) => await Task.FromResult<ApplicationUser>(users.FirstOrDefault(x => x.Id == userId)));

            var service = new ShoppingCartService(shoppingCartRepository.Object, userManager.Object, productsService.Object);

            Assert.True(await service.AddProductAsync(true, null, "TestUserId1", "TestProductId2"));
            Assert.Equal(2, shoppingCartProducts.Count);
            Assert.Equal("TestProductId2", shoppingCartProducts.LastOrDefault().ProductId);
            Assert.Equal("TestShoppingCartId1", shoppingCartProducts.LastOrDefault().ShoppingCartId);
            Assert.Equal(1, shoppingCartProducts.LastOrDefault().Quantity);

            shoppingCartRepository.Verify(x => x.All(), Times.Once);
            shoppingCartRepository.Verify(x => x.AddAsync(It.IsAny<ShoppingCartProduct>()), Times.Once);
            shoppingCartRepository.Verify(x => x.SaveChangesAsync());

            productsService.Verify(x => x.HasProduct(It.IsAny<string>()), Times.Once);

            userManager.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetAllProductsGenericUserAuthenticatedShouldReturnCorrectCountUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var shoppingCartRepository = new Mock<IRepository<ShoppingCartProduct>>();
            var productsService = new Mock<IProductsService>();

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "TestUserId1", ShoppingCartId = "TestShoppingCartId1" },
            };

            var products = new List<Product>
            {
                new Product { Id = "TestProductId1", Name = "TestProductName1", Price = 42 },
                new Product { Id = "TestProductId2" },
            };

            var shoppingCartProducts = new List<ShoppingCartProduct>
            {
                new ShoppingCartProduct { Id = "TestId1", ProductId = "TestProductId1", ShoppingCartId = "TestShoppingCartId1", Product = products.FirstOrDefault() },
                new ShoppingCartProduct { Id = "TestId2", ProductId = "TestProductId2", ShoppingCartId = "TestShoppingCartId1", Product = products.LastOrDefault() },
            };

            shoppingCartRepository.Setup(r => r.AllAsNoTracking()).Returns(shoppingCartProducts.AsQueryable());

            userManager.Setup(r => r.FindByIdAsync(It.IsAny<string>()))
                .Returns(async (string userId) => await Task.FromResult<ApplicationUser>(users.FirstOrDefault(x => x.Id == userId)));

            var service = new ShoppingCartService(shoppingCartRepository.Object, userManager.Object, productsService.Object);

            var result = await service.GetAllProductsAsync<ShoppingCartProductViewModel>(true, null, "TestUserId1");
            Assert.Equal(2, result.Count());

            shoppingCartRepository.Verify(x => x.AllAsNoTracking(), Times.Once);

            userManager.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetAllProductsGenericUserAuthenticatedShouldWorkCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var shoppingCartRepository = new Mock<IRepository<ShoppingCartProduct>>();
            var productsService = new Mock<IProductsService>();

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "TestUserId1", ShoppingCartId = "TestShoppingCartId1" },
            };

            var images = new List<ProductImage>
            {
                new ProductImage { ImageUrl = "TestImageUrl" },
            };
            var reviews = new List<UserProductReview>
            {
                new UserProductReview { Rating = 5 },
                new UserProductReview { Rating = 2 },
                new UserProductReview { Rating = 1 },
            };
            var products = new List<Product>
            {
                new Product { Id = "TestProductId1", Name = "TestProductName1", Price = 42, Images = images, Reviews = reviews },
                new Product { Id = "TestProductId2" },
            };

            var shoppingCartProducts = new List<ShoppingCartProduct>
            {
                new ShoppingCartProduct { Id = "TestId1", ProductId = "TestProductId1", ShoppingCartId = "TestShoppingCartId1", Product = products.FirstOrDefault(), Quantity = 2 },
                new ShoppingCartProduct { Id = "TestId2", ProductId = "TestProductId2", ShoppingCartId = "TestShoppingCartId1", Product = products.LastOrDefault() },
            };

            shoppingCartRepository.Setup(r => r.AllAsNoTracking()).Returns(shoppingCartProducts.AsQueryable());

            userManager.Setup(r => r.FindByIdAsync(It.IsAny<string>()))
                .Returns(async (string userId) => await Task.FromResult<ApplicationUser>(users.FirstOrDefault(x => x.Id == userId)));

            var service = new ShoppingCartService(shoppingCartRepository.Object, userManager.Object, productsService.Object);

            var result = await service.GetAllProductsAsync<ShoppingCartProductViewModel>(true, null, "TestUserId1");
            Assert.Equal("TestProductId1", result.FirstOrDefault().ProductId);
            Assert.Equal("TestProductName1", result.FirstOrDefault().ProductName);
            Assert.Equal(42, result.FirstOrDefault().ProductPrice);
            Assert.Equal("TestImageUrl", result.FirstOrDefault().ImageUrl);
            Assert.Equal(2, result.FirstOrDefault().Quantity);
            Assert.Equal(2.67, result.FirstOrDefault().AverageRating);
            Assert.Equal(84, result.FirstOrDefault().TotalPrice);

            shoppingCartRepository.Verify(x => x.AllAsNoTracking(), Times.Once);

            userManager.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetProductsCountUserAuthenticatedShouldReturnCorrectCountUsingMoq()
        {
            var shoppingCartRepository = new Mock<IRepository<ShoppingCartProduct>>();
            var productsService = new Mock<IProductsService>();

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "TestUserId1", ShoppingCartId = "TestShoppingCartId1" },
            };

            var products = new List<Product>
            {
                new Product { Id = "TestProductId1", Name = "TestProductName1", Price = 42 },
                new Product { Id = "TestProductId2" },
            };

            var shoppingCartProducts = new List<ShoppingCartProduct>
            {
                new ShoppingCartProduct { Id = "TestId1", ProductId = "TestProductId1", ShoppingCartId = "TestShoppingCartId1", Product = products.FirstOrDefault() },
                new ShoppingCartProduct { Id = "TestId2", ProductId = "TestProductId2", ShoppingCartId = "TestShoppingCartId1", Product = products.LastOrDefault() },
            };

            shoppingCartRepository.Setup(r => r.AllAsNoTracking()).Returns(shoppingCartProducts.AsQueryable());

            userManager.Setup(r => r.FindByIdAsync(It.IsAny<string>()))
                .Returns(async (string userId) => await Task.FromResult<ApplicationUser>(users.FirstOrDefault(x => x.Id == userId)));

            var service = new ShoppingCartService(shoppingCartRepository.Object, userManager.Object, productsService.Object);

            Assert.Equal(2, await service.GetProductsCountAsync(true, null, "TestUserId1"));

            shoppingCartRepository.Verify(x => x.AllAsNoTracking(), Times.Once);

            userManager.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task AnyProductsUserAuthenticatedShouldWorkCorrectlyUsingMoq()
        {
            var shoppingCartRepository = new Mock<IRepository<ShoppingCartProduct>>();
            var productsService = new Mock<IProductsService>();

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "TestUserId1", ShoppingCartId = "TestShoppingCartId1" },
                new ApplicationUser { Id = "TestUserId2", ShoppingCartId = "TestShoppingCartId2" },
            };

            var products = new List<Product>
            {
                new Product { Id = "TestProductId1", Name = "TestProductName1", Price = 42 },
                new Product { Id = "TestProductId2" },
            };

            var shoppingCartProducts = new List<ShoppingCartProduct>
            {
                new ShoppingCartProduct { Id = "TestId1", ProductId = "TestProductId1", ShoppingCartId = "TestShoppingCartId1", Product = products.FirstOrDefault() },
                new ShoppingCartProduct { Id = "TestId2", ProductId = "TestProductId2", ShoppingCartId = "TestShoppingCartId1", Product = products.LastOrDefault() },
            };

            shoppingCartRepository.Setup(r => r.AllAsNoTracking()).Returns(shoppingCartProducts.AsQueryable());

            userManager.Setup(r => r.FindByIdAsync(It.IsAny<string>()))
                .Returns(async (string userId) => await Task.FromResult<ApplicationUser>(users.FirstOrDefault(x => x.Id == userId)));

            var service = new ShoppingCartService(shoppingCartRepository.Object, userManager.Object, productsService.Object);

            Assert.True(await service.AnyProductsAsync("TestUserId1"));
            Assert.False(await service.AnyProductsAsync("TestUserId2"));

            shoppingCartRepository.Verify(x => x.AllAsNoTracking(), Times.Exactly(2));

            userManager.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task DeleteProductAsyncUserAuthenticatedShouldReturnFalseWithInvalidShoppingCartIdUsingMoq()
        {
            var repository = new Mock<IRepository<ShoppingCartProduct>>();

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "TestUserId1", ShoppingCartId = "TestShoppingCartId1" },
            };

            var shoppingCartProducts = new List<ShoppingCartProduct>
            {
                new ShoppingCartProduct { Id = "TestId1", ProductId = "TestProductId1", ShoppingCartId = "TestShoppingCartId1" },
            };

            repository.Setup(r => r.All()).Returns(shoppingCartProducts.AsQueryable());

            userManager.Setup(r => r.FindByIdAsync(It.IsAny<string>()))
                .Returns(async (string userId) => await Task.FromResult<ApplicationUser>(users.FirstOrDefault(x => x.Id == userId)));

            var service = new ShoppingCartService(repository.Object, userManager.Object, null);

            Assert.False(await service.DeleteProductAsync(true, null, "TestUserId1", "TestProductId2"));

            repository.Verify(x => x.All(), Times.Once);

            userManager.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task DeleteProductAsyncUserAuthenticatedShouldWorkCorrectlyUsingMoq()
        {
            var repository = new Mock<IRepository<ShoppingCartProduct>>();

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "TestUserId1", ShoppingCartId = "TestShoppingCartId1" },
            };

            var shoppingCartProducts = new List<ShoppingCartProduct>
            {
                new ShoppingCartProduct { Id = "TestId1", ProductId = "TestProductId1", ShoppingCartId = "TestShoppingCartId1" },
            };

            repository.Setup(r => r.All()).Returns(shoppingCartProducts.AsQueryable());
            repository.Setup(r => r.Delete(It.IsAny<ShoppingCartProduct>())).Callback((ShoppingCartProduct item) => shoppingCartProducts.Remove(item));
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            userManager.Setup(r => r.FindByIdAsync(It.IsAny<string>()))
                .Returns(async (string userId) => await Task.FromResult<ApplicationUser>(users.FirstOrDefault(x => x.Id == userId)));

            var service = new ShoppingCartService(repository.Object, userManager.Object, null);

            Assert.True(await service.DeleteProductAsync(true, null, "TestUserId1", "TestProductId1"));
            Assert.Empty(shoppingCartProducts);

            repository.Verify(x => x.All(), Times.Once);
            repository.Verify(x => x.Delete(It.IsAny<ShoppingCartProduct>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());

            userManager.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAllProductsAsyncUserAuthenticatedShouldWorkCorrectlyWithNoProductsUsingMoq()
        {
            var repository = new Mock<IRepository<ShoppingCartProduct>>();

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "TestUserId1", ShoppingCartId = "TestShoppingCartId2" },
            };

            var shoppingCartProducts = new List<ShoppingCartProduct>
            {
                new ShoppingCartProduct { Id = "TestId1", ProductId = "TestProductId1", ShoppingCartId = "TestShoppingCartId1" },
                new ShoppingCartProduct { Id = "TestId2", ProductId = "TestProductId2", ShoppingCartId = "TestShoppingCartId1" },
            };

            repository.Setup(r => r.All()).Returns(shoppingCartProducts.AsQueryable());
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            userManager.Setup(r => r.FindByIdAsync(It.IsAny<string>()))
                .Returns(async (string userId) => await Task.FromResult<ApplicationUser>(users.FirstOrDefault(x => x.Id == userId)));

            var service = new ShoppingCartService(repository.Object, userManager.Object, null);

            await service.DeleteAllProductsAsync("TestUserId1");
            Assert.Equal(2, shoppingCartProducts.Count);

            repository.Verify(x => x.All(), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());

            userManager.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAllProductsAsyncUserAuthenticatedShouldWorkCorrectlyWithProductsUsingMoq()
        {
            var repository = new Mock<IRepository<ShoppingCartProduct>>();

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "TestUserId1", ShoppingCartId = "TestShoppingCartId1" },
            };

            var shoppingCartProducts = new List<ShoppingCartProduct>
            {
                new ShoppingCartProduct { Id = "TestId1", ProductId = "TestProductId1", ShoppingCartId = "TestShoppingCartId1" },
                new ShoppingCartProduct { Id = "TestId2", ProductId = "TestProductId2", ShoppingCartId = "TestShoppingCartId1" },
            };

            repository.Setup(r => r.All()).Returns(shoppingCartProducts.AsQueryable());
            repository.Setup(r => r.Delete(It.IsAny<ShoppingCartProduct>())).Callback((ShoppingCartProduct item) => shoppingCartProducts.Remove(item));
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            userManager.Setup(r => r.FindByIdAsync(It.IsAny<string>()))
                .Returns(async (string userId) => await Task.FromResult<ApplicationUser>(users.FirstOrDefault(x => x.Id == userId)));

            var service = new ShoppingCartService(repository.Object, userManager.Object, null);

            await service.DeleteAllProductsAsync("TestUserId1");
            Assert.Empty(shoppingCartProducts);

            repository.Verify(x => x.All(), Times.Once);
            repository.Verify(x => x.Delete(It.IsAny<ShoppingCartProduct>()), Times.Exactly(2));
            repository.Verify(x => x.SaveChangesAsync());

            userManager.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task UpdateQuantityAsyncUserAuthenticatedShouldReturnFalseWithInvalidShoppingCartIdUsingMoq()
        {
            var repository = new Mock<IRepository<ShoppingCartProduct>>();

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "TestUserId1", ShoppingCartId = "TestShoppingCartId1" },
            };

            var shoppingCartProducts = new List<ShoppingCartProduct>
            {
                new ShoppingCartProduct { Id = "TestId1", ProductId = "TestProductId1", ShoppingCartId = "TestShoppingCartId1" },
            };

            repository.Setup(r => r.All()).Returns(shoppingCartProducts.AsQueryable());

            userManager.Setup(r => r.FindByIdAsync(It.IsAny<string>()))
                .Returns(async (string userId) => await Task.FromResult<ApplicationUser>(users.FirstOrDefault(x => x.Id == userId)));

            var service = new ShoppingCartService(repository.Object, userManager.Object, null);

            Assert.False(await service.UpdateQuantityAsync(true, null, "TestUserId1", "TestProductId2", true));

            repository.Verify(x => x.All(), Times.Once);

            userManager.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task UpdateQuantityAsyncUserAuthenticatedShouldIncreaseQuantityUsingMoq()
        {
            var repository = new Mock<IRepository<ShoppingCartProduct>>();

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "TestUserId1", ShoppingCartId = "TestShoppingCartId1" },
            };

            var shoppingCartProducts = new List<ShoppingCartProduct>
            {
                new ShoppingCartProduct { Id = "TestId1", ProductId = "TestProductId1", ShoppingCartId = "TestShoppingCartId1", Quantity = 1 },
            };

            repository.Setup(r => r.All()).Returns(shoppingCartProducts.AsQueryable());
            repository.Setup(r => r.Update(It.IsAny<ShoppingCartProduct>())).Callback((ShoppingCartProduct item) =>
            {
                var shoppingCartProduct = shoppingCartProducts.FirstOrDefault(x => x.Id == item.Id);
                shoppingCartProduct.Quantity = item.Quantity;
            });
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            userManager.Setup(r => r.FindByIdAsync(It.IsAny<string>()))
                .Returns(async (string userId) => await Task.FromResult<ApplicationUser>(users.FirstOrDefault(x => x.Id == userId)));

            var service = new ShoppingCartService(repository.Object, userManager.Object, null);

            Assert.True(await service.UpdateQuantityAsync(true, null, "TestUserId1", "TestProductId1", true));
            Assert.Equal(2, shoppingCartProducts.FirstOrDefault().Quantity);

            repository.Verify(x => x.All(), Times.Once);
            repository.Verify(x => x.Update(It.IsAny<ShoppingCartProduct>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());

            userManager.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task UpdateQuantityAsyncUserAuthenticatedShouldDecreaseQuantityButNotBelowOneUsingMoq()
        {
            var repository = new Mock<IRepository<ShoppingCartProduct>>();

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "TestUserId1", ShoppingCartId = "TestShoppingCartId1" },
            };

            var shoppingCartProducts = new List<ShoppingCartProduct>
            {
                new ShoppingCartProduct { Id = "TestId1", ProductId = "TestProductId1", ShoppingCartId = "TestShoppingCartId1", Quantity = 1 },
                new ShoppingCartProduct { Id = "TestId1", ProductId = "TestProductId2", ShoppingCartId = "TestShoppingCartId1", Quantity = 1 },
            };

            repository.Setup(r => r.All()).Returns(shoppingCartProducts.AsQueryable());
            repository.Setup(r => r.Update(It.IsAny<ShoppingCartProduct>())).Callback((ShoppingCartProduct item) =>
            {
                var shoppingCartProduct = shoppingCartProducts.FirstOrDefault(x => x.Id == item.Id);
                shoppingCartProduct.Quantity = item.Quantity;
            });
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            userManager.Setup(r => r.FindByIdAsync(It.IsAny<string>()))
                .Returns(async (string userId) => await Task.FromResult<ApplicationUser>(users.FirstOrDefault(x => x.Id == userId)));

            var service = new ShoppingCartService(repository.Object, userManager.Object, null);

            Assert.True(await service.UpdateQuantityAsync(true, null, "TestUserId1", "TestProductId1", false));
            Assert.True(await service.UpdateQuantityAsync(true, null, "TestUserId1", "TestProductId2", false));
            Assert.Equal(1, shoppingCartProducts.FirstOrDefault().Quantity);
            Assert.Equal(1, shoppingCartProducts.LastOrDefault().Quantity);

            repository.Verify(x => x.All(), Times.Exactly(2));
            repository.Verify(x => x.Update(It.IsAny<ShoppingCartProduct>()), Times.Exactly(2));
            repository.Verify(x => x.SaveChangesAsync());

            userManager.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Exactly(2));
        }
    }
}
