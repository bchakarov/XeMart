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
    using XeMart.Web.ViewModels.Administration.MainCategories;

    using Xunit;

    [Collection("Sequential")]
    public class MainCategoriesServiceTests
    {
        [Fact]
        public void GetAllShouldWorkCorrectlyUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<MainCategory>>();

            var categoriesList = new List<MainCategory>
            {
                new MainCategory { Id = 1, CreatedOn = DateTime.UtcNow, Name = "Test1", FontAwesomeIcon = "TestIcon1" },
                new MainCategory { Id = 2, CreatedOn = DateTime.UtcNow, Name = "Test2", FontAwesomeIcon = "TestIcon2" },
                new MainCategory { Id = 3, CreatedOn = DateTime.UtcNow, Name = "Test3", FontAwesomeIcon = "TestIcon3" },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(categoriesList.AsQueryable());

            var service = new MainCategoriesService(repository.Object);
            Assert.Equal(categoriesList, service.GetAll());

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task GetAllShouldWorkCorrectlyUsingDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GetAllShouldWorkCorrectlyUsingDbContextMainCategoriesServiceTests").Options;
            using var dbContext = new ApplicationDbContext(options);
            await dbContext.MainCategories.AddAsync(new MainCategory { Name = "Test1", FontAwesomeIcon = "TestIcon1" });
            await dbContext.MainCategories.AddAsync(new MainCategory { Name = "Test2", FontAwesomeIcon = "TestIcon2" });
            await dbContext.MainCategories.AddAsync(new MainCategory { Name = "Test3", FontAwesomeIcon = "TestIcon3" });
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<MainCategory>(dbContext);
            var service = new MainCategoriesService(repository);

            Assert.Equal(3, service.GetAll().Count());
            Assert.Equal(1, service.GetAll().FirstOrDefault().Id);
            Assert.Equal("Test1", service.GetAll().FirstOrDefault().Name);
            Assert.Equal("TestIcon1", service.GetAll().FirstOrDefault().FontAwesomeIcon);
            Assert.Equal(2, service.GetAll().ElementAt(1).Id);
            Assert.Equal("Test2", service.GetAll().ElementAt(1).Name);
            Assert.Equal("TestIcon2", service.GetAll().ElementAt(1).FontAwesomeIcon);
            Assert.Equal(3, service.GetAll().ElementAt(2).Id);
            Assert.Equal("Test3", service.GetAll().ElementAt(2).Name);
            Assert.Equal("TestIcon3", service.GetAll().ElementAt(2).FontAwesomeIcon);
        }

        [Fact]
        public void GetAllShouldWorkCorrectlyWithNoAddedCategoriesUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<MainCategory>>();

            var categoriesList = new List<MainCategory>();

            repository.Setup(r => r.AllAsNoTracking()).Returns(categoriesList.AsQueryable());

            var service = new MainCategoriesService(repository.Object);
            Assert.Empty(service.GetAll());
            Assert.Equal(categoriesList, service.GetAll());

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(2));
        }

        [Fact]
        public void GetAllShouldWorkCorrectlyWithNoAddedCategoriesUsingDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GetAllShouldWorkCorrectlyWithNoAddedCategoriesUsingDbContextMainCategoriesServiceTests").Options;
            using var dbContext = new ApplicationDbContext(options);

            using var repository = new EfDeletableEntityRepository<MainCategory>(dbContext);
            var service = new MainCategoriesService(repository);

            Assert.Empty(service.GetAll());
        }

        [Fact]
        public void GetAllGenericShouldReturnCorrectCountUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<MainCategory>>();

            var categoriesList = new List<MainCategory>
            {
                new MainCategory { Id = 1, CreatedOn = DateTime.UtcNow, Name = "Test1", FontAwesomeIcon = "TestIcon1" },
                new MainCategory { Id = 2, CreatedOn = DateTime.UtcNow, Name = "Test2", FontAwesomeIcon = "TestIcon2" },
                new MainCategory { Id = 3, CreatedOn = DateTime.UtcNow, Name = "Test3", FontAwesomeIcon = "TestIcon3" },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(categoriesList.AsQueryable());

            var service = new MainCategoriesService(repository.Object);
            Assert.Equal(3, service.GetAll<MainCategoryViewModel>().Count());

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task GetAllGenericShouldReturnCorrectCountUsingDbContext()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GetAllGenericShouldReturnCorrectCountUsingDbContextMainCategoriesServiceTests").Options;
            using var dbContext = new ApplicationDbContext(options);
            await dbContext.MainCategories.AddAsync(new MainCategory { Name = "Test1", FontAwesomeIcon = "TestIcon1" });
            await dbContext.MainCategories.AddAsync(new MainCategory { Name = "Test2", FontAwesomeIcon = "TestIcon2" });
            await dbContext.MainCategories.AddAsync(new MainCategory { Name = "Test3", FontAwesomeIcon = "TestIcon3" });
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<MainCategory>(dbContext);
            var service = new MainCategoriesService(repository);

            Assert.Equal(3, service.GetAll<MainCategoryViewModel>().Count());
        }

        [Fact]
        public void GetAllGenericShouldMapCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<MainCategory>>();

            var categoriesList = new List<MainCategory>
            {
                new MainCategory
                {
                    Id = 1,
                    CreatedOn = DateTime.UtcNow,
                    Name = "Test",
                    FontAwesomeIcon = "TestIcon",
                    Subcategories = new List<Subcategory> { new Subcategory { Id = 1, CreatedOn = DateTime.UtcNow, MainCategoryId = 1, Name = "TestSubcategory" } },
                },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(categoriesList.AsQueryable());

            var service = new MainCategoriesService(repository.Object);
            Assert.Equal(1, service.GetAll<MainCategoryViewModel>().FirstOrDefault().Id);
            Assert.Equal("Test", service.GetAll<MainCategoryViewModel>().FirstOrDefault().Name);
            Assert.Equal("TestIcon", service.GetAll<MainCategoryViewModel>().FirstOrDefault().FontAwesomeIcon);
            Assert.Equal(1, service.GetAll<MainCategoryViewModel>().FirstOrDefault().SubcategoriesCount);

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(4));
        }

        [Fact]
        public async Task GetAllGenericShouldMapCorrectlyUsingDbContext()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GetAllGenericShouldMapCorrectlyUsingDbContextMainCategoriesServiceTests").Options;
            using var dbContext = new ApplicationDbContext(options);
            await dbContext.MainCategories.AddAsync(new MainCategory { Name = "Test", FontAwesomeIcon = "TestIcon" });
            await dbContext.Subcategories.AddAsync(new Subcategory { Name = "TestSubcategory", MainCategoryId = 1 });
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<MainCategory>(dbContext);
            var service = new MainCategoriesService(repository);

            Assert.Equal(1, service.GetAll<MainCategoryViewModel>().FirstOrDefault().Id);
            Assert.Equal("Test", service.GetAll<MainCategoryViewModel>().FirstOrDefault().Name);
            Assert.Equal("TestIcon", service.GetAll<MainCategoryViewModel>().FirstOrDefault().FontAwesomeIcon);
            Assert.Equal(1, service.GetAll<MainCategoryViewModel>().FirstOrDefault().SubcategoriesCount);
        }

        [Fact]
        public void GetAllDeletedGenericShouldReturnCorrectCountUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<MainCategory>>();

            var categoriesList = new List<MainCategory>
            {
                new MainCategory { Id = 1, CreatedOn = DateTime.UtcNow, IsDeleted = true, DeletedOn = DateTime.UtcNow, Name = "Test1", FontAwesomeIcon = "TestIcon1" },
                new MainCategory { Id = 2, CreatedOn = DateTime.UtcNow, IsDeleted = true, DeletedOn = DateTime.UtcNow, Name = "Test2", FontAwesomeIcon = "TestIcon2" },
                new MainCategory { Id = 3, CreatedOn = DateTime.UtcNow, IsDeleted = true, DeletedOn = DateTime.UtcNow, Name = "Test3", FontAwesomeIcon = "TestIcon3" },
            };

            repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(categoriesList.AsQueryable());

            var service = new MainCategoriesService(repository.Object);
            Assert.Equal(3, service.GetAllDeleted<DeletedMainCategoryViewModel>().Count());

            repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Once);
        }

        [Fact]
        public async Task GetAllDeletedGenericShouldReturnCorrectCountUsingDbContext()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GetAllDeletedGenericShouldReturnCorrectCountUsingDbContextMainCategoriesServiceTests").Options;
            using var dbContext = new ApplicationDbContext(options);
            await dbContext.MainCategories.AddAsync(new MainCategory { Name = "Test1", FontAwesomeIcon = "TestIcon1", IsDeleted = true, DeletedOn = DateTime.UtcNow });
            await dbContext.MainCategories.AddAsync(new MainCategory { Name = "Test2", FontAwesomeIcon = "TestIcon2", IsDeleted = true, DeletedOn = DateTime.UtcNow });
            await dbContext.MainCategories.AddAsync(new MainCategory { Name = "Test3", FontAwesomeIcon = "TestIcon3", IsDeleted = true, DeletedOn = DateTime.UtcNow });
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<MainCategory>(dbContext);
            var service = new MainCategoriesService(repository);

            Assert.Equal(3, service.GetAllDeleted<DeletedMainCategoryViewModel>().Count());
        }

        [Fact]
        public void GetAllDeletedGenericShouldMapCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<MainCategory>>();

            var categoriesList = new List<MainCategory>
            {
                new MainCategory
                {
                    Id = 1,
                    CreatedOn = DateTime.UtcNow,
                    IsDeleted = true,
                    DeletedOn = new DateTime(2020, 12, 31, 12, 12, 12),
                    Name = "Test",
                    FontAwesomeIcon = "TestIcon",
                    Subcategories = new List<Subcategory> { new Subcategory { Id = 1, CreatedOn = DateTime.UtcNow, MainCategoryId = 1, Name = "TestSubcategory" } },
                },
            };

            repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(categoriesList.AsQueryable());

            var service = new MainCategoriesService(repository.Object);
            Assert.Equal(1, service.GetAllDeleted<DeletedMainCategoryViewModel>().FirstOrDefault().Id);
            Assert.Equal("Test", service.GetAllDeleted<DeletedMainCategoryViewModel>().FirstOrDefault().Name);
            Assert.Equal("TestIcon", service.GetAllDeleted<DeletedMainCategoryViewModel>().FirstOrDefault().FontAwesomeIcon);
            Assert.Equal(1, service.GetAllDeleted<DeletedMainCategoryViewModel>().FirstOrDefault().SubcategoriesCount);
            Assert.Equal("31-Dec-2020 12:12", service.GetAllDeleted<DeletedMainCategoryViewModel>().FirstOrDefault().DeletedOn);

            repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Exactly(5));
        }

        [Fact]
        public async Task GetAllDeletedGenericShouldMapCorrectlyUsingDbContext()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GetAllDeletedGenericShouldMapCorrectlyUsingDbContextMainCategoriesServiceTests").Options;
            using var dbContext = new ApplicationDbContext(options);
            await dbContext.MainCategories.AddAsync(new MainCategory { Name = "Test", FontAwesomeIcon = "TestIcon", IsDeleted = true, DeletedOn = new DateTime(2020, 12, 31, 12, 12, 12) });
            await dbContext.Subcategories.AddAsync(new Subcategory { Name = "TestSubcategory", MainCategoryId = 1 });
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<MainCategory>(dbContext);
            var service = new MainCategoriesService(repository);

            Assert.Equal(1, service.GetAllDeleted<DeletedMainCategoryViewModel>().FirstOrDefault().Id);
            Assert.Equal("Test", service.GetAllDeleted<DeletedMainCategoryViewModel>().FirstOrDefault().Name);
            Assert.Equal("TestIcon", service.GetAllDeleted<DeletedMainCategoryViewModel>().FirstOrDefault().FontAwesomeIcon);
            Assert.Equal(1, service.GetAllDeleted<DeletedMainCategoryViewModel>().FirstOrDefault().SubcategoriesCount);
            Assert.Equal("31-Dec-2020 12:12", service.GetAllDeleted<DeletedMainCategoryViewModel>().FirstOrDefault().DeletedOn);
        }

        [Fact]
        public void GetByIdGenericShouldMapCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<MainCategory>>();

            var categoriesList = new List<MainCategory>
            {
                new MainCategory
                {
                    Id = 42,
                    CreatedOn = DateTime.UtcNow,
                    Name = "Test",
                    FontAwesomeIcon = "TestIcon",
                    Subcategories = new List<Subcategory> { new Subcategory { Id = 1, CreatedOn = DateTime.UtcNow, MainCategoryId = 42, Name = "TestSubcategory" } },
                },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(categoriesList.AsQueryable());

            var service = new MainCategoriesService(repository.Object);
            Assert.Equal(42, service.GetById<MainCategoryViewModel>(42).Id);
            Assert.Equal("Test", service.GetById<MainCategoryViewModel>(42).Name);
            Assert.Equal("TestIcon", service.GetById<MainCategoryViewModel>(42).FontAwesomeIcon);
            Assert.Equal(1, service.GetById<MainCategoryViewModel>(42).SubcategoriesCount);

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(4));
        }

        [Fact]
        public async Task GetByIdGenericShouldMapCorrectlyUsingDbContext()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GetByIdGenericShouldMapCorrectlyUsingDbContextMainCategoriesServiceTests").Options;
            using var dbContext = new ApplicationDbContext(options);
            await dbContext.MainCategories.AddAsync(new MainCategory { Name = "Test", FontAwesomeIcon = "TestIcon" });
            await dbContext.Subcategories.AddAsync(new Subcategory { Name = "TestSubcategory", MainCategoryId = 1 });
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<MainCategory>(dbContext);
            var service = new MainCategoriesService(repository);

            Assert.Equal(1, service.GetById<MainCategoryViewModel>(1).Id);
            Assert.Equal("Test", service.GetById<MainCategoryViewModel>(1).Name);
            Assert.Equal("TestIcon", service.GetById<MainCategoryViewModel>(1).FontAwesomeIcon);
            Assert.Equal(1, service.GetById<MainCategoryViewModel>(1).SubcategoriesCount);
        }

        [Fact]
        public async Task DeleteAsyncShouldReturnFalseWhenIdIsInvalidUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<MainCategory>>();

            var categoriesList = new List<MainCategory>
            {
                new MainCategory { Id = 1, CreatedOn = DateTime.UtcNow, Name = "Test1", FontAwesomeIcon = "TestIcon1" },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(categoriesList.AsQueryable());

            var service = new MainCategoriesService(repository.Object);
            Assert.False(await service.DeleteAsync(2));

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsyncShouldReturnFalseWhenIdIsInvalidUsingDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "DeleteAsyncShouldReturnFalseWhenIdIsInvalidUsingDbContextMainCategoriesServiceTests").Options;
            using var dbContext = new ApplicationDbContext(options);
            await dbContext.MainCategories.AddAsync(new MainCategory { Name = "Test1", FontAwesomeIcon = "TestIcon1" });
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<MainCategory>(dbContext);
            var service = new MainCategoriesService(repository);

            Assert.False(await service.DeleteAsync(2));
        }

        [Fact]
        public async Task DeleteAsyncShouldReturnFalseWhenMainCategoryHasSubcategoriesUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<MainCategory>>();

            var categoriesList = new List<MainCategory>
            {
                new MainCategory
                {
                    Id = 1,
                    CreatedOn = DateTime.UtcNow,
                    Name = "Test",
                    FontAwesomeIcon = "TestIcon",
                    Subcategories = new List<Subcategory> { new Subcategory { Id = 1, CreatedOn = DateTime.UtcNow, MainCategoryId = 1, Name = "TestSubcategory" } },
                },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(categoriesList.AsQueryable());

            var service = new MainCategoriesService(repository.Object);
            Assert.False(await service.DeleteAsync(1));

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsyncShouldReturnFalseWhenMainCategoryHasSubcategoriesUsingDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "DeleteAsyncShouldReturnFalseWhenMainCategoryHasSubcategoriesUsingDbContextMainCategoriesServiceTests").Options;
            using var dbContext = new ApplicationDbContext(options);
            await dbContext.MainCategories.AddAsync(new MainCategory { Name = "Test", FontAwesomeIcon = "TestIcon" });
            await dbContext.Subcategories.AddAsync(new Subcategory { Name = "TestSubcategory", MainCategoryId = 1 });
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<MainCategory>(dbContext);
            var service = new MainCategoriesService(repository);

            Assert.False(await service.DeleteAsync(1));
        }

        [Fact]
        public async Task DeleteAsyncShouldWorkCorrectlyUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<MainCategory>>();

            var categoriesList = new List<MainCategory>
            {
                new MainCategory { Id = 1, CreatedOn = DateTime.UtcNow, Name = "Test1", FontAwesomeIcon = "TestIcon1" },
                new MainCategory { Id = 2, CreatedOn = DateTime.UtcNow, Name = "Test2", FontAwesomeIcon = "TestIcon2" },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(categoriesList.AsQueryable());
            repository.Setup(r => r.Delete(It.IsAny<MainCategory>())).Callback((MainCategory item) => categoriesList.Remove(item));

            var service = new MainCategoriesService(repository.Object);
            Assert.True(await service.DeleteAsync(1));
            Assert.Single(service.GetAll());

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(2));
        }

        [Fact]
        public async Task DeleteAsyncShouldWorkCorrectlyUsingDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "DeleteAsyncShouldWorkCorrectlyUsingDbContextMainCategoriesServiceTests").Options;
            using (var dbContext = new ApplicationDbContext(options))
            {
                await dbContext.MainCategories.AddAsync(new MainCategory { Name = "Test1", FontAwesomeIcon = "TestIcon1" });
                await dbContext.MainCategories.AddAsync(new MainCategory { Name = "Test2", FontAwesomeIcon = "TestIcon2" });
                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new ApplicationDbContext(options))
            {
                using var repository = new EfDeletableEntityRepository<MainCategory>(dbContext);
                var service = new MainCategoriesService(repository);

                Assert.True(await service.DeleteAsync(1));
                Assert.Single(service.GetAll());
            }
        }

        [Fact]
        public async Task UndeleteAsyncShouldReturnFalseWhenIdIsInvalidUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<MainCategory>>();

            var categoriesList = new List<MainCategory>
            {
                new MainCategory { Id = 1, CreatedOn = DateTime.UtcNow, Name = "Test1", FontAwesomeIcon = "TestIcon1", IsDeleted = true, DeletedOn = DateTime.UtcNow },
            };

            repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(categoriesList.AsQueryable());

            var service = new MainCategoriesService(repository.Object);
            Assert.False(await service.UndeleteAsync(2));

            repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Once);
        }

        [Fact]
        public async Task UndeleteAsyncShouldReturnFalseWhenIdIsInvalidUsingDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "UndeleteAsyncShouldReturnFalseWhenIdIsInvalidUsingDbContextMainCategoriesServiceTests").Options;
            using var dbContext = new ApplicationDbContext(options);
            await dbContext.MainCategories.AddAsync(new MainCategory { Name = "Test1", FontAwesomeIcon = "TestIcon1" });
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<MainCategory>(dbContext);
            var service = new MainCategoriesService(repository);

            Assert.False(await service.UndeleteAsync(2));
        }

        [Fact]
        public async Task UndeleteAsyncShouldWorkCorrectlyUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<MainCategory>>();

            var categoriesList = new List<MainCategory>
            {
                new MainCategory { Id = 1, CreatedOn = DateTime.UtcNow, Name = "Test1", FontAwesomeIcon = "TestIcon1", IsDeleted = true, DeletedOn = DateTime.UtcNow },
                new MainCategory { Id = 2, CreatedOn = DateTime.UtcNow, Name = "Test2", FontAwesomeIcon = "TestIcon2" },
                new MainCategory { Id = 3, CreatedOn = DateTime.UtcNow, Name = "Test3", FontAwesomeIcon = "TestIcon3", IsDeleted = true, DeletedOn = DateTime.UtcNow },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(categoriesList.Where(x => !x.IsDeleted).AsQueryable());
            repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(categoriesList.AsQueryable());
            repository.Setup(r => r.Undelete(It.IsAny<MainCategory>())).Callback((MainCategory item) =>
            {
                var foundItem = categoriesList.FirstOrDefault(x => x.Id == item.Id);
                foundItem.IsDeleted = false;
                foundItem.DeletedOn = null;
            });

            var service = new MainCategoriesService(repository.Object);
            Assert.True(await service.UndeleteAsync(1));
            Assert.Equal(2, service.GetAll().Count());

            repository.Verify(x => x.AllAsNoTracking(), Times.Once());
            repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Once());
        }

        [Fact]
        public async Task UndeleteAsyncShouldWorkCorrectlyUsingDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "UndeleteAsyncShouldWorkCorrectlyUsingDbContextMainCategoriesServiceTests").Options;
            using (var dbContext = new ApplicationDbContext(options))
            {
                await dbContext.MainCategories.AddAsync(new MainCategory { Name = "Test1", FontAwesomeIcon = "TestIcon1", IsDeleted = true, DeletedOn = DateTime.UtcNow });
                await dbContext.MainCategories.AddAsync(new MainCategory { Name = "Test2", FontAwesomeIcon = "TestIcon2" });
                await dbContext.MainCategories.AddAsync(new MainCategory { Name = "Test3", FontAwesomeIcon = "TestIcon3", IsDeleted = true, DeletedOn = DateTime.UtcNow });
                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new ApplicationDbContext(options))
            {
                using var repository = new EfDeletableEntityRepository<MainCategory>(dbContext);
                var service = new MainCategoriesService(repository);

                Assert.True(await service.UndeleteAsync(1));
                Assert.Equal(2, service.GetAll().Count());
            }
        }

        [Fact]
        public async Task CreateAsyncGenericShouldWorkCorrectlyWithOneItemUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<MainCategory>>();

            var categoriesList = new List<MainCategory>
            {
                new MainCategory { Id = 1, CreatedOn = DateTime.UtcNow, Name = "Test1", FontAwesomeIcon = "TestIcon1" },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(categoriesList.AsQueryable());
            repository.Setup(r => r.AddAsync(It.IsAny<MainCategory>())).Callback((MainCategory item) => categoriesList.Add(item));
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new MainCategoriesService(repository.Object);
            var model = new CreateMainCategoryInputViewModel
            {
                Name = "Test2",
                FontAwesomeIcon = "TestIcon2",
            };
            await service.CreateAsync<CreateMainCategoryInputViewModel>(model);

            Assert.Equal(2, service.GetAll().Count());
            Assert.Equal("Test2", service.GetAll().ElementAt(1).Name);
            Assert.Equal("TestIcon2", service.GetAll().ElementAt(1).FontAwesomeIcon);

            repository.Verify(x => x.AddAsync(It.IsAny<MainCategory>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsyncGenericShouldWorkCorrectlyWithOneItemUsingDbContext()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "CreateAsyncGenericShouldWorkCorrectlyWithOneItemUsingDbContextMainCategoriesServiceTests").Options;

            var dbContext = new ApplicationDbContext(options);
            await dbContext.MainCategories.AddAsync(new MainCategory { Name = "Test1", FontAwesomeIcon = "TestIcon1" });
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<MainCategory>(dbContext);
            var service = new MainCategoriesService(repository);
            var model = new CreateMainCategoryInputViewModel
            {
                Name = "Test2",
                FontAwesomeIcon = "TestIcon2",
            };
            await service.CreateAsync<CreateMainCategoryInputViewModel>(model);

            Assert.Equal(2, service.GetAll().Count());
            Assert.Equal("Test2", service.GetAll().ElementAt(1).Name);
            Assert.Equal("TestIcon2", service.GetAll().ElementAt(1).FontAwesomeIcon);
        }

        [Fact]
        public async Task CreateAsyncGenericShouldWorkCorrectlyWithMultipleItemsUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<MainCategory>>();

            var categoriesList = new List<MainCategory>
            {
                new MainCategory { Id = 1, CreatedOn = DateTime.UtcNow, Name = "Test1", FontAwesomeIcon = "TestIcon1" },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(categoriesList.AsQueryable());
            repository.Setup(r => r.AddAsync(It.IsAny<MainCategory>())).Callback((MainCategory item) => categoriesList.Add(item));
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new MainCategoriesService(repository.Object);
            await service.CreateAsync<CreateMainCategoryInputViewModel>(new CreateMainCategoryInputViewModel { Name = "Test2", FontAwesomeIcon = "TestIcon2" });
            await service.CreateAsync<CreateMainCategoryInputViewModel>(new CreateMainCategoryInputViewModel { Name = "Test3", FontAwesomeIcon = "TestIcon3" });

            Assert.Equal(3, service.GetAll().Count());
            Assert.Equal("Test2", service.GetAll().ElementAt(1).Name);
            Assert.Equal("TestIcon2", service.GetAll().ElementAt(1).FontAwesomeIcon);
            Assert.Equal("Test3", service.GetAll().ElementAt(2).Name);
            Assert.Equal("TestIcon3", service.GetAll().ElementAt(2).FontAwesomeIcon);

            repository.Verify(x => x.AddAsync(It.IsAny<MainCategory>()), Times.Exactly(2));
            repository.Verify(x => x.SaveChangesAsync(), Times.Exactly(2));
        }

        [Fact]
        public async Task CreateAsyncGenericShouldWorkCorrectlyWithMultipleItemsUsingDbContext()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "CreateAsyncGenericShouldWorkCorrectlyWithMultipleItemsUsingDbContextMainCategoriesServiceTests").Options;

            var dbContext = new ApplicationDbContext(options);
            await dbContext.MainCategories.AddAsync(new MainCategory { Name = "Test1", FontAwesomeIcon = "TestIcon1" });
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<MainCategory>(dbContext);
            var service = new MainCategoriesService(repository);
            await service.CreateAsync<CreateMainCategoryInputViewModel>(new CreateMainCategoryInputViewModel { Name = "Test2", FontAwesomeIcon = "TestIcon2" });
            await service.CreateAsync<CreateMainCategoryInputViewModel>(new CreateMainCategoryInputViewModel { Name = "Test3", FontAwesomeIcon = "TestIcon3" });

            Assert.Equal(3, service.GetAll().Count());
            Assert.Equal("Test2", service.GetAll().ElementAt(1).Name);
            Assert.Equal("TestIcon2", service.GetAll().ElementAt(1).FontAwesomeIcon);
            Assert.Equal("Test3", service.GetAll().ElementAt(2).Name);
            Assert.Equal("TestIcon3", service.GetAll().ElementAt(2).FontAwesomeIcon);
        }

        [Fact]
        public async Task EditAsyncShouldReturnFalseWhenModelIdIsInvalidUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<MainCategory>>();

            var categoriesList = new List<MainCategory>
            {
                new MainCategory { Id = 1, CreatedOn = DateTime.UtcNow, Name = "Test1", FontAwesomeIcon = "TestIcon1" },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(categoriesList.AsQueryable());

            var service = new MainCategoriesService(repository.Object);
            Assert.False(await service.EditAsync<EditMainCategoryViewModel>(new EditMainCategoryViewModel { Id = 2 }));

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task EditAsyncShouldReturnFalseWhenModelIdIsInvalidUsingDbContext()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "EditAsyncShouldReturnFalseWhenModelIdIsInvalidUsingDbContextMainCategoriesServiceTests").Options;
            using var dbContext = new ApplicationDbContext(options);
            await dbContext.MainCategories.AddAsync(new MainCategory { Name = "Test1", FontAwesomeIcon = "TestIcon1" });
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<MainCategory>(dbContext);
            var service = new MainCategoriesService(repository);

            Assert.False(await service.EditAsync<EditMainCategoryViewModel>(new EditMainCategoryViewModel { Id = 2 }));
        }

        [Fact]
        public async Task EditAsyncShouldWorkCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<MainCategory>>();

            var categoriesList = new List<MainCategory>
            {
                new MainCategory { Id = 1, CreatedOn = DateTime.UtcNow, Name = "Test1", FontAwesomeIcon = "TestIcon1" },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(categoriesList.AsQueryable());
            repository.Setup(r => r.Update(It.IsAny<MainCategory>())).Callback((MainCategory item) =>
            {
                var foundCategory = categoriesList.FirstOrDefault(x => x.Id == item.Id);
                foundCategory.Name = item.Name;
                foundCategory.FontAwesomeIcon = item.FontAwesomeIcon;
            });
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new MainCategoriesService(repository.Object);
            Assert.True(await service.EditAsync<EditMainCategoryViewModel>(new EditMainCategoryViewModel { Id = 1, Name = "Test1Edited", FontAwesomeIcon = "TestIcon1Edited" }));
            Assert.Equal("Test1Edited", service.GetAll().FirstOrDefault(x => x.Id == 1).Name);
            Assert.Equal("TestIcon1Edited", service.GetAll().FirstOrDefault(x => x.Id == 1).FontAwesomeIcon);

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(3));
            repository.Verify(x => x.Update(It.IsAny<MainCategory>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task EditAsyncShouldWorkCorrectlyUsingDbContext()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "EditAsyncShouldWorkCorrectlyUsingDbContextMainCategoriesServiceTests").Options;
            using (var dbContext = new ApplicationDbContext(options))
            {
                await dbContext.MainCategories.AddAsync(new MainCategory { Name = "Test1", FontAwesomeIcon = "TestIcon1" });
                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new ApplicationDbContext(options))
            {
                using var repository = new EfDeletableEntityRepository<MainCategory>(dbContext);
                var service = new MainCategoriesService(repository);

                Assert.True(await service.EditAsync<EditMainCategoryViewModel>(new EditMainCategoryViewModel { Id = 1, Name = "Test1Edited", FontAwesomeIcon = "TestIcon1Edited" }));
                Assert.Equal("Test1Edited", service.GetAll().FirstOrDefault(x => x.Id == 1).Name);
                Assert.Equal("TestIcon1Edited", service.GetAll().FirstOrDefault(x => x.Id == 1).FontAwesomeIcon);
            }
        }
    }
}
