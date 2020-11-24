namespace XeMart.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : AdministrationController
    {
        public DashboardController()
        {
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult StatusCodePage(int code)
        {
            this.ViewData["StatusCode"] = code;
            return this.View();
        }
    }
}
