namespace XeMart.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Moq;

    using XeMart.Data.Common.Repositories;
    using XeMart.Data.Models;
    using XeMart.Data.Models.Enums;
    using XeMart.Services.Mapping;
    using XeMart.Web.ViewModels;
    using XeMart.Web.ViewModels.Administration.Suppliers;

    using Xunit;

    [Collection("Sequential")]
    public class SuppliersServiceTests
    {
        [Fact]
        public void GetAllGenericShouldReturnCorrectCountUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Supplier>>();

            var suppliers = new List<Supplier>
            {
                new Supplier { Id = 1 },
                new Supplier { Id = 2 },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(suppliers.AsQueryable());

            var service = new SuppliersService(repository.Object);
            Assert.Equal(2, service.GetAll<SupplierViewModel>().Count());

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetAllGenericShouldMapCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Supplier>>();

            var suppliers = new List<Supplier>
            {
                new Supplier { Id = 1, Name = "TestName1", PriceToHome = 25, PriceToOffice = 30, IsDefault = true },
                new Supplier { Id = 2 },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(suppliers.AsQueryable());

            var service = new SuppliersService(repository.Object);
            Assert.Equal(1, service.GetAll<SupplierViewModel>().FirstOrDefault().Id);
            Assert.Equal("TestName1", service.GetAll<SupplierViewModel>().FirstOrDefault().Name);
            Assert.Equal(25, service.GetAll<SupplierViewModel>().FirstOrDefault().PriceToHome);
            Assert.Equal(30, service.GetAll<SupplierViewModel>().FirstOrDefault().PriceToOffice);
            Assert.True(service.GetAll<SupplierViewModel>().FirstOrDefault().IsDefault);

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(5));
        }

        [Fact]
        public void GetAllDeletedGenericShouldReturnCorrectCountUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Supplier>>();

            var suppliers = new List<Supplier>
            {
                new Supplier { Id = 1, IsDeleted = true, DeletedOn = DateTime.UtcNow},
                new Supplier { Id = 2 },
            };

            repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(suppliers.AsQueryable());

            var service = new SuppliersService(repository.Object);
            Assert.Single(service.GetAllDeleted<DeletedSupplierViewModel>());

            repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Once);
        }

        [Fact]
        public void GetAllDeletedGenericShouldMapCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Supplier>>();

            var suppliers = new List<Supplier>
            {
                new Supplier { Id = 1, Name = "TestName1", PriceToHome = 25, PriceToOffice = 30, IsDefault = true, IsDeleted = true, DeletedOn = new DateTime(2020, 12, 31, 12, 12, 12) },
                new Supplier { Id = 2 },
            };

            repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(suppliers.AsQueryable());

            var service = new SuppliersService(repository.Object);
            Assert.Equal(1, service.GetAllDeleted<DeletedSupplierViewModel>().FirstOrDefault().Id);
            Assert.Equal("TestName1", service.GetAllDeleted<DeletedSupplierViewModel>().FirstOrDefault().Name);
            Assert.Equal(25, service.GetAllDeleted<DeletedSupplierViewModel>().FirstOrDefault().PriceToHome);
            Assert.Equal(30, service.GetAllDeleted<DeletedSupplierViewModel>().FirstOrDefault().PriceToOffice);
            Assert.True(service.GetAllDeleted<DeletedSupplierViewModel>().FirstOrDefault().IsDefault);
            Assert.Equal("31-Dec-2020 12:12", service.GetAllDeleted<DeletedSupplierViewModel>().FirstOrDefault().DeletedOn);

            repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Exactly(6));
        }

        [Fact]
        public void GetByIdGenericShouldMapCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Supplier>>();

            var suppliers = new List<Supplier>
            {
                new Supplier { Id = 1, Name = "TestName1", PriceToHome = 25, PriceToOffice = 30, IsDefault = true },
                new Supplier { Id = 2 },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(suppliers.AsQueryable());

            var service = new SuppliersService(repository.Object);
            Assert.Equal(1, service.GetById<SupplierViewModel>(1).Id);
            Assert.Equal("TestName1", service.GetById<SupplierViewModel>(1).Name);
            Assert.Equal(25, service.GetById<SupplierViewModel>(1).PriceToHome);
            Assert.Equal(30, service.GetById<SupplierViewModel>(1).PriceToOffice);
            Assert.True(service.GetById<SupplierViewModel>(1).IsDefault);

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(5));
        }

        [Fact]
        public void GetDeliveryPriceShouldWorkCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Supplier>>();

            var suppliers = new List<Supplier>
            {
                new Supplier { Id = 1, Name = "TestName1", PriceToHome = 25, PriceToOffice = 30 },
                new Supplier { Id = 2 },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(suppliers.AsQueryable());

            var service = new SuppliersService(repository.Object);
            Assert.Equal(25, service.GetDeliveryPrice(1, DeliveryType.Home));
            Assert.Equal(30, service.GetDeliveryPrice(1, DeliveryType.Office));

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(2));
        }

        [Fact]
        public async Task CreateAsyncGenericShouldIncreaseCountUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Supplier>>();

            var suppliers = new List<Supplier>
            {
                new Supplier { Id = 1, Name = "TestName1", PriceToHome = 25, PriceToOffice = 30 },
            };

            repository.Setup(r => r.AddAsync(It.IsAny<Supplier>())).Callback((Supplier item) => suppliers.Add(item));
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new SuppliersService(repository.Object);
            var model = new CreateSupplierInputViewModel
            {
                Name = "TestName2",
                PriceToHome = 35,
                PriceToOffice = 40,
            };
            await service.CreateAsync<CreateSupplierInputViewModel>(model);

            Assert.Equal(2, suppliers.Count);

            repository.Verify(x => x.AddAsync(It.IsAny<Supplier>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task CreateAsyncGenericShouldMapCorrectlyWhenThereIsNoDefaultSupplierUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Supplier>>();

            var suppliers = new List<Supplier>
            {
                new Supplier { Id = 1, Name = "TestName1", PriceToHome = 25, PriceToOffice = 30 },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(suppliers.AsQueryable());
            repository.Setup(r => r.AddAsync(It.IsAny<Supplier>())).Callback((Supplier item) => suppliers.Add(item));
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new SuppliersService(repository.Object);
            var model = new CreateSupplierInputViewModel
            {
                Name = "TestName2",
                PriceToHome = 35,
                PriceToOffice = 40,
            };
            await service.CreateAsync<CreateSupplierInputViewModel>(model);

            Assert.Equal("TestName2", suppliers.Last().Name);
            Assert.Equal(35, suppliers.Last().PriceToHome);
            Assert.Equal(40, suppliers.Last().PriceToOffice);
            Assert.True(suppliers.Last().IsDefault);

            repository.Verify(x => x.AddAsync(It.IsAny<Supplier>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task CreateAsyncGenericShouldMapCorrectlyWhenThereIsDefaultSupplierUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Supplier>>();

            var suppliers = new List<Supplier>
            {
                new Supplier { Id = 1, Name = "TestName1", PriceToHome = 25, PriceToOffice = 30, IsDefault = true },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(suppliers.AsQueryable());
            repository.Setup(r => r.AddAsync(It.IsAny<Supplier>())).Callback((Supplier item) => suppliers.Add(item));
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new SuppliersService(repository.Object);
            var model = new CreateSupplierInputViewModel
            {
                Name = "TestName2",
                PriceToHome = 35,
                PriceToOffice = 40,
            };
            await service.CreateAsync<CreateSupplierInputViewModel>(model);

            Assert.Equal("TestName2", suppliers.Last().Name);
            Assert.Equal(35, suppliers.Last().PriceToHome);
            Assert.Equal(40, suppliers.Last().PriceToOffice);
            Assert.False(suppliers.Last().IsDefault);

            repository.Verify(x => x.AddAsync(It.IsAny<Supplier>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task MakeDafaultAsyncShouldReturnFalseWithInvalidSupplierIdUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Supplier>>();

            var suppliers = new List<Supplier>
            {
                new Supplier { Id = 1, Name = "TestName1", PriceToHome = 25, PriceToOffice = 30, IsDefault = true },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(suppliers.AsQueryable());

            var service = new SuppliersService(repository.Object);

            Assert.False(await service.MakeDafaultAsync(2));

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task MakeDafaultAsyncShouldWorkCorrectlyWhenThereIsNotAnyDefaultSupplierUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Supplier>>();

            var suppliers = new List<Supplier>
            {
                new Supplier { Id = 1, Name = "TestName1", PriceToHome = 25, PriceToOffice = 30, IsDefault = false },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(suppliers.AsQueryable());
            repository.Setup(r => r.Update(It.IsAny<Supplier>())).Callback((Supplier item) =>
            {
                var foundSupplier = suppliers.FirstOrDefault(x => x.Id == item.Id);
                foundSupplier.IsDefault = item.IsDefault;
            });
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new SuppliersService(repository.Object);

            Assert.True(await service.MakeDafaultAsync(1));
            Assert.True(suppliers.FirstOrDefault().IsDefault);

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(2));
            repository.Verify(x => x.Update(It.IsAny<Supplier>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task MakeDafaultAsyncShouldWorkCorrectlyWhenThereIsADefaultSupplierUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Supplier>>();

            var suppliers = new List<Supplier>
            {
                new Supplier { Id = 1, Name = "TestName1", PriceToHome = 25, PriceToOffice = 30, IsDefault = true },
                new Supplier { Id = 2, Name = "TestName2", PriceToHome = 25, PriceToOffice = 30, IsDefault = false },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(suppliers.AsQueryable());
            repository.Setup(r => r.Update(It.IsAny<Supplier>())).Callback((Supplier item) =>
            {
                var foundSupplier = suppliers.FirstOrDefault(x => x.Id == item.Id);
                foundSupplier.IsDefault = item.IsDefault;
            });
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new SuppliersService(repository.Object);

            Assert.True(await service.MakeDafaultAsync(2));
            Assert.False(suppliers.FirstOrDefault(x => x.Id == 1).IsDefault);
            Assert.True(suppliers.FirstOrDefault(x => x.Id == 2).IsDefault);

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(2));
            repository.Verify(x => x.Update(It.IsAny<Supplier>()), Times.Exactly(2));
            repository.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task EditAsyncGenericShouldReturnFalseWithInvalidSupplierIdUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Supplier>>();

            var suppliers = new List<Supplier>
            {
                new Supplier { Id = 1, Name = "TestName1", PriceToHome = 25, PriceToOffice = 30, IsDefault = false },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(suppliers.AsQueryable());

            var service = new SuppliersService(repository.Object);

            var model = new EditSupplierViewModel { Id = 2 };
            Assert.False(await service.EditAsync<EditSupplierViewModel>(model));

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Theory]
        [InlineData("TestName2", 25, 30)]
        [InlineData("TestName1", 27, 30)]
        [InlineData("TestName1", 25, 35)]
        [InlineData("TestName2", 30, 40)]
        public async Task EditAsyncGenericShouldWorkCorrectlyUsingMoq(string name, decimal priceToHome, decimal priceToOffice)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Supplier>>();

            var suppliers = new List<Supplier>
            {
                new Supplier { Id = 1, Name = "TestName1", PriceToHome = 25, PriceToOffice = 30, IsDefault = true },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(suppliers.AsQueryable());
            repository.Setup(r => r.Update(It.IsAny<Supplier>())).Callback((Supplier item) =>
            {
                var foundSupplier = suppliers.FirstOrDefault(x => x.Id == item.Id);
                foundSupplier.Name = item.Name;
                foundSupplier.PriceToHome = item.PriceToHome;
                foundSupplier.PriceToOffice = item.PriceToOffice;
            });
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new SuppliersService(repository.Object);

            var model = new EditSupplierViewModel
            {
                Id = 1,
                Name = name,
                PriceToHome = priceToHome,
                PriceToOffice = priceToOffice,
            };
            Assert.True(await service.EditAsync<EditSupplierViewModel>(model));
            Assert.Equal(name, suppliers.FirstOrDefault(x => x.Id == model.Id).Name);
            Assert.Equal(priceToHome, suppliers.FirstOrDefault(x => x.Id == model.Id).PriceToHome);
            Assert.Equal(priceToOffice, suppliers.FirstOrDefault(x => x.Id == model.Id).PriceToOffice);

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
            repository.Verify(x => x.Update(It.IsAny<Supplier>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task DeleteAsyncShouldReturnFalseWithInvalidSupplierIdUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Supplier>>();

            var suppliers = new List<Supplier>
            {
                new Supplier { Id = 1, Name = "TestName1", PriceToHome = 25, PriceToOffice = 30 },
                new Supplier { Id = 2 },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(suppliers.AsQueryable());

            var service = new SuppliersService(repository.Object);

            Assert.False(await service.DeleteAsync(42));

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsyncShouldReturnFalseWithDefaultSupplierUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Supplier>>();

            var suppliers = new List<Supplier>
            {
                new Supplier { Id = 1, Name = "TestName1", PriceToHome = 25, PriceToOffice = 30, IsDefault = true },
                new Supplier { Id = 2 },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(suppliers.AsQueryable());

            var service = new SuppliersService(repository.Object);

            Assert.False(await service.DeleteAsync(1));

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsyncShouldWorkCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Supplier>>();

            var suppliers = new List<Supplier>
            {
                new Supplier { Id = 1, Name = "TestName1", PriceToHome = 25, PriceToOffice = 30 },
                new Supplier { Id = 2 },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(suppliers.AsQueryable());
            repository.Setup(r => r.Delete(It.IsAny<Supplier>())).Callback((Supplier item) => suppliers.Remove(item));
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new SuppliersService(repository.Object);

            Assert.True(await service.DeleteAsync(1));
            Assert.Single(suppliers);

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
            repository.Verify(x => x.Delete(It.IsAny<Supplier>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task UndeleteAsyncShouldReturnFalseWithInvalidSupplierIdUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Supplier>>();

            var suppliers = new List<Supplier>
            {
                new Supplier { Id = 1, Name = "TestName1", PriceToHome = 25, PriceToOffice = 30, IsDeleted = true, DeletedOn = DateTime.UtcNow },
                new Supplier { Id = 2 },
            };

            repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(suppliers.AsQueryable());

            var service = new SuppliersService(repository.Object);

            Assert.False(await service.UndeleteAsync(42));

            repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Once);
        }

        [Fact]
        public async Task UndeleteAsyncShouldWorkCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Supplier>>();

            var suppliers = new List<Supplier>
            {
                new Supplier { Id = 1, Name = "TestName1", PriceToHome = 25, PriceToOffice = 30, IsDeleted = true, DeletedOn = DateTime.UtcNow },
                new Supplier { Id = 2 },
            };

            repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(suppliers.AsQueryable());
            repository.Setup(r => r.Undelete(It.IsAny<Supplier>())).Callback((Supplier item) =>
            {
                var foundSupplier = suppliers.FirstOrDefault(x => x.Id == item.Id);
                foundSupplier.IsDeleted = false;
                foundSupplier.DeletedOn = null;
            });
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new SuppliersService(repository.Object);

            Assert.True(await service.UndeleteAsync(1));
            Assert.False(suppliers.FirstOrDefault().IsDeleted);
            Assert.Null(suppliers.FirstOrDefault().DeletedOn);

            repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Once);
            repository.Verify(x => x.Undelete(It.IsAny<Supplier>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());
        }
    }
}
