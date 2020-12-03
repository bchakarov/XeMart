namespace XeMart.Services.Data
{
    using System.Collections.Generic;

    using XeMart.Data.Models;

    public interface ICountriesService
    {
        public IEnumerable<Country> GetAll();
    }
}
