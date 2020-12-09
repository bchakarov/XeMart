namespace XeMart.Web.Controllers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Text.RegularExpressions;

    using Microsoft.AspNetCore.Mvc;

    using XeMart.Services;
    using XeMart.Services.Data;
    using XeMart.Web.ViewModels.Products;
    using XeMart.Web.ViewModels.Search;

    public class SearchController : BaseController
    {
        private const int DescriptionMaxLength = 100;
        private readonly List<int> itemsPerPageValues = new List<int> { 6, 12, 18, 24 };
        private readonly List<string> sortingValues = new List<string> { "Price asc", "Price desc", "Newest", "Oldest" };

        private readonly ISubcategoriesService subcategoriesService;
        private readonly IProductsService productsService;
        private readonly IStringService stringService;

        public SearchController(
            ISubcategoriesService subcategoriesService,
            IProductsService productsService,
            IStringService stringService)
        {
            this.subcategoriesService = subcategoriesService;
            this.productsService = productsService;
            this.stringService = stringService;
        }

        public IActionResult Index(string searchTerm, int? mainCategoryId = null, int pageNumber = 1, int itemsPerPage = 6, string sorting = "price asc")
        {
            if (pageNumber <= 0)
            {
                this.TempData["Error"] = "Page number cannot be negative.";
                return this.RedirectToAction("Index", "Home");
            }

            if (itemsPerPage <= 0)
            {
                this.TempData["Error"] = "Items per page cannot be negative.";
                return this.RedirectToAction("Index", "Home");
            }

            var products = this.productsService.TakeProductsBySearchStringAndMainCategoryId<ProductViewModel>(searchTerm, mainCategoryId, pageNumber, itemsPerPage, sorting);

            foreach (var product in products)
            {
                if (!string.IsNullOrEmpty(product.Description))
                {
                    var descriptionText = WebUtility.HtmlDecode(Regex.Replace(product.Description, @"<[^>]+>", string.Empty));
                    product.Description = this.stringService.TruncateAtWord(descriptionText, DescriptionMaxLength);
                }
            }

            var searchViewModel = new SearchProductsViewModel
            {
                ItemsCount = this.productsService.GetProductsCountBySearchStringAndMainCategoryId(searchTerm, mainCategoryId),
                ItemsPerPage = itemsPerPage,
                PageNumber = pageNumber,
                Products = products,
                SortingValues = this.sortingValues,
                Sorting = sorting,
                SearchTerm = searchTerm,
                MainCategoryId = mainCategoryId,
                ItemsPerPageValues = this.itemsPerPageValues,
                Area = string.Empty,
                Controller = "Search",
                Action = nameof(this.Index),
            };

            return this.View(searchViewModel);
        }
    }
}
