namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface IShoppingCartService
    {
        public Task<bool> AddProductAsync(bool isUserAuthenticated, ISession session, string userId, string productId, int quantity = 1);

        public Task<IEnumerable<T>> GetAllProducts<T>(bool isUserAuthenticated, ISession session, string userId);

        public Task<int> GetProductsCount(bool isUserAuthenticated, ISession session, string userId);

        public Task<bool> AnyProducts(string userId);

        public Task<bool> UpdateQuantityAsync(bool isUserAuthenticated, ISession session, string userId, string productId, bool increase);

        public Task<bool> DeleteProductAsync(bool isUserAuthenticated, ISession session, string userId, string productId);
    }
}
