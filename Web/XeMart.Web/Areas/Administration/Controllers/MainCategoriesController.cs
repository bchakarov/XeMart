namespace XeMart.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using XeMart.Services.Data;
    using XeMart.Web.ViewModels.Administration.MainCategories;

    public class MainCategoriesController : AdministrationController
    {
        private readonly IMainCategoriesService mainCategoriesService;

        public MainCategoriesController(IMainCategoriesService mainCategoriesService)
        {
            this.mainCategoriesService = mainCategoriesService;
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

            await this.mainCategoriesService.CreateAsync<CreateMainCategoryInputViewModel>(model);

            this.TempData["Alert"] = "Successfully created main category.";

            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult All()
        {
            var mainCategories = this.mainCategoriesService.GetAll<MainCategoryViewModel>();
            return this.View(mainCategories);
        }

        public IActionResult Deleted()
        {
            var mainCategories = this.mainCategoriesService.GetAllDeleted<DeletedMainCategoryViewModel>();
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

            var editResult = await this.mainCategoriesService.EditAsync<EditMainCategoryViewModel>(model);
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

        public async Task<IActionResult> Undelete(int id)
        {
            var undeleteResult = await this.mainCategoriesService.UndeleteAsync(id);
            if (undeleteResult)
            {
                this.TempData["Alert"] = "Successfully undeleted main category.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem undeleting the main category.";
            }

            return this.RedirectToAction(nameof(this.Deleted));
        }
    }
}
