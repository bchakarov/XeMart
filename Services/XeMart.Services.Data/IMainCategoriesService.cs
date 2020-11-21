namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    using XeMart.Data.Models;

    public interface IMainCategoriesService
    {
        public Task CreateAsync<T>(T model, IFormFile image, string imagePath, string webRootPath);

        public IEnumerable<T> All<T>();

        public IEnumerable<MainCategory> All();

        public Task<bool> EditAsync<T>(T model, IFormFile image, string imagePath, string webRootPath);

        public Task<bool> DeleteAsync(int id);

        public T GetById<T>(int id);
    }
}
