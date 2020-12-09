namespace XeMart.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;

    using XeMart.Services.Data;
    using XeMart.Web.ViewModels.Categories;
    using XeMart.Web.ViewModels.Search;

    public class SearchViewComponent : ViewComponent
    {
        private readonly IMainCategoriesService mainCategoriesService;

        public SearchViewComponent(IMainCategoriesService mainCategoriesService)
        {
            this.mainCategoriesService = mainCategoriesService;
        }

        public IViewComponentResult Invoke()
        {
            var categories = this.mainCategoriesService.GetAll<MainCategoryNameViewModel>();
            var viewModel = new SearchInputViewModel
            {
                MainCategories = categories,
            };
            return this.View(viewModel);
        }
    }
}
