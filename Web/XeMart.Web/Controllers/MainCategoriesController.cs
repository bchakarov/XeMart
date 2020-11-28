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

        [HttpGet("/maincategories/{mainCategoryId}")]
        public IActionResult Subcategories(int mainCategoryId)
        {
            var mainCategoryModel = this.mainCategoriesService.GetById<MainCategorySubcategoriesViewModel>(mainCategoryId);
            return this.View(mainCategoryModel);
        }
    }
}
