namespace XeMart.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    using XeMart.Services.Data;
    using XeMart.Web.ViewModels.Administration.MainCategories;

    public class MainCategoriesController : AdministrationController
    {
        private const string MainCategoriesDirectoryPath = "\\images\\categories\\";

        private readonly IMainCategoriesService mainCategoriesService;
        private readonly IWebHostEnvironment webHostEnvironment;

        private readonly string path;

        public MainCategoriesController(
            IMainCategoriesService mainCategoriesService,
            IWebHostEnvironment webHostEnvironment)
        {
            this.mainCategoriesService = mainCategoriesService;
            this.webHostEnvironment = webHostEnvironment;

            this.path = this.webHostEnvironment.WebRootPath + MainCategoriesDirectoryPath;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMainCategoryInputViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.mainCategoriesService.CreateAsync<CreateMainCategoryInputViewModel>(model, model.Image, this.path, this.webHostEnvironment.WebRootPath);

            this.TempData["Alert"] = "Successfully created main category.";

            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult All()
        {
            var mainCategories = this.mainCategoriesService.GetAll<MainCategoryViewModel>();
            return this.View(mainCategories);
        }

        public IActionResult Edit(int id)
        {
            var mainCategory = this.mainCategoriesService.GetById<EditMainCategoryViewModel>(id);
            if (mainCategory == null)
            {
                this.TempData["Error"] = "Main category not found.";
                return this.RedirectToAction(nameof(this.All));
            }

            return this.View(mainCategory);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditMainCategoryViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var editResult = await this.mainCategoriesService.EditAsync<EditMainCategoryViewModel>(model, model.Image, this.path, this.webHostEnvironment.WebRootPath);
            if (editResult)
            {
                this.TempData["Alert"] = "Successfully edited main category.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem editing the main category.";
            }

            return this.RedirectToAction(nameof(this.All));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var deleteResult = await this.mainCategoriesService.DeleteAsync(id);
            if (deleteResult)
            {
                this.TempData["Alert"] = "Successfully deleted main category.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem deleting the main category.";
            }

            return this.RedirectToAction(nameof(this.All));
        }
    }
}
