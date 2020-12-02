namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using XeMart.Data.Common.Repositories;
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class FavouritesService : IFavouritesService
    {
        private readonly IRepository<UserFavouriteProduct> favouritesRepository;
        private readonly IDeletableEntityRepository<Product> productsRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public FavouritesService(
            IRepository<UserFavouriteProduct> favouritesRepository,
            IDeletableEntityRepository<Product> productsRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.favouritesRepository = favouritesRepository;
            this.productsRepository = productsRepository;
            this.userManager = userManager;
        }

        public async Task<bool> AddAsync(string productId, string userId)
        {
            var product = this.GetProductById(productId);
            if (product == null)
            {
                return false;
            }

            var user = await this.userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            await this.favouritesRepository.AddAsync(new UserFavouriteProduct()
            {
                ProductId = product.Id,
                UserId = user.Id,
            });

            await this.favouritesRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(string productId, string userId)
        {
            var product = this.GetProductById(productId);
            if (product == null)
            {
                return false;
            }

            var user = await this.userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var favouriteProduct = this.GetFavouriteProductById(productId, userId);
            if (favouriteProduct == null)
            {
                return false;
            }

            this.favouritesRepository.Delete(favouriteProduct);
            await this.favouritesRepository.SaveChangesAsync();

            return true;
        }

        public IEnumerable<T> GetAll<T>(string userId) =>
            this.favouritesRepository.AllAsNoTracking()
            .Where(x => x.UserId == userId)
            .To<T>().ToList();

        public int GetCount(string userId) =>
            this.favouritesRepository.AllAsNoTracking()
            .Count(x => x.UserId == userId);

        private Product GetProductById(string id) =>
            this.productsRepository.AllAsNoTracking()
            .FirstOrDefault(x => x.Id == id);

        private UserFavouriteProduct GetFavouriteProductById(string productId, string userId) =>
            this.favouritesRepository.AllAsNoTracking()
            .FirstOrDefault(x => x.ProductId == productId && x.UserId == userId);
    }
}
