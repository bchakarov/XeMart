namespace XeMart.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;

    using XeMart.Services.Data;
    using XeMart.Web.ViewModels.Categories;

    public class CategoriesSidebarViewComponent : ViewComponent
    {
        private readonly IMainCategoriesService mainCategoriesService;

        public CategoriesSidebarViewComponent(IMainCategoriesService mainCategoriesService)
        {
            this.mainCategoriesService = mainCategoriesService;
        }

        public IViewComponentResult Invoke()
        {
            var categories = this.mainCategoriesService.GetAll<CategoriesSidebarViewModel>();
            return this.View(categories);
        }
    }
}
