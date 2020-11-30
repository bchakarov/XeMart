namespace XeMart.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;

    using XeMart.Services.Data;
    using XeMart.Web.ViewModels.Categories;

    public class MobileMenuViewComponent : ViewComponent
    {
        private readonly IMainCategoriesService mainCategoriesService;

        public MobileMenuViewComponent(IMainCategoriesService mainCategoriesService)
        {
            this.mainCategoriesService = mainCategoriesService;
        }

        public IViewComponentResult Invoke()
        {
            var categories = this.mainCategoriesService.GetAll<MainCategoriesWithSubcategoriesViewModel>();
            return this.View(categories);
        }
    }
}
