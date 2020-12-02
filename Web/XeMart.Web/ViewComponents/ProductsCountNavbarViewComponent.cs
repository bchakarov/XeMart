namespace XeMart.Web.ViewComponents
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using XeMart.Services.Data;
    using XeMart.Web.ViewModels.FavouritesAndShoppingCart;

    public class ProductsCountNavbarViewComponent : ViewComponent
    {
        private readonly IFavouritesService favouritesService;
        private readonly IShoppingCartService shoppingCartService;

        public ProductsCountNavbarViewComponent(
            IFavouritesService favouritesService,
            IShoppingCartService shoppingCartService)
        {
            this.favouritesService = favouritesService;
            this.shoppingCartService = shoppingCartService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = this.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var favouritesCount = this.favouritesService.GetCount(userId);
            var shoppingCartProductsCount = await this.shoppingCartService.GetProductsCount(this.User.Identity.IsAuthenticated, this.HttpContext.Session, userId);

            var viewModel = new FavouritesAndShoppingCartNavbarViewModel
            {
                FavouriteProductsCount = favouritesCount,
                ShoppingCartProductsCount = shoppingCartProductsCount,
            };

            return this.View(viewModel);
        }
    }
}
