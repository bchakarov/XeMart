namespace XeMart.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using XeMart.Services.Data;
    using XeMart.Web.ViewModels.ShoppingCart;

    public class ShoppingCartController : BaseController
    {
        private readonly IShoppingCartService shoppingCartService;
        private readonly IProductsService productsService;

        private readonly string userId;
        private readonly bool isUserAuthenticated;
        private readonly ISession session;

        public ShoppingCartController(
            IShoppingCartService shoppingCartService,
            IProductsService productsService,
            IHttpContextAccessor contextAccessor)
        {
            this.shoppingCartService = shoppingCartService;
            this.productsService = productsService;

            this.userId = contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            this.isUserAuthenticated = contextAccessor.HttpContext.User.Identity.IsAuthenticated;
            this.session = contextAccessor.HttpContext.Session;
        }

        public async Task<IActionResult> Index()
        {
            var shoppingCartProducts = await this.shoppingCartService.GetAllProductsAsync<ShoppingCartProductViewModel>(this.isUserAuthenticated, this.session, this.userId);
            if (shoppingCartProducts == null || !shoppingCartProducts.Any())
            {
                return this.RedirectToAction("Index", "Home");
            }

            return this.View(new ShoppingCartViewModel() { Products = shoppingCartProducts });
        }

        [HttpGet("/ShoppingCart/Add/{productId}")]
        public async Task<IActionResult> Add(string productId)
        {
            var addResult = await this.shoppingCartService.AddProductAsync(this.isUserAuthenticated, this.session, this.userId, productId);
            if (addResult)
            {
                this.TempData["Alert"] = "Successfully added the product to the shopping cart.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem adding the product to the shopping cart.";
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpGet("/ShoppingCart/Delete/{productId}")]
        public async Task<IActionResult> Delete(string productId)
        {
            var deleteResult = await this.shoppingCartService.DeleteProductAsync(this.isUserAuthenticated, this.session, this.userId, productId);
            if (deleteResult)
            {
                this.TempData["Alert"] = "Successfully removed the product from the shopping cart.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem removing the product from the shopping cart.";
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpGet("/ShoppingCart/Quantity/{productId}")]
        public async Task<IActionResult> UpdateQuantity(string productId, bool increase)
        {
            var updateResult = await this.shoppingCartService.UpdateQuantityAsync(this.isUserAuthenticated, this.session, this.userId, productId, increase);
            if (updateResult)
            {
                this.TempData["Alert"] = "Successfully updated product quantity.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem updating the product quantity.";
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
