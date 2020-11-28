namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using XeMart.Data.Models;

    public interface IMainCategoriesService
    {
        public Task CreateAsync<T>(T model);

        public IEnumerable<T> GetAll<T>();

        public IEnumerable<MainCategory> GetAll();

        public IEnumerable<T> GetAllDeleted<T>();

        public Task<bool> EditAsync<T>(T model);

        public Task<bool> DeleteAsync(int id);

        public Task<bool> UndeleteAsync(int id);

        public T GetById<T>(int id);
    }
}
