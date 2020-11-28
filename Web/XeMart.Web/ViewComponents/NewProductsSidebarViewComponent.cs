namespace XeMart.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;

    using XeMart.Services.Data;
    using XeMart.Web.ViewModels.Products;

    public class NewProductsSidebarViewComponent : ViewComponent
    {
        private readonly IProductsService productsService;

        public NewProductsSidebarViewComponent(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        public IViewComponentResult Invoke(int subcategoryId, int productsToTake)
        {
            var products = this.productsService.GetNewestBySubcategoryId<ProductSidebarViewModel>(subcategoryId, productsToTake);
            return this.View(products);
        }
    }
}
