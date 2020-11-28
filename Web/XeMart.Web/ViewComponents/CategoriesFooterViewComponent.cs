namespace XeMart.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;

    using XeMart.Services.Data;
    using XeMart.Web.ViewModels.Categories;

    public class CategoriesFooterViewComponent : ViewComponent
    {
        private readonly IMainCategoriesService mainCategoriesService;

        public CategoriesFooterViewComponent(IMainCategoriesService mainCategoriesService)
        {
            this.mainCategoriesService = mainCategoriesService;
        }

        public IViewComponentResult Invoke()
        {
            var categories = this.mainCategoriesService.GetAll<MainCategoryNameViewModel>();
            return this.View(categories);
        }
    }
}
