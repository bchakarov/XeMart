namespace XeMart.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Moq;

    using XeMart.Data;
    using XeMart.Data.Common.Repositories;
    using XeMart.Data.Models;
    using XeMart.Data.Repositories;

    using Xunit;

    [Collection("Sequential")]
    public class CountriesServiceTests
    {
        [Fact]
        public void GetAllShouldWorkCorrectlyUsingMoq()
        {
            var repository = new Mock<IRepository<Country>>();

            var countriesList = new List<Country>
            {
                new Country { Id = 1, CreatedOn = DateTime.UtcNow, Name = "TestCountry1" },
                new Country { Id = 1, CreatedOn = DateTime.UtcNow, Name = "TestCountry2" },
                new Country { Id = 1, CreatedOn = DateTime.UtcNow, Name = "TestCountry3" },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(countriesList.AsQueryable());

            var service = new CountriesService(repository.Object);
            Assert.Equal(countriesList, service.GetAll());

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task GetAllShouldWorkCorrectlyUsingDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GetAllShouldWorkCorrectlyUsingDbContextCountriesServiceTests").Options;
            using var dbContext = new ApplicationDbContext(options);
            await dbContext.Countries.AddAsync(new Country { Name = "TestCountry1" });
            await dbContext.Countries.AddAsync(new Country { Name = "TestCountry2" });
            await dbContext.Countries.AddAsync(new Country { Name = "TestCountry3" });
            await dbContext.SaveChangesAsync();

            using var repository = new EfRepository<Country>(dbContext);
            var service = new CountriesService(repository);

            Assert.Equal(3, service.GetAll().Count());
            Assert.Equal(1, service.GetAll().FirstOrDefault().Id);
            Assert.Equal("TestCountry1", service.GetAll().FirstOrDefault().Name);
            Assert.Equal(2, service.GetAll().ElementAt(1).Id);
            Assert.Equal("TestCountry2", service.GetAll().ElementAt(1).Name);
            Assert.Equal(3, service.GetAll().ElementAt(2).Id);
            Assert.Equal("TestCountry3", service.GetAll().ElementAt(2).Name);
        }

        [Fact]
        public void GetAllShouldWorkCorrectlyWithNoAddedCountriesUsingMoq()
        {
            var repository = new Mock<IRepository<Country>>();

            var countriesList = new List<Country>();

            repository.Setup(r => r.AllAsNoTracking()).Returns(countriesList.AsQueryable());

            var service = new CountriesService(repository.Object);
            Assert.Empty(service.GetAll());
            Assert.Equal(countriesList, service.GetAll());

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(2));
        }

        [Fact]
        public void GetAllShouldWorkCorrectlyWithNoAddedCategoriesUsingDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GetAllShouldWorkCorrectlyWithNoAddedCategoriesUsingDbContextCountriesServiceTests").Options;
            using var dbContext = new ApplicationDbContext(options);

            using var repository = new EfRepository<Country>(dbContext);
            var service = new CountriesService(repository);

            Assert.Empty(service.GetAll());
        }
    }
}
