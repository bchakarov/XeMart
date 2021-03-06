﻿namespace XeMart.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;

    using XeMart.Common;
    using XeMart.Data.Common.Repositories;
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;
    using XeMart.Web.Infrastructure.ExtensionMethods;
    using XeMart.Web.ViewModels.Products;
    using XeMart.Web.ViewModels.ShoppingCart;

    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCartProduct> shoppingCartProductRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IProductsService productsService;

        public ShoppingCartService(
            IRepository<ShoppingCartProduct> shoppingCartProductRepository,
            UserManager<ApplicationUser> userManager,
            IProductsService productsService)
        {
            this.shoppingCartProductRepository = shoppingCartProductRepository;
            this.userManager = userManager;
            this.productsService = productsService;
        }

        public async Task<bool> AddProductAsync(bool isUserAuthenticated, ISession session, string userId, string productId, int quantity = 1)
        {
            if (isUserAuthenticated)
            {
                var user = await this.userManager.FindByIdAsync(userId);
                var shoppingCartId = user.ShoppingCartId;

                var shoppingCartExists = this.GetShoppingCartByIdAndProductId(productId, shoppingCartId) != null;

                if (shoppingCartExists)
                {
                    return false;
                }

                var productExists = this.productsService.HasProduct(productId);

                if (!productExists)
                {
                    return false;
                }

                var newShoppingCart = new ShoppingCartProduct
                {
                    ShoppingCartId = shoppingCartId,
                    ProductId = productId,
                    Quantity = quantity,
                };

                await this.shoppingCartProductRepository.AddAsync(newShoppingCart);
                await this.shoppingCartProductRepository.SaveChangesAsync();

                return true;
            }
            else
            {
                var shoppingCartSession = session.GetObjectFromJson<List<ShoppingCartProductViewModel>>(GlobalConstants.SessionShoppingCartKey);
                if (shoppingCartSession == null)
                {
                    shoppingCartSession = new List<ShoppingCartProductViewModel>();
                }

                if (shoppingCartSession.Any(x => x.ProductId == productId))
                {
                    return false;
                }

                var product = this.productsService.GetById<ProductSidebarViewModel>(productId);
                var shoppingCartProduct = new ShoppingCartProductViewModel
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ProductPrice = product.Price,
                    ImageUrl = product.ImageUrl,
                    AverageRating = product.AverageRating,
                    Quantity = quantity,
                };

                shoppingCartSession.Add(shoppingCartProduct);

                session.SetObjectAsJson(GlobalConstants.SessionShoppingCartKey, shoppingCartSession);

                return true;
            }
        }

        public async Task<IEnumerable<T>> GetAllProductsAsync<T>(bool isUserAuthenticated, ISession session, string userId)
        {
            if (isUserAuthenticated)
            {
                var user = await this.userManager.FindByIdAsync(userId);
                var shoppingCartId = user.ShoppingCartId;

                return this.shoppingCartProductRepository.AllAsNoTracking()
                    .Where(x => x.ShoppingCartId == shoppingCartId)
                    .To<T>().ToList();
            }
            else
            {
                var products = session.GetObjectFromJson<List<T>>(GlobalConstants.SessionShoppingCartKey);
                return products;
            }
        }

        public async Task<int> GetProductsCountAsync(bool isUserAuthenticated, ISession session, string userId)
        {
            if (isUserAuthenticated)
            {
                var user = await this.userManager.FindByIdAsync(userId);
                var shoppingCartId = user.ShoppingCartId;

                return this.shoppingCartProductRepository.AllAsNoTracking()
                    .Count(x => x.ShoppingCartId == shoppingCartId);
            }
            else
            {
                var products = session.GetObjectFromJson<List<ShoppingCartProductViewModel>>(GlobalConstants.SessionShoppingCartKey);
                return (products == null) ? 0 : products.Count;
            }
        }

        public async Task<bool> AnyProductsAsync(string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            var shoppingCartId = user.ShoppingCartId;

            return this.shoppingCartProductRepository.AllAsNoTracking()
                .Any(x => x.ShoppingCartId == shoppingCartId);
        }

        public async Task<bool> UpdateQuantityAsync(bool isUserAuthenticated, ISession session, string userId, string productId, bool increase)
        {
            if (isUserAuthenticated)
            {
                var user = await this.userManager.FindByIdAsync(userId);
                var shoppingCartId = user.ShoppingCartId;

                var shoppingCart = this.GetShoppingCartByIdAndProductId(productId, shoppingCartId);

                if (shoppingCart == null)
                {
                    return false;
                }

                var quantity = shoppingCart.Quantity;
                if (increase)
                {
                    quantity++;
                }
                else
                {
                    quantity = Math.Max(quantity - 1, 1);
                }

                shoppingCart.Quantity = quantity;

                this.shoppingCartProductRepository.Update(shoppingCart);
                await this.shoppingCartProductRepository.SaveChangesAsync();

                return true;
            }
            else
            {
                var shoppingCartSession = session.GetObjectFromJson<List<ShoppingCartProductViewModel>>(GlobalConstants.SessionShoppingCartKey);
                if (shoppingCartSession == null)
                {
                    return false;
                }

                var product = shoppingCartSession.FirstOrDefault(x => x.ProductId == productId);
                if (product == null)
                {
                    return false;
                }

                var quantity = product.Quantity;
                if (increase)
                {
                    quantity++;
                }
                else
                {
                    quantity = Math.Max(quantity - 1, 1);
                }

                product.Quantity = quantity;

                session.SetObjectAsJson(GlobalConstants.SessionShoppingCartKey, shoppingCartSession);

                return true;
            }
        }

        public async Task<bool> DeleteProductAsync(bool isUserAuthenticated, ISession session, string userId, string productId)
        {
            if (isUserAuthenticated)
            {
                var user = await this.userManager.FindByIdAsync(userId);
                var shoppingCartId = user.ShoppingCartId;

                var shoppingCart = this.GetShoppingCartByIdAndProductId(productId, shoppingCartId);

                if (shoppingCart == null)
                {
                    return false;
                }

                this.shoppingCartProductRepository.Delete(shoppingCart);
                await this.shoppingCartProductRepository.SaveChangesAsync();

                return true;
            }
            else
            {
                var shoppingCartSession = session.GetObjectFromJson<List<ShoppingCartProductViewModel>>(GlobalConstants.SessionShoppingCartKey);
                if (shoppingCartSession == null)
                {
                    return false;
                }

                var product = shoppingCartSession.FirstOrDefault(x => x.ProductId == productId);
                if (product == null)
                {
                    return false;
                }

                shoppingCartSession.Remove(product);

                session.SetObjectAsJson(GlobalConstants.SessionShoppingCartKey, shoppingCartSession);

                return true;
            }
        }

        public async Task DeleteAllProductsAsync(string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            var shoppingCartId = user.ShoppingCartId;

            var products = this.shoppingCartProductRepository.All()
                .Where(x => x.ShoppingCartId == shoppingCartId)
                .ToList();

            foreach (var product in products)
            {
                this.shoppingCartProductRepository.Delete(product);
            }

            await this.shoppingCartProductRepository.SaveChangesAsync();
        }

        private ShoppingCartProduct GetShoppingCartByIdAndProductId(string productId, string shoppingCartId) =>
            this.shoppingCartProductRepository.All()
            .FirstOrDefault(x => x.ShoppingCartId == shoppingCartId && x.ProductId == productId);
    }
}
