namespace XeMart.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Moq;

    using XeMart.Data;
    using XeMart.Data.Common.Repositories;
    using XeMart.Data.Models;
    using XeMart.Data.Repositories;
    using XeMart.Services.Mapping;
    using XeMart.Web.ViewModels;
    using XeMart.Web.ViewModels.Administration.Subcategories;
    using Xunit;

    public class SubcategoriesServiceTests
    {
        [Fact]
        public void GetAllShouldWorkCorrectly()
        {
            var repository = new Mock<IDeletableEntityRepository<Subcategory>>();

            var categoriesList = new List<Subcategory>
            {
                new Subcategory { Id = 1, CreatedOn = DateTime.UtcNow, Name = "Test1", ImageUrl = "TestUrl1" },
                new Subcategory { Id = 2, CreatedOn = DateTime.UtcNow, Name = "Test2", ImageUrl = "TestUrl2" },
                new Subcategory { Id = 3, CreatedOn = DateTime.UtcNow, Name = "Test3", ImageUrl = "TestUrl3" },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(categoriesList.AsQueryable());

            var service = new SubcategoriesService(repository.Object, null);
            Assert.Equal(categoriesList, service.GetAll());

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task GetAllShouldWorkCorrectlyUsingDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GetAllShouldWorkCorrectlyUsingDbContext").Options;
            using var dbContext = new ApplicationDbContext(options);
            await dbContext.Subcategories.AddAsync(new Subcategory { Name = "Test1", ImageUrl = "TestUrl1" });
            await dbContext.Subcategories.AddAsync(new Subcategory { Name = "Test2", ImageUrl = "TestUrl2" });
            await dbContext.Subcategories.AddAsync(new Subcategory { Name = "Test3", ImageUrl = "TestUrl3" });
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<Subcategory>(dbContext);
            var service = new SubcategoriesService(repository, null);

            Assert.Equal(3, service.GetAll().Count());
            Assert.Equal(1, service.GetAll().FirstOrDefault().Id);
            Assert.Equal("Test1", service.GetAll().FirstOrDefault().Name);
            Assert.Equal("TestUrl1", service.GetAll().FirstOrDefault().ImageUrl);
            Assert.Equal(2, service.GetAll().ElementAt(1).Id);
            Assert.Equal("Test2", service.GetAll().ElementAt(1).Name);
            Assert.Equal("TestUrl2", service.GetAll().ElementAt(1).ImageUrl);
            Assert.Equal(3, service.GetAll().ElementAt(2).Id);
            Assert.Equal("Test3", service.GetAll().ElementAt(2).Name);
            Assert.Equal("TestUrl3", service.GetAll().ElementAt(2).ImageUrl);
        }

        [Fact]
        public void GetAllShouldWorkCorrectlyWithNoAddedCategories()
        {
            var repository = new Mock<IDeletableEntityRepository<Subcategory>>();

            var categoriesList = new List<Subcategory>();

            repository.Setup(r => r.AllAsNoTracking()).Returns(categoriesList.AsQueryable());

            var service = new SubcategoriesService(repository.Object, null);
            Assert.Empty(service.GetAll());
            Assert.Equal(categoriesList, service.GetAll());

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(2));
        }

        [Fact]
        public void GetAllShouldWorkCorrectlyWithNoAddedCategoriesUsingDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GetAllShouldWorkCorrectlyWithNoAddedCategoriesUsingDbContext").Options;
            using var dbContext = new ApplicationDbContext(options);

            using var repository = new EfDeletableEntityRepository<Subcategory>(dbContext);
            var service = new SubcategoriesService(repository, null);

            Assert.Empty(service.GetAll());
        }

        [Fact]
        public void GetAllGenericShouldReturnCorrectCount()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Subcategory>>();

            var mainCategory = new MainCategory
            {
                Name = "TestMainCategory",
                FontAwesomeIcon = "TestFAIcon",
            };

            var categoriesList = new List<Subcategory>
            {
                new Subcategory { Id = 1, CreatedOn = DateTime.UtcNow, Name = "Test1", ImageUrl = "TestUrl1", MainCategory = mainCategory },
                new Subcategory { Id = 2, CreatedOn = DateTime.UtcNow, Name = "Test2", ImageUrl = "TestUrl2", MainCategory = mainCategory },
                new Subcategory { Id = 3, CreatedOn = DateTime.UtcNow, Name = "Test3", ImageUrl = "TestUrl3", MainCategory = mainCategory },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(categoriesList.AsQueryable());

            var service = new SubcategoriesService(repository.Object, null);
            Assert.Equal(3, service.GetAll<SubcategoryViewModel>().Count());

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task GetAllGenericShouldReturnCorrectCountUsingDbContext()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GetAllGenericShouldReturnCorrectCountUsingDbContext").Options;
            using var dbContext = new ApplicationDbContext(options);
            var mainCategory = new MainCategory { Name = "TestMainCategory", FontAwesomeIcon = "TestFAIcon" };
            await dbContext.MainCategories.AddAsync(mainCategory);
            await dbContext.Subcategories.AddAsync(new Subcategory { Name = "Test1", ImageUrl = "TestUrl1", MainCategory = mainCategory });
            await dbContext.Subcategories.AddAsync(new Subcategory { Name = "Test2", ImageUrl = "TestUrl2", MainCategory = mainCategory });
            await dbContext.Subcategories.AddAsync(new Subcategory { Name = "Test3", ImageUrl = "TestUrl3", MainCategory = mainCategory });
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<Subcategory>(dbContext);
            var service = new SubcategoriesService(repository, null);

            Assert.Equal(3, service.GetAll<SubcategoryViewModel>().Count());
        }

        [Fact]
        public void GetAllGenericShouldMapCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Subcategory>>();

            var mainCategory = new MainCategory
            {
                Name = "TestMainCategory",
                FontAwesomeIcon = "TestFAIcon",
            };

            var productsList = new List<Product>
            {
                new Product { Id = "TestId1", CreatedOn = DateTime.UtcNow, Name = "TestProduct1", Price = 42 },
                new Product { Id = "TestId2", CreatedOn = DateTime.UtcNow, Name = "TestProduct2", Price = 25 },
            };

            var categoriesList = new List<Subcategory>
            {
                new Subcategory { Id = 1, CreatedOn = DateTime.UtcNow, Name = "TestSubcategory", ImageUrl = "TestUrl", MainCategory = mainCategory, Products = productsList },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(categoriesList.AsQueryable());

            var service = new SubcategoriesService(repository.Object, null);
            Assert.Equal(1, service.GetAll<SubcategoryViewModel>().FirstOrDefault().Id);
            Assert.Equal("TestSubcategory", service.GetAll<SubcategoryViewModel>().FirstOrDefault().Name);
            Assert.Equal("TestUrl", service.GetAll<SubcategoryViewModel>().FirstOrDefault().ImageUrl);
            Assert.Equal("TestMainCategory", service.GetAll<SubcategoryViewModel>().FirstOrDefault().MainCategoryName);
            Assert.Equal(2, service.GetAll<SubcategoryViewModel>().FirstOrDefault().ProductsCount);

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(5));
        }

        [Fact]
        public async Task GetAllGenericShouldMapCorrectlyUsingDbContext()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GetAllGenericShouldMapCorrectlyUsingDbContext").Options;
            using var dbContext = new ApplicationDbContext(options);

            var productsList = new List<Product>
            {
                new Product { Name = "TestProduct1", Price = 42 },
                new Product { Name = "TestProduct2", Price = 25 },
            };
            await dbContext.MainCategories.AddAsync(new MainCategory { Name = "TestMainCategory", FontAwesomeIcon = "TestIcon" });
            await dbContext.Subcategories.AddAsync(new Subcategory { Name = "TestSubcategory", MainCategoryId = 1, ImageUrl = "TestUrl", Products = productsList });
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<Subcategory>(dbContext);
            var service = new SubcategoriesService(repository, null);

            Assert.Equal(1, service.GetAll<SubcategoryViewModel>().FirstOrDefault().Id);
            Assert.Equal("TestSubcategory", service.GetAll<SubcategoryViewModel>().FirstOrDefault().Name);
            Assert.Equal("TestUrl", service.GetAll<SubcategoryViewModel>().FirstOrDefault().ImageUrl);
            Assert.Equal("TestMainCategory", service.GetAll<SubcategoryViewModel>().FirstOrDefault().MainCategoryName);
            Assert.Equal(2, service.GetAll<SubcategoryViewModel>().FirstOrDefault().ProductsCount);
        }

        [Fact]
        public void GetAllDeletedGenericShouldReturnCorrectCount()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Subcategory>>();

            var mainCategory = new MainCategory
            {
                Name = "TestMainCategory",
                FontAwesomeIcon = "TestFAIcon",
            };
            var categoriesList = new List<Subcategory>
            {
                new Subcategory { Id = 1, CreatedOn = DateTime.UtcNow, IsDeleted = true, DeletedOn = DateTime.UtcNow, Name = "Test1", ImageUrl = "TestUrl1", MainCategory = mainCategory },
                new Subcategory { Id = 2, CreatedOn = DateTime.UtcNow, IsDeleted = true, DeletedOn = DateTime.UtcNow, Name = "Test2", ImageUrl = "TestUrl2", MainCategory = mainCategory },
                new Subcategory { Id = 3, CreatedOn = DateTime.UtcNow, IsDeleted = true, DeletedOn = DateTime.UtcNow, Name = "Test3", ImageUrl = "TestUrl3", MainCategory = mainCategory },
            };

            repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(categoriesList.AsQueryable());

            var service = new SubcategoriesService(repository.Object, null);
            Assert.Equal(3, service.GetAllDeleted<DeletedSubcategoryViewModel>().Count());

            repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Once);
        }

        [Fact]
        public async Task GetAllDeletedGenericShouldReturnCorrectCountUsingDbContext()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GetAllDeletedGenericShouldReturnCorrectCountUsingDbContext").Options;
            using var dbContext = new ApplicationDbContext(options);
            await dbContext.MainCategories.AddAsync(new MainCategory { Name = "TestMainCategory", FontAwesomeIcon = "TestFAIcon" });
            await dbContext.Subcategories.AddAsync(new Subcategory { Name = "TestSubcategory1", ImageUrl = "TestUrl1", MainCategoryId = 1, IsDeleted = true, DeletedOn = DateTime.UtcNow });
            await dbContext.Subcategories.AddAsync(new Subcategory { Name = "TestSubcategory2", ImageUrl = "TestUrl2", MainCategoryId = 1, IsDeleted = true, DeletedOn = DateTime.UtcNow });
            await dbContext.Subcategories.AddAsync(new Subcategory { Name = "TestSubcategory3", ImageUrl = "TestUrl3", MainCategoryId = 1, IsDeleted = true, DeletedOn = DateTime.UtcNow });
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<Subcategory>(dbContext);
            var service = new SubcategoriesService(repository, null);

            Assert.Equal(3, service.GetAllDeleted<DeletedSubcategoryViewModel>().Count());
        }

        [Fact]
        public void GetAllDeletedGenericShouldMapCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Subcategory>>();

            var mainCategory = new MainCategory
            {
                Name = "TestMainCategory",
                FontAwesomeIcon = "TestFAIcon",
            };

            var productsList = new List<Product>
            {
                new Product { Id = "TestId1", CreatedOn = DateTime.UtcNow, Name = "TestProduct1", Price = 42 },
                new Product { Id = "TestId2", CreatedOn = DateTime.UtcNow, Name = "TestProduct2", Price = 25 },
            };

            var categoriesList = new List<Subcategory>
            {
                new Subcategory { Id = 1, CreatedOn = DateTime.UtcNow, IsDeleted = true, DeletedOn = new DateTime(2020, 12, 31, 12, 12, 12), Name = "TestSubcategory1", ImageUrl = "TestUrl1", MainCategory = mainCategory, Products = productsList },
                new Subcategory { Id = 1, CreatedOn = DateTime.UtcNow, Name = "TestSubcategory2", ImageUrl = "TestUrl2", MainCategory = mainCategory },
            };

            repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(categoriesList.AsQueryable());

            var service = new SubcategoriesService(repository.Object, null);
            Assert.Equal(1, service.GetAllDeleted<DeletedSubcategoryViewModel>().FirstOrDefault().Id);
            Assert.Equal("TestSubcategory1", service.GetAllDeleted<DeletedSubcategoryViewModel>().FirstOrDefault().Name);
            Assert.Equal("TestUrl1", service.GetAllDeleted<DeletedSubcategoryViewModel>().FirstOrDefault().ImageUrl);
            Assert.Equal("TestMainCategory", service.GetAllDeleted<DeletedSubcategoryViewModel>().FirstOrDefault().MainCategoryName);
            Assert.Equal(2, service.GetAllDeleted<DeletedSubcategoryViewModel>().FirstOrDefault().ProductsCount);
            Assert.Equal("31-Dec-2020 12:12", service.GetAllDeleted<DeletedSubcategoryViewModel>().FirstOrDefault().DeletedOn);

            repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Exactly(6));
        }

        [Fact]
        public async Task GetAllDeletedGenericShouldMapCorrectlyUsingDbContext()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GetAllDeletedGenericShouldMapCorrectlyUsingDbContext").Options;
            using var dbContext = new ApplicationDbContext(options);

            var productsList = new List<Product>
            {
                new Product { Name = "TestProduct1", Price = 42 },
                new Product { Name = "TestProduct2", Price = 25 },
            };
            await dbContext.MainCategories.AddAsync(new MainCategory { Name = "TestMainCategory", FontAwesomeIcon = "TestIcon" });
            await dbContext.Subcategories.AddAsync(new Subcategory { Name = "TestSubcategory1", ImageUrl = "TestUrl1", IsDeleted = true, DeletedOn = new DateTime(2020, 12, 31, 12, 12, 12), MainCategoryId = 1, Products = productsList });
            await dbContext.Subcategories.AddAsync(new Subcategory { Name = "TestSubcategory2", MainCategoryId = 1 });
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<Subcategory>(dbContext);
            var service = new SubcategoriesService(repository, null);

            Assert.Equal(1, service.GetAllDeleted<DeletedSubcategoryViewModel>().FirstOrDefault().Id);
            Assert.Equal("TestSubcategory1", service.GetAllDeleted<DeletedSubcategoryViewModel>().FirstOrDefault().Name);
            Assert.Equal("TestUrl1", service.GetAllDeleted<DeletedSubcategoryViewModel>().FirstOrDefault().ImageUrl);
            Assert.Equal("TestMainCategory", service.GetAllDeleted<DeletedSubcategoryViewModel>().FirstOrDefault().MainCategoryName);
            Assert.Equal(2, service.GetAllDeleted<DeletedSubcategoryViewModel>().FirstOrDefault().ProductsCount);
            Assert.Equal("31-Dec-2020 12:12", service.GetAllDeleted<DeletedSubcategoryViewModel>().FirstOrDefault().DeletedOn);
        }

        [Fact]
        public void GetByIdGenericShouldMapCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Subcategory>>();

            var mainCategory = new MainCategory
            {
                Name = "TestMainCategory",
                FontAwesomeIcon = "TestFAIcon",
            };

            var productsList = new List<Product>
            {
                new Product { Id = "TestId1", CreatedOn = DateTime.UtcNow, Name = "TestProduct1", Price = 42 },
                new Product { Id = "TestId2", CreatedOn = DateTime.UtcNow, Name = "TestProduct2", Price = 25 },
            };

            var categoriesList = new List<Subcategory>
            {
                new Subcategory { Id = 42, CreatedOn = DateTime.UtcNow, Name = "TestSubcategory", ImageUrl = "TestUrl", MainCategory = mainCategory, Products = productsList },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(categoriesList.AsQueryable());

            var service = new SubcategoriesService(repository.Object, null);
            Assert.Equal(42, service.GetById<SubcategoryViewModel>(42).Id);
            Assert.Equal("TestSubcategory", service.GetById<SubcategoryViewModel>(42).Name);
            Assert.Equal("TestUrl", service.GetById<SubcategoryViewModel>(42).ImageUrl);
            Assert.Equal("TestMainCategory", service.GetById<SubcategoryViewModel>(42).MainCategoryName);
            Assert.Equal(2, service.GetById<SubcategoryViewModel>(42).ProductsCount);

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(5));
        }

        [Fact]
        public async Task GetByIdGenericShouldMapCorrectlyUsingDbContext()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GetByIdGenericShouldMapCorrectlyUsingDbContext").Options;
            using var dbContext = new ApplicationDbContext(options);

            var productsList = new List<Product>
            {
                new Product { Name = "TestProduct1", Price = 42 },
                new Product { Name = "TestProduct2", Price = 25 },
            };
            await dbContext.MainCategories.AddAsync(new MainCategory { Name = "TestMainCategory", FontAwesomeIcon = "TestIcon" });
            await dbContext.Subcategories.AddAsync(new Subcategory { Name = "TestSubcategory", MainCategoryId = 1, ImageUrl = "TestUrl", Products = productsList });
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<Subcategory>(dbContext);
            var service = new SubcategoriesService(repository, null);

            Assert.Equal(1, service.GetById<SubcategoryViewModel>(1).Id);
            Assert.Equal("TestSubcategory", service.GetById<SubcategoryViewModel>(1).Name);
            Assert.Equal("TestUrl", service.GetById<SubcategoryViewModel>(1).ImageUrl);
            Assert.Equal("TestMainCategory", service.GetById<SubcategoryViewModel>(1).MainCategoryName);
            Assert.Equal(2, service.GetById<SubcategoryViewModel>(1).ProductsCount);
        }

        [Fact]
        public async Task DeleteAsyncShouldReturnFalseWhenIdIsInvalid()
        {
            var repository = new Mock<IDeletableEntityRepository<Subcategory>>();

            var categoriesList = new List<Subcategory>
            {
                new Subcategory { Id = 1, CreatedOn = DateTime.UtcNow, Name = "Test1", ImageUrl = "TestUrl1" },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(categoriesList.AsQueryable());

            var service = new SubcategoriesService(repository.Object, null);
            Assert.False(await service.DeleteAsync(2));

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsyncShouldReturnFalseWhenIdIsInvalidUsingDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "DeleteAsyncShouldReturnFalseWhenIdIsInvalidUsingDbContext").Options;
            using var dbContext = new ApplicationDbContext(options);
            await dbContext.Subcategories.AddAsync(new Subcategory { Name = "Test1", ImageUrl = "TestUrl1" });
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<Subcategory>(dbContext);
            var service = new SubcategoriesService(repository, null);

            Assert.False(await service.DeleteAsync(2));
        }

        [Fact]
        public async Task DeleteAsyncShouldReturnFalseWhenSubcategoryHasProducts()
        {
            var repository = new Mock<IDeletableEntityRepository<Subcategory>>();

            var mainCategory = new MainCategory
            {
                Name = "TestMainCategory",
                FontAwesomeIcon = "TestFAIcon",
            };

            var productsList = new List<Product>
            {
                new Product { Id = "TestId1", CreatedOn = DateTime.UtcNow, Name = "TestProduct1", Price = 42 },
                new Product { Id = "TestId2", CreatedOn = DateTime.UtcNow, Name = "TestProduct2", Price = 25 },
            };

            var categoriesList = new List<Subcategory>
            {
                new Subcategory { Id = 1, CreatedOn = DateTime.UtcNow, Name = "TestSubcategory", ImageUrl = "TestUrl", MainCategory = mainCategory, Products = productsList },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(categoriesList.AsQueryable());

            var service = new SubcategoriesService(repository.Object, null);
            Assert.False(await service.DeleteAsync(1));

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsyncShouldReturnFalseWhenSubcategoryHasProductsUsingDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "DeleteAsyncShouldReturnFalseWhenMainCategoryHasSubcategoriesUsingDbContext").Options;
            using var dbContext = new ApplicationDbContext(options);
            var productsList = new List<Product>
            {
                new Product { Name = "TestProduct1", Price = 42 },
                new Product { Name = "TestProduct2", Price = 25 },
            };
            await dbContext.MainCategories.AddAsync(new MainCategory { Name = "TestMainCategory", FontAwesomeIcon = "TestIcon" });
            await dbContext.Subcategories.AddAsync(new Subcategory { Name = "TestSubcategory", MainCategoryId = 1, ImageUrl = "TestUrl", Products = productsList });
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<Subcategory>(dbContext);
            var service = new SubcategoriesService(repository, null);

            Assert.False(await service.DeleteAsync(1));
        }

        [Fact]
        public async Task DeleteAsyncShouldWorkCorrectly()
        {
            var repository = new Mock<IDeletableEntityRepository<Subcategory>>();

            var categoriesList = new List<Subcategory>
            {
                new Subcategory { Id = 1, CreatedOn = DateTime.UtcNow, Name = "TestSubcategory1", ImageUrl = "TestUrl1" },
                new Subcategory { Id = 2, CreatedOn = DateTime.UtcNow, Name = "TestSubcategory2", ImageUrl = "TestUrl2" },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(categoriesList.AsQueryable());
            repository.Setup(r => r.Delete(It.IsAny<Subcategory>())).Callback((Subcategory item) => categoriesList.Remove(item));

            var service = new SubcategoriesService(repository.Object, null);
            Assert.True(await service.DeleteAsync(1));
            Assert.Single(service.GetAll());

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(2));
        }

        [Fact]
        public async Task DeleteAsyncShouldWorkCorrectlyUsingDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "DeleteAsyncShouldWorkCorrectlyUsingDbContext").Options;
            using (var dbContext = new ApplicationDbContext(options))
            {
                await dbContext.MainCategories.AddAsync(new MainCategory { Name = "TestMainCategory", FontAwesomeIcon = "TestIcon" });
                await dbContext.Subcategories.AddAsync(new Subcategory { Name = "TestSubcategory1", MainCategoryId = 1, ImageUrl = "TestUrl1" });
                await dbContext.Subcategories.AddAsync(new Subcategory { Name = "TestSubcategory2", MainCategoryId = 1, ImageUrl = "TestUrl2" });
                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new ApplicationDbContext(options))
            {
                using var repository = new EfDeletableEntityRepository<Subcategory>(dbContext);
                var service = new SubcategoriesService(repository, null);

                Assert.True(await service.DeleteAsync(1));
                Assert.Single(service.GetAll());
            }
        }

        [Fact]
        public async Task UndeleteAsyncShouldReturnFalseWhenIdIsInvalid()
        {
            var repository = new Mock<IDeletableEntityRepository<Subcategory>>();

            var categoriesList = new List<Subcategory>
            {
                new Subcategory { Id = 1, CreatedOn = DateTime.UtcNow, Name = "TestSubcategory", ImageUrl = "TestUrl" },
            };

            repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(categoriesList.AsQueryable());

            var service = new SubcategoriesService(repository.Object, null);
            Assert.False(await service.UndeleteAsync(2));

            repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Once);
        }

        [Fact]
        public async Task UndeleteAsyncShouldReturnFalseWhenIdIsInvalidUsingDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "UndeleteAsyncShouldReturnFalseWhenIdIsInvalidUsingDbContext").Options;
            using var dbContext = new ApplicationDbContext(options);
            await dbContext.MainCategories.AddAsync(new MainCategory { Name = "TestMainCategory", FontAwesomeIcon = "TestIcon" });
            await dbContext.Subcategories.AddAsync(new Subcategory { Name = "TestSubcategory", ImageUrl = "TestUrl", MainCategoryId = 1 });
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<Subcategory>(dbContext);
            var service = new SubcategoriesService(repository, null);

            Assert.False(await service.UndeleteAsync(2));
        }

        [Fact]
        public async Task UndeleteAsyncShouldWorkCorrectly()
        {
            var repository = new Mock<IDeletableEntityRepository<Subcategory>>();

            var categoriesList = new List<Subcategory>
            {
                new Subcategory { Id = 1, CreatedOn = DateTime.UtcNow, Name = "TestSubcategory1", ImageUrl = "TestUrl1", IsDeleted = true, DeletedOn = DateTime.UtcNow },
                new Subcategory { Id = 2, CreatedOn = DateTime.UtcNow, Name = "TestSubcategory2", ImageUrl = "TestUrl2" },
                new Subcategory { Id = 3, CreatedOn = DateTime.UtcNow, Name = "TestSubcategory3", ImageUrl = "TestUrl3", IsDeleted = true, DeletedOn = DateTime.UtcNow },                
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(categoriesList.Where(x => !x.IsDeleted).AsQueryable());
            repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(categoriesList.AsQueryable());
            repository.Setup(r => r.Undelete(It.IsAny<Subcategory>())).Callback((Subcategory item) =>
            {
                var foundItem = categoriesList.FirstOrDefault(x => x.Id == item.Id);
                foundItem.IsDeleted = false;
                foundItem.DeletedOn = null;
            });

            var service = new SubcategoriesService(repository.Object, null);
            Assert.True(await service.UndeleteAsync(1));
            Assert.Equal(2, service.GetAll().Count());

            repository.Verify(x => x.AllAsNoTracking(), Times.Once());
            repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Once());
        }

        [Fact]
        public async Task UndeleteAsyncShouldWorkCorrectlyUsingDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "UndeleteAsyncShouldWorkCorrectlyUsingDbContext").Options;
            using (var dbContext = new ApplicationDbContext(options))
            {
                await dbContext.MainCategories.AddAsync(new MainCategory { Name = "TestMainCategory", FontAwesomeIcon = "TestIcon", IsDeleted = true, DeletedOn = DateTime.UtcNow });
                await dbContext.Subcategories.AddAsync(new Subcategory { Name = "TestSubcategory1", ImageUrl = "TestUrl1", MainCategoryId = 1, IsDeleted = true, DeletedOn = DateTime.UtcNow });
                await dbContext.Subcategories.AddAsync(new Subcategory { Name = "TestSubcategory2", ImageUrl = "TestUrl2", MainCategoryId = 1 });
                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new ApplicationDbContext(options))
            {
                using var repository = new EfDeletableEntityRepository<Subcategory>(dbContext);
                var service = new SubcategoriesService(repository, null);

                Assert.True(await service.UndeleteAsync(1));
                Assert.Equal(2, service.GetAll().Count());
            }
        }
    }
}
