namespace XeMart.Web.Controllers
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Mvc;

    using XeMart.Web.ViewModels;
    using XeMart.Web.ViewModels.Home;

    public class HomeController : BaseController
    {
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
        public IActionResult Contact(ContactFormInputViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            this.TempData["Alert"] = "Thank you! Your request was sent successfully!";

            // this.userMessagesService.Create(model.Title, model.Email, model.Content);

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
