namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IFavouritesService
    {
        public Task<bool> AddAsync(string productId, string userId);

        public Task<bool> DeleteAsync(string productId, string userId);

        public IEnumerable<T> GetAll<T>(string userId);

        public int GetCount(string userId);
    }
}
