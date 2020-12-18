namespace XeMart.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using XeMart.Data.Models.Enums;
    using XeMart.Services;
    using XeMart.Services.Data;
    using XeMart.Web.ViewModels.Addresses;
    using XeMart.Web.ViewModels.Administration.Suppliers;
    using XeMart.Web.ViewModels.Orders;

    [Authorize]
    public class OrdersController : BaseController
    {
        private const string ShoppingCartIsEmptyMessage = "The shopping cart is empty.";

        private readonly IOrdersService ordersService;
        private readonly ISuppliersService suppliersService;
        private readonly IAddressesService addressesService;
        private readonly ICountriesService countriesService;
        private readonly IShoppingCartService shoppingCartService;
        private readonly IStringService stringService;

        private readonly string userId;

        public OrdersController(
            IOrdersService ordersService,
            ISuppliersService suppliersService,
            IAddressesService addressesService,
            ICountriesService countriesService,
            IShoppingCartService shoppingCartService,
            IStringService stringService,
            IHttpContextAccessor contextAccessor)
        {
            this.ordersService = ordersService;
            this.suppliersService = suppliersService;
            this.addressesService = addressesService;
            this.countriesService = countriesService;
            this.shoppingCartService = shoppingCartService;
            this.stringService = stringService;

            this.userId = contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public async Task<IActionResult> Create()
        {
            var hasProducts = await this.shoppingCartService.AnyProductsAsync(this.userId);
            if (!hasProducts)
            {
                this.TempData["Error"] = ShoppingCartIsEmptyMessage;
                return this.RedirectToAction("Index", "Home");
            }

            var suppliers = this.suppliersService.GetAll<SupplierViewModel>();

            var addresses = this.addressesService.GetAll<AddressViewModel>(this.userId);

            foreach (var address in addresses)
            {
                address.Description = this.stringService.TruncateAtWord(address.Description, 30);
            }

            var countries = this.countriesService.GetAll();

            var email = this.User.Identity.Name;

            var model = new CreateOrderInputViewModel
            {
                Suppliers = suppliers,
                Addresses = addresses,
                Email = email,
                Countries = countries,
            };

            // cancel any processing orders
            await this.ordersService.CancelAnyProcessingOrders(this.userId);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderInputViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                var hasProducts = await this.shoppingCartService.AnyProductsAsync(this.userId);
                if (!hasProducts)
                {
                    this.TempData["Error"] = ShoppingCartIsEmptyMessage;
                    return this.RedirectToAction("Index", "Home");
                }

                var suppliers = this.suppliersService.GetAll<SupplierViewModel>();

                var addresses = this.addressesService.GetAll<AddressViewModel>(this.userId);

                foreach (var address in addresses)
                {
                    address.Description = this.stringService.TruncateAtWord(address.Description, 30);
                }

                var countries = this.countriesService.GetAll();

                var email = this.User.Identity.Name;

                model.Suppliers = suppliers;
                model.Addresses = addresses;
                model.Email = email;
                model.Countries = countries;

                return this.View(model);
            }

            await this.ordersService.CreateAsync<CreateOrderInputViewModel>(model, this.userId);

            return this.RedirectToAction(nameof(this.Complete));
        }

        public async Task<IActionResult> Complete()
        {
            var hasProducts = await this.shoppingCartService.AnyProductsAsync(this.userId);
            if (!hasProducts)
            {
                this.TempData["Error"] = ShoppingCartIsEmptyMessage;
                return this.RedirectToAction("Index", "Home");
            }

            var orderId = await this.ordersService.CompleteOrderAsync(this.userId);

            if (this.ordersService.GetPaymentTypeById(orderId) == PaymentType.CashOnDelivery)
            {
                this.TempData["Alert"] = "Successfully registered order.";
            }

            var order = this.ordersService.GetById<OrderPaymentStatusViewModel>(orderId);

            return this.View(order);
        }

        [HttpGet("/Orders/History/{pageNumber?}")]
        public IActionResult History(int pageNumber = 1)
        {
            if (pageNumber <= 0)
            {
                return this.History();
            }

            var itemsPerPage = 6;
            var orders = this.ordersService.TakeOrdersByUserId<OrderSummaryViewModel>(this.userId, pageNumber, itemsPerPage);
            var ordersCount = this.ordersService.GetOrdersCountByUserId(this.userId);

            var viewModel = new OrderListViewModel
            {
                ItemsCount = ordersCount,
                ItemsPerPage = itemsPerPage,
                PageNumber = pageNumber,
                Orders = orders,
                Area = string.Empty,
                Controller = "Orders",
                Action = nameof(this.History),
            };

            return this.View(viewModel);
        }

        public IActionResult Details(string id)
        {
            if (!this.ordersService.UserHasOrder(this.userId, id))
            {
                this.TempData["Error"] = "Order not found.";
                return this.RedirectToAction("Index", "Home");
            }

            this.ViewData["OrderId"] = id;
            return this.View();
        }
    }
}
