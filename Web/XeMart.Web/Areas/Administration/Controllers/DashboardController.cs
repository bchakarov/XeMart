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
            // var viewModel = new IndexViewModel();
            return this.View();
        }
    }
}
