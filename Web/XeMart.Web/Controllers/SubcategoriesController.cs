namespace XeMart.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using XeMart.Services;
    using XeMart.Services.Data;
    using XeMart.Web.ViewModels.Categories;
    using XeMart.Web.ViewModels.Products;

    public class SubcategoriesController : BaseController
    {
        private const int DescriptionMaxLength = 200;

        private readonly ISubcategoriesService subcategoriesService;
        private readonly IProductsService productsService;
        private readonly IStringService stringService;

        public SubcategoriesController(
            ISubcategoriesService subcategoriesService,
            IProductsService productsService,
            IStringService stringService)
        {
            this.subcategoriesService = subcategoriesService;
            this.productsService = productsService;
            this.stringService = stringService;
        }

        [HttpGet("/Subcategories/{subcategoryId}")]
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

            var products = this.productsService.TakeProductsBySubcategoryId<ProductViewModel>(subcategoryId, page, productsPerPage);

            foreach (var product in products)
            {
                product.Description = this.stringService.TruncateAtWord(product.Description, DescriptionMaxLength);
            }

            var subcategory = new SubcategoryProductsViewModel
            {
                Name = subcategoryNameAndProductCount.Name,
                ItemsCount = subcategoryNameAndProductCount.ProductsCount,
                ItemsPerPage = productsPerPage,
                PageNumber = page,
                Products = products,
            };

            return this.View(subcategory);
        }
    }
}
