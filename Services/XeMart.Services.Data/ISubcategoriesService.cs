namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using XeMart.Data.Models;

    public interface ISubcategoriesService
    {
        public Task CreateAsync<T>(T model);

        public IEnumerable<T> GetAll<T>();

        public IEnumerable<Subcategory> GetAll();

        public Task<bool> EditAsync<T>(T model);

        public Task<bool> DeleteAsync(int id);

        public T GetById<T>(int id);
    }
}
