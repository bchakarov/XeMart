namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    using XeMart.Data.Models;

    public interface IMainCategoriesService
    {
        public Task CreateAsync<T>(T model, IFormFile image, string fullDirectoryPath, string webRootPath);

        public IEnumerable<T> GetAll<T>();

        public IEnumerable<MainCategory> GetAll();

        public IEnumerable<T> GetAllDeleted<T>();

        public Task<bool> EditAsync<T>(T model, IFormFile image, string fullDirectoryPath, string webRootPath);

        public Task<bool> DeleteAsync(int id);

        public Task<bool> UndeleteAsync(int id);

        public T GetById<T>(int id);
    }
}
