namespace XeMart.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using XeMart.Services.Data;
    using XeMart.Services.Mapping;
    using XeMart.Web.ViewModels.Categories;
    using XeMart.Web.ViewModels.Products;

    public class SubcategoriesController : BaseController
    {
        private readonly ISubcategoriesService subcategoriesService;
        private readonly IProductsService productsService;

        public SubcategoriesController(
            ISubcategoriesService subcategoriesService,
            IProductsService productsService)
        {
            this.subcategoriesService = subcategoriesService;
            this.productsService = productsService;
        }

        [HttpGet("/subcategories/{subcategoryId}")]
        public IActionResult Products(int subcategoryId, int page = 1, int productsPerPage = 12)
        {
            if (page <= 0)
            {
                return this.NotFound();
            }

            if (productsPerPage <= 0)
            {
                return this.NotFound();
            }

            var subcategoryNameAndProductCount = this.subcategoriesService.GetById<SubcategoryNameAndProductCountViewModel>(subcategoryId);

            var subcategory = new SubcategoryProductsViewModel
            {
                Name = subcategoryNameAndProductCount.Name,
                ItemsCount = subcategoryNameAndProductCount.ProductsCount,
                ItemsPerPage = productsPerPage,
                PageNumber = page,
                Products = this.productsService.TakeProductsBySubcategoryId<ProductViewModel>(subcategoryId, page, productsPerPage),
            };

            return this.View(subcategory);
        }
    }
}
