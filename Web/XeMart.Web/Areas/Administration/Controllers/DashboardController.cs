﻿namespace XeMart.Web.Areas.Administration.Controllers
{
    using XeMart.Web.ViewModels.Administration.Dashboard;

    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : AdministrationController
    {
        public DashboardController()
        {
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel();
            return this.View(viewModel);
        }
    }
}
