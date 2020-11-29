namespace XeMart.Web.Controllers
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;

    using XeMart.Services;
    using XeMart.Services.Data;
    using XeMart.Web.ViewModels.Categories;
    using XeMart.Web.ViewModels.Products;

    public class SubcategoriesController : BaseController
    {
        private const int DescriptionMaxLength = 200;
        private readonly List<int> itemsPerPageValues = new List<int> { 6, 12, 18, 24 };

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
        public IActionResult Products(int subcategoryId, int pageNumber = 1, int itemsPerPage = 6)
        {
            if (pageNumber <= 0)
            {
                return this.Products(subcategoryId);
            }

            if (itemsPerPage <= 0)
            {
                return this.Products(subcategoryId);
            }

            var subcategoryNameAndProductCount = this.subcategoriesService.GetById<SubcategoryNameAndProductCountViewModel>(subcategoryId);
            if (subcategoryNameAndProductCount == null)
            {
                this.TempData["Error"] = "Subcategory not found.";
                return this.RedirectToAction("Index", "Home");
            }

            var products = this.productsService.TakeProductsBySubcategoryId<ProductViewModel>(subcategoryId, pageNumber, itemsPerPage);

            foreach (var product in products)
            {
                product.Description = this.stringService.TruncateAtWord(product.Description, DescriptionMaxLength);
            }

            var subcategory = new SubcategoryProductsViewModel
            {
                SubcategoryId = subcategoryNameAndProductCount.Id,
                Name = subcategoryNameAndProductCount.Name,
                ItemsCount = subcategoryNameAndProductCount.ProductsCount,
                ItemsPerPage = itemsPerPage,
                PageNumber = pageNumber,
                Products = products,
                ItemsPerPageValues = this.itemsPerPageValues,
            };

            return this.View(subcategory);
        }
    }
}
