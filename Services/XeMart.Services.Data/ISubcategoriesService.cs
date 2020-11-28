namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    using XeMart.Data.Models;

    public interface ISubcategoriesService
    {
        public Task CreateAsync<T>(T model, IFormFile image, string directoryPath, string webRootPath);

        public IEnumerable<T> GetAll<T>();

        public IEnumerable<Subcategory> GetAll();

        public IEnumerable<T> GetAllDeleted<T>();

        public Task<bool> EditAsync<T>(T model, IFormFile image, string directoryPath, string webRootPath);

        public Task<bool> DeleteAsync(int id);

        public Task<bool> UndeleteAsync(int id);

        public T GetById<T>(int id);
    }
}
