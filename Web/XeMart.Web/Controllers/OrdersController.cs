namespace XeMart.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using XeMart.Services;
    using XeMart.Services.Data;
    using XeMart.Web.ViewModels.Addresses;
    using XeMart.Web.ViewModels.Orders;

    [Authorize]
    public class OrdersController : BaseController
    {
        private readonly IOrdersService ordersService;
        private readonly IAddressesService addressesService;
        private readonly ICountriesService countriesService;
        private readonly IShoppingCartService shoppingCartService;
        private readonly IStringService stringService;

        public OrdersController(
            IOrdersService ordersService,
            IAddressesService addressesService,
            ICountriesService countriesService,
            IShoppingCartService shoppingCartService,
            IStringService stringService)
        {
            this.ordersService = ordersService;
            this.addressesService = addressesService;
            this.countriesService = countriesService;
            this.shoppingCartService = shoppingCartService;
            this.stringService = stringService;
        }

        public async Task<IActionResult> Create()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var hasProducts = await this.shoppingCartService.AnyProducts(userId);
            if (!hasProducts)
            {
                this.TempData["Error"] = "The shopping cart is empty.";
                return this.RedirectToAction("Index", "Home");
            }

            var addresses = this.addressesService.GetAll<AddressViewModel>(userId);

            foreach (var address in addresses)
            {
                address.Description = this.stringService.TruncateAtWord(address.Description, 30);
            }

            var countries = this.countriesService.GetAll();

            var email = this.User.FindFirstValue(ClaimTypes.Email);

            var model = new CreateOrderInputViewModel
            {
                Addresses = addresses,
                Email = email,
                Countries = countries,
            };

            return this.View(model);
        }

        //[HttpPost]
        //public async Task<IActionResult> Create(CreateSubcategoryInputViewModel model)
        //{
        //    if (!this.ModelState.IsValid)
        //    {
        //        var mainCategories = this.mainCategoriesService.GetAll();
        //        model.MainCategories = mainCategories;
        //        return this.View(model);
        //    }

        //    await this.subcategoriesService.CreateAsync<CreateSubcategoryInputViewModel>(model, model.Image, SubcategoriesImagesDirectoryPath, this.webHostEnvironment.WebRootPath);

        //    this.TempData["Alert"] = "Successfully created subcategory.";

        //    return this.RedirectToAction(nameof(this.All));
        //}
    }
}
