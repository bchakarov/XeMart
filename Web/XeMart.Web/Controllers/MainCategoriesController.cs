namespace XeMart.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using XeMart.Services.Data;
    using XeMart.Web.ViewModels.Categories;

    public class MainCategoriesController : BaseController
    {
        private readonly IMainCategoriesService mainCategoriesService;

        public MainCategoriesController(IMainCategoriesService mainCategoriesService)
        {
            this.mainCategoriesService = mainCategoriesService;
        }

        [HttpGet("/Maincategories/{mainCategoryId}")]
        public IActionResult Subcategories(int mainCategoryId)
        {
            var mainCategoryModel = this.mainCategoriesService.GetById<MainCategorySubcategoriesViewModel>(mainCategoryId);
            if (mainCategoryModel == null)
            {
                this.TempData["Error"] = "Main category not found.";
                return this.RedirectToAction("Index", "Home");
            }

            return this.View(mainCategoryModel);
        }
    }
}
