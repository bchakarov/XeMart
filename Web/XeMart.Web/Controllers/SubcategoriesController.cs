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
        public IActionResult Products(int subcategoryId, int pageNumber = 1, int productsPerPage = 12)
        {
            if (pageNumber <= 0)
            {
                return this.Products(subcategoryId);
            }

            if (productsPerPage <= 0)
            {
                return this.Products(subcategoryId);
            }

            var subcategoryNameAndProductCount = this.subcategoriesService.GetById<SubcategoryNameAndProductCountViewModel>(subcategoryId);
            if (subcategoryNameAndProductCount == null)
            {
                this.TempData["Error"] = "Subcategory not found.";
                return this.RedirectToAction("Index", "Home");
            }

            var products = this.productsService.TakeProductsBySubcategoryId<ProductViewModel>(subcategoryId, pageNumber, productsPerPage);

            foreach (var product in products)
            {
                product.Description = this.stringService.TruncateAtWord(product.Description, DescriptionMaxLength);
            }

            var subcategory = new SubcategoryProductsViewModel
            {
                SubcategoryId = subcategoryNameAndProductCount.Id,
                Name = subcategoryNameAndProductCount.Name,
                ItemsCount = subcategoryNameAndProductCount.ProductsCount,
                ItemsPerPage = productsPerPage,
                PageNumber = pageNumber,
                Products = products,
            };

            return this.View(subcategory);
        }
    }
}
