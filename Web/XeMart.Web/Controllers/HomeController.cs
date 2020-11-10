namespace XeMart.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using XeMart.Data.Models;
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
            await this.userMessagesService.Add(new UserMessage
            {
                Subject = model.Subject,
                Email = model.Email,
                Message = model.Message,
                IP = ip,
            });

            return this.RedirectToAction(nameof(this.Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
