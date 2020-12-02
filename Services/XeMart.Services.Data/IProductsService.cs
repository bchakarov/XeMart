namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface IProductsService
    {
        public Task CreateAsync<T>(T model, IEnumerable<IFormFile> images, string fullDirectoryPath, string webRootPath);

        public Task<bool> CreateReviewAsync<T>(T model);

        public IEnumerable<T> GetAll<T>();

        public IEnumerable<T> TakeProductsBySubcategoryId<T>(int subcategoryId, int page, int productsToTake, string columnName, bool isAscending);

        public IEnumerable<T> GetAllDeleted<T>();

        public IEnumerable<T> GetNewestBySubcategoryId<T>(int subcategoryId, int productsToTake);

        public Task<bool> EditAsync<T>(T model, IEnumerable<IFormFile> images, string fullDirectoryPath, string webRootPath);

        public Task<bool> DeleteAsync(string id);

        public Task<bool> UndeleteAsync(string id);

        public Task<bool> DeleteImageAsync(string id);

        public Task<bool> DeleteReviewAsync(string id);

        public T GetById<T>(string id);

        public bool HasProduct(string id);
    }
}
