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
    using XeMart.Services.Mapping;
    using XeMart.Web.ViewModels;
    using XeMart.Web.ViewModels.Addresses;

    using Xunit;

    [Collection("Sequential")]
    public class AddressesServiceTests
    {
        [Fact]
        public void GetAllGenericShouldReturnCorrectCountUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Address>>();

            var country = new Country
            {
                Id = 1,
                Name = "Bulgaria",
            };

            var city = new City
            {
                Id = 1,
                Name = "Ruse",
                ZIPCode = "7000",
                Country = country,
            };

            var addressesList = new List<Address>
            {
                new Address { Id = "TestId1", CreatedOn = DateTime.UtcNow, City = city, Street = "Test Street 1", UserId = "Test User 1" },
                new Address { Id = "TestId2", CreatedOn = DateTime.UtcNow, City = city, Street = "Test Street 2", UserId = "Test User 2" },
                new Address { Id = "TestId3", CreatedOn = DateTime.UtcNow, City = city, Street = "Test Street 3", UserId = "Test User 1" },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(addressesList.AsQueryable());

            var service = new AddressesService(repository.Object, null);
            Assert.Equal(2, service.GetAll<AddressViewModel>("Test User 1").Count());

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetAllGenericShouldMapCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Address>>();

            var country = new Country
            {
                Id = 1,
                Name = "Bulgaria",
            };

            var city = new City
            {
                Id = 1,
                Name = "Ruse",
                ZIPCode = "7000",
                Country = country,
            };

            var addressesList = new List<Address>
            {
                new Address { Id = "TestId1", CreatedOn = DateTime.UtcNow, City = city, Street = "Test Street 1", UserId = "Test User 1", Description = "TestDescription" },
                new Address { Id = "TestId1", CreatedOn = DateTime.UtcNow, City = city, Street = "Test Street 2", UserId = "Test User 2" },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(addressesList.AsQueryable());

            var service = new AddressesService(repository.Object, null);
            Assert.Equal("TestId1", service.GetAll<AddressViewModel>("Test User 1").FirstOrDefault().Id);
            Assert.Equal("Test Street 1", service.GetAll<AddressViewModel>("Test User 1").FirstOrDefault().Street);
            Assert.Equal("TestDescription", service.GetAll<AddressViewModel>("Test User 1").FirstOrDefault().Description);
            Assert.Equal("Ruse", service.GetAll<AddressViewModel>("Test User 1").FirstOrDefault().CityName);
            Assert.Equal("7000", service.GetAll<AddressViewModel>("Test User 1").FirstOrDefault().CityZIPCode);
            Assert.Equal("Bulgaria", service.GetAll<AddressViewModel>("Test User 1").FirstOrDefault().CityCountryName);

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(6));
        }

        [Fact]
        public void GetAllGenericShouldReturnEmptyCollectionIfUserIdIsNotFoundUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Address>>();

            var country = new Country
            {
                Id = 1,
                Name = "Bulgaria",
            };

            var city = new City
            {
                Id = 1,
                Name = "Ruse",
                ZIPCode = "7000",
                Country = country,
            };

            var addressesList = new List<Address>
            {
                new Address { Id = "TestId1", CreatedOn = DateTime.UtcNow, City = city, Street = "Test Street 1", UserId = "Test User 1" },
                new Address { Id = "TestId2", CreatedOn = DateTime.UtcNow, City = city, Street = "Test Street 2", UserId = "Test User 2" },
                new Address { Id = "TestId3", CreatedOn = DateTime.UtcNow, City = city, Street = "Test Street 3", UserId = "Test User 1" },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(addressesList.AsQueryable());

            var service = new AddressesService(repository.Object, null);
            Assert.Empty(service.GetAll<AddressViewModel>("Test User 3"));

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsyncShouldReturnFalseWhenIdIsInvalidUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<Address>>();

            var addressesList = new List<Address>
            {
                new Address { Id = "TestId1", CreatedOn = DateTime.UtcNow, Street = "Test Street 1", },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(addressesList.AsQueryable());

            var service = new AddressesService(repository.Object, null);
            Assert.False(await service.DeleteAsync("TestId"));

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsyncShouldWorkCorrectlyUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<Address>>();

            var addressesList = new List<Address>
            {
                new Address { Id = "TestId1", CreatedOn = DateTime.UtcNow, Street = "Test Street 1" },
                new Address { Id = "TestId2", CreatedOn = DateTime.UtcNow, Street = "Test Street 2" },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(addressesList.AsQueryable());
            repository.Setup(r => r.Delete(It.IsAny<Address>())).Callback((Address item) => addressesList.Remove(item));
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new AddressesService(repository.Object, null);
            Assert.True(await service.DeleteAsync("TestId1"));
            Assert.Single(addressesList);

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
            repository.Verify(x => x.Delete(It.IsAny<Address>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsyncShouldReturnFalseIfAddressAlreadyExistsUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var addressRepository = new Mock<IDeletableEntityRepository<Address>>();
            var cityRepository = new Mock<IRepository<City>>();

            var citiesList = new List<City>
            {
                new City { Id = 1, Name = "Ruse", ZIPCode = "7000", CountryId = 42 },
                new City { Id = 2, Name = "Sofia", ZIPCode = "1000", CountryId = 42 },
            };

            var addressesList = new List<Address>
            {
                new Address { Id = "TestId1", CreatedOn = DateTime.UtcNow, Street = "TestStreet", City = citiesList.ElementAt(0), CityId = 1, Description = "TestDescription", UserId = "TestUserId" },
            };

            addressRepository.Setup(r => r.AllAsNoTracking()).Returns(addressesList.AsQueryable());

            cityRepository.Setup(r => r.All()).Returns(citiesList.AsQueryable());

            var service = new AddressesService(addressRepository.Object, cityRepository.Object);
            var model = new AddressInputViewModel
            {
                Street = "TestStreet",
                Description = "TestDescription",
                UserId = "TestUserId",
                City = "Ruse",
                ZIPCode = "7000",
                CountryId = 42,
            };

            Assert.False(await service.CreateAsync(model));

            cityRepository.Verify(x => x.All(), Times.Once);
            addressRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Theory]
        [InlineData("TestStreet2", "TestDescription", "TestUserId")]
        [InlineData("TestStreet", "TestDescription2", "TestUserId")]
        [InlineData("TestStreet", "TestDescription", "TestUserId2")]
        [InlineData("TestStreet2", "TestDescription2", "TestUserId2")]
        public async Task CreateAsyncShouldWorkCorrectlyWithExistingCityUsingMoq(string streetName, string description, string userId)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var addressRepository = new Mock<IDeletableEntityRepository<Address>>();
            var cityRepository = new Mock<IRepository<City>>();

            var citiesList = new List<City>
            {
                new City { Id = 1, Name = "Ruse", ZIPCode = "7000", CountryId = 42 },
                new City { Id = 2, Name = "Sofia", ZIPCode = "1000", CountryId = 42 },
            };

            var addressesList = new List<Address>
            {
                new Address { Id = "TestId1", CreatedOn = DateTime.UtcNow, Street = "TestStreet", City = citiesList.ElementAt(0), CityId = 1, Description = "TestDescription", UserId = "TestUserId" },
            };

            addressRepository.Setup(r => r.AllAsNoTracking()).Returns(addressesList.AsQueryable());
            addressRepository.Setup(r => r.AddAsync(It.IsAny<Address>())).Callback((Address item) => addressesList.Add(item));
            addressRepository.Setup(r => r.SaveChangesAsync()).Verifiable();

            cityRepository.Setup(r => r.All()).Returns(citiesList.AsQueryable());

            var service = new AddressesService(addressRepository.Object, cityRepository.Object);
            var model = new AddressInputViewModel
            {
                Street = streetName,
                Description = description,
                UserId = userId,
                City = "Ruse",
                ZIPCode = "7000",
                CountryId = 42,
            };

            Assert.True(await service.CreateAsync(model));
            Assert.Equal(2, addressesList.Count);
            Assert.Equal(streetName, addressesList.ElementAt(1).Street);
            Assert.Equal(description, addressesList.ElementAt(1).Description);
            Assert.Equal(userId, addressesList.ElementAt(1).UserId);
            Assert.Equal("Ruse", addressesList.ElementAt(1).City.Name);

            cityRepository.Verify(x => x.All(), Times.Once);
            cityRepository.Verify(x => x.AddAsync(It.IsAny<City>()), Times.Never);

            addressRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
            addressRepository.Verify(x => x.AddAsync(It.IsAny<Address>()), Times.Once);
            addressRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Theory]
        [InlineData("Ruse2", "7000", 42)]
        [InlineData("Ruse", "7001", 42)]
        [InlineData("Ruse", "7000", 43)]
        [InlineData("Ruse2", "7001", 43)]
        public async Task CreateAsyncShouldWorkCorrectlyWithUnexistingCityUsingMoq(string cityName, string cityZIPCode, int countryId)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var addressRepository = new Mock<IDeletableEntityRepository<Address>>();
            var cityRepository = new Mock<IRepository<City>>();

            var citiesList = new List<City>
            {
                new City { Id = 1, Name = "Ruse", ZIPCode = "7000", CountryId = 42 },
                new City { Id = 2, Name = "Sofia", ZIPCode = "1000", CountryId = 42 },
            };

            var addressesList = new List<Address>
            {
                new Address { Id = "TestId1", CreatedOn = DateTime.UtcNow, Street = "TestStreet", City = citiesList.ElementAt(0), CityId = 1, Description = "TestDescription", UserId = "TestUserId" },
            };

            addressRepository.Setup(r => r.AllAsNoTracking()).Returns(addressesList.AsQueryable());
            addressRepository.Setup(r => r.AddAsync(It.IsAny<Address>())).Callback((Address item) => addressesList.Add(item));
            addressRepository.Setup(r => r.SaveChangesAsync()).Verifiable();

            cityRepository.Setup(r => r.All()).Returns(citiesList.AsQueryable());
            cityRepository.Setup(r => r.AddAsync(It.IsAny<City>())).Callback((City item) => citiesList.Add(item));

            var service = new AddressesService(addressRepository.Object, cityRepository.Object);
            var model = new AddressInputViewModel
            {
                Street = "TestStreet",
                Description = "TestDescription",
                UserId = "TestUserId",
                City = cityName,
                ZIPCode = cityZIPCode,
                CountryId = countryId,
            };

            Assert.True(await service.CreateAsync(model));
            Assert.Equal(2, addressesList.Count);
            Assert.Equal("TestStreet", addressesList.ElementAt(1).Street);
            Assert.Equal("TestDescription", addressesList.ElementAt(1).Description);
            Assert.Equal("TestUserId", addressesList.ElementAt(1).UserId);
            Assert.Equal(cityName, addressesList.ElementAt(1).City.Name);

            Assert.Equal(3, citiesList.Count);
            Assert.Equal(cityName, citiesList.Last().Name);
            Assert.Equal(cityZIPCode, citiesList.Last().ZIPCode);
            Assert.Equal(countryId, citiesList.Last().CountryId);

            cityRepository.Verify(x => x.All(), Times.Once);
            cityRepository.Verify(x => x.AddAsync(It.IsAny<City>()), Times.Once);

            addressRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
            addressRepository.Verify(x => x.AddAsync(It.IsAny<Address>()), Times.Once);
            addressRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
