namespace XeMart.Web.ViewComponents
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using XeMart.Services;
    using XeMart.Services.Data;
    using XeMart.Web.ViewModels.ShoppingCart;

    public class CartBodyViewComponent : ViewComponent
    {
        private readonly IShoppingCartService shoppingCartService;
        private readonly IStringService stringService;

        public CartBodyViewComponent(
            IShoppingCartService shoppingCartService,
            IStringService stringService)
        {
            this.shoppingCartService = shoppingCartService;
            this.stringService = stringService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = this.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var products = await this.shoppingCartService.GetAllProducts<ShoppingCartProductViewModel>(this.User.Identity.IsAuthenticated, this.HttpContext.Session, userId);

            if (products != null)
            {
                foreach (var product in products)
                {
                    product.ProductName = this.stringService.TruncateAtWord(product.ProductName, 15);
                }
            }

            var viewModel = new ShoppingCartViewModel
            {
                Products = products ?? new List<ShoppingCartProductViewModel>(),
            };

            return this.View(viewModel);
        }
    }
}
