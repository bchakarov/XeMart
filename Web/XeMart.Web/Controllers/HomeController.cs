namespace XeMart.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Mvc;

    using XeMart.Services.Data;
    using XeMart.Web.ViewModels;
    using XeMart.Web.ViewModels.Home;

    public class HomeController : BaseController
    {
        private readonly IUserMessagesService userMessagesService;

        public HomeController(IUserMessagesService userMessagesService)
        {
            this.userMessagesService = userMessagesService;
        }

        public IActionResult Index()
        {
            return this.View();
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
