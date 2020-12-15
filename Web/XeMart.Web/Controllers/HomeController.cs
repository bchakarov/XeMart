namespace XeMart.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Mvc;

    using XeMart.Services;
    using XeMart.Services.Data;
    using XeMart.Web.ViewModels;
    using XeMart.Web.ViewModels.Home;
    using XeMart.Web.ViewModels.HomePageSlides;
    using XeMart.Web.ViewModels.Products;

    public class HomeController : BaseController
    {
        private readonly IUserMessagesService userMessagesService;
        private readonly IOrdersService ordersService;
        private readonly IProductsService productsService;
        private readonly IHomePageSlidesService homePageSlidesService;
        private readonly IStringService stringService;

        public HomeController(
            IUserMessagesService userMessagesService,
            IOrdersService ordersService,
            IProductsService productsService,
            IHomePageSlidesService homePageSlidesService,
            IStringService stringService)
        {
            this.userMessagesService = userMessagesService;
            this.ordersService = ordersService;
            this.productsService = productsService;
            this.homePageSlidesService = homePageSlidesService;
            this.stringService = stringService;
        }

        public IActionResult Index()
        {
            var mostBoughtProducts = this.ordersService.GetMostBoughtProducts<ProductSidebarViewModel>(10);
            var newestProducts = this.productsService.GetNewest<ProductViewModel>(10);
            var topRatedProducts = this.productsService.GetTopRated<ProductSidebarViewModel>(4);

            var slides = this.homePageSlidesService.GetAll<SlideHomeViewModel>();

            foreach (var product in topRatedProducts)
            {
                product.Name = this.stringService.TruncateAtWord(product.Name, 30);
            }

            var viewModel = new IndexViewModel
            {
                MostBoughtProducts = mostBoughtProducts,
                NewestProducts = newestProducts,
                TopRatedProducts = topRatedProducts,
                Slides = slides,
            };

            return this.View(viewModel);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        public IActionResult Contact()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                ContactFormInputViewModel model = new ContactFormInputViewModel
                {
                    Email = this.User.Identity.Name,
                };

                return this.View(model);
            }

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactFormInputViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            this.TempData["Alert"] = "Thank you! Your request was sent successfully!";

            var ip = this.HttpContext.Connection.RemoteIpAddress.ToString();
            await this.userMessagesService.CreateAsync<ContactFormInputViewModel>(model, ip);

            return this.RedirectToAction(nameof(this.Index));
        }

        public IActionResult StatusCodePage(int code)
        {
            this.ViewData["StatusCode"] = code;
            var statusCodeFeature = this.HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            if (statusCodeFeature.OriginalPath.Contains("/Administration/"))
            {
                return this.RedirectToAction("StatusCodePage", "Dashboard", new { code, Area = "Administration" });
            }
            else
            {
                return this.View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
