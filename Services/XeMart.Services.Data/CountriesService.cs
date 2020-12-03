namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using XeMart.Data.Common.Repositories;
    using XeMart.Data.Models;

    public class CountriesService : ICountriesService
    {
        private readonly IRepository<Country> countriesRepository;

        public CountriesService(IRepository<Country> countriesRepository)
        {
            this.countriesRepository = countriesRepository;
        }

        public IEnumerable<Country> GetAll() =>
            this.countriesRepository.AllAsNoTracking()
            .ToList();
    }
}
