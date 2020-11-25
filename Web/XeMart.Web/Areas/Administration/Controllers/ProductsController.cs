namespace XeMart.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    using XeMart.Services.Data;
    using XeMart.Web.ViewModels.Administration.Products;

    public class ProductsController : AdministrationController
    {
        private const string ProductsDirectoryPath = "\\images\\products\\";

        private readonly ISubcategoriesService subcategoriesService;
        private readonly IProductsService productsService;
        private readonly IWebHostEnvironment webHostEnvironment;

        private readonly string fullDirectoryPath;

        public ProductsController(
            ISubcategoriesService subcategoriesService,
            IProductsService productsService,
            IWebHostEnvironment webHostEnvironment)
        {
            this.subcategoriesService = subcategoriesService;
            this.productsService = productsService;
            this.webHostEnvironment = webHostEnvironment;

            this.fullDirectoryPath = this.webHostEnvironment.WebRootPath + ProductsDirectoryPath;
        }

        public IActionResult Create()
        {
            var subcategories = this.subcategoriesService.GetAll();

            var model = new CreateProductInputViewModel
            {
                Subcategories = subcategories,
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductInputViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                var subcategories = this.subcategoriesService.GetAll();
                model.Subcategories = subcategories;
                return this.View(model);
            }

            await this.productsService.CreateAsync<CreateProductInputViewModel>(model, model.UploadedImages, this.fullDirectoryPath, this.webHostEnvironment.WebRootPath);

            this.TempData["Alert"] = "Successfully created product.";

            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult All()
        {
            var products = this.productsService.GetAll<ProductViewModel>();
            return this.View(products);
        }

        public IActionResult Deleted()
        {
            var products = this.productsService.GetAllDeleted<DeletedProductViewModel>();
            return this.View(products);
        }

        public IActionResult Edit(string id)
        {
            var subcategories = this.subcategoriesService.GetAll();
            var product = this.productsService.GetById<EditProductViewModel>(id);
            if (product == null)
            {
                this.TempData["Error"] = "Product not found.";
                return this.RedirectToAction(nameof(this.All));
            }

            product.Subcategories = subcategories;

            return this.View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProductViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var editResult = await this.productsService.EditAsync<EditProductViewModel>(model, model.UploadedImages, this.fullDirectoryPath, this.webHostEnvironment.WebRootPath);
            if (editResult)
            {
                this.TempData["Alert"] = "Successfully edited product.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem editing the product.";
            }

            return this.RedirectToAction(nameof(this.All));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var deleteResult = await this.productsService.DeleteAsync(id);
            if (deleteResult)
            {
                this.TempData["Alert"] = "Successfully deleted product.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem deleting the product.";
            }

            return this.RedirectToAction(nameof(this.All));
        }

        public async Task<IActionResult> Undelete(string id)
        {
            var undeleteResult = await this.productsService.UndeleteAsync(id);
            if (undeleteResult)
            {
                this.TempData["Alert"] = "Successfully undeleted product.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem undeleting the product.";
            }

            return this.RedirectToAction(nameof(this.Deleted));
        }

        public async Task<IActionResult> DeleteImage(string id)
        {
            var deleteResult = await this.productsService.DeleteImageAsync(id);
            if (deleteResult)
            {
                this.TempData["Alert"] = "Successfully deleted image.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem deleting the image.";
            }

            return this.RedirectToAction(nameof(this.All));
        }
    }
}
