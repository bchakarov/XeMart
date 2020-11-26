namespace XeMart.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using XeMart.Services.Data;
    using XeMart.Web.ViewModels.Products;

    public class ProductsController : BaseController
    {
        private readonly IProductsService productsService;

        public ProductsController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        public IActionResult Details(string id)
        {
            var product = this.productsService.GetById<ProductDetailsViewModel>(id);
            return this.View(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview(ProductReviewInputViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.Details), new { id = model.ProductId });
            }

            var createResult = await this.productsService.CreateReviewAsync<ProductReviewInputViewModel>(model);
            if (createResult)
            {
                this.TempData["Alert"] = "Successfully added product review.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem adding the product review.";
            }

            return this.RedirectToAction(nameof(this.Details), new { id = model.ProductId });
        }
    }
}
