namespace XeMart.Web.Controllers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Text.RegularExpressions;

    using Microsoft.AspNetCore.Mvc;

    using XeMart.Services;
    using XeMart.Services.Data;
    using XeMart.Web.ViewModels.Categories;
    using XeMart.Web.ViewModels.Products;

    public class SubcategoriesController : BaseController
    {
        private const int DescriptionMaxLength = 100;
        private readonly List<int> itemsPerPageValues = new List<int> { 6, 12, 18, 24 };
        private readonly List<string> sortingValues = new List<string> { "Price asc", "Price desc", "Newest", "Oldest" };

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
        public IActionResult Products(int subcategoryId, int pageNumber = 1, int itemsPerPage = 6, string sorting = "price asc")
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

            var products = this.productsService.TakeProductsBySubcategoryId<ProductViewModel>(subcategoryId, pageNumber, itemsPerPage, sorting);

            foreach (var product in products)
            {
                if (!string.IsNullOrEmpty(product.Description))
                {
                    var descriptionText = WebUtility.HtmlDecode(Regex.Replace(product.Description, @"<[^>]+>", string.Empty));
                    product.Description = this.stringService.TruncateAtWord(descriptionText, DescriptionMaxLength);
                }
            }

            var subcategory = new SubcategoryProductsViewModel
            {
                Id = subcategoryNameAndProductCount.Id,
                Name = subcategoryNameAndProductCount.Name,
                ItemsCount = subcategoryNameAndProductCount.ProductsCount,
                ItemsPerPage = itemsPerPage,
                PageNumber = pageNumber,
                Products = products,
                ItemsPerPageValues = this.itemsPerPageValues,
                Sorting = sorting,
                SortingValues = this.sortingValues,
                Area = string.Empty,
                Controller = "Subcategories",
                Action = nameof(this.Products),
            };

            return this.View(subcategory);
        }
    }
}
