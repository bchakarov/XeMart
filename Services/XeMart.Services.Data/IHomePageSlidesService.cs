namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface IHomePageSlidesService
    {
        public Task CreateAsync<T>(T model, IFormFile image);

        public IEnumerable<T> GetAll<T>();

        public T GetById<T>(int id);

        public Task<bool> EditAsync<T>(T model, IFormFile image);

        public Task<bool> MoveUpAsync(int id);

        public Task<bool> MoveDownAsync(int id);

        public Task<bool> DeleteAsync(int id);
    }
}
