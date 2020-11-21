namespace XeMart.Web.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using XeMart.Data.Models;
    using XeMart.Services.Data;
    using XeMart.Services.Mapping;
    using XeMart.Web.ViewModels.Administration.Subcategories;

    public class SubcategoriesController : AdministrationController
    {
        private readonly ISubcategoriesService subcategoriesService;
        private readonly IMainCategoriesService mainCategoriesService;

        public SubcategoriesController(ISubcategoriesService subcategoriesService, IMainCategoriesService mainCategoriesService)
        {
            this.subcategoriesService = subcategoriesService;
            this.mainCategoriesService = mainCategoriesService;
        }

        public IActionResult Create()
        {
            var mainCategories = this.mainCategoriesService.All();

            var model = new CreateSubcategoryInputViewModel
            {
                MainCategories = mainCategories,
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSubcategoryInputViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                var mainCategories = this.mainCategoriesService.All();
                model.MainCategories = mainCategories;
                return this.View(model);
            }

            await this.subcategoriesService.CreateAsync<CreateSubcategoryInputViewModel>(model);

            this.TempData["Alert"] = "Successfully created subcategory.";

            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult All()
        {
            var subcategories = this.subcategoriesService.All<SubcategoryViewModel>();
            return this.View(subcategories);
        }

        public IActionResult Edit(int id)
        {
            var mainCategories = this.mainCategoriesService.All();
            var subcategory = this.subcategoriesService.GetById<EditSubcategoryViewModel>(id);
            if (subcategory == null)
            {
                this.TempData["Error"] = "Subcategory not found.";
                return this.RedirectToAction(nameof(this.All));
            }

            subcategory.MainCategories = mainCategories;

            return this.View(subcategory);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditSubcategoryViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var editResult = await this.subcategoriesService.EditAsync<EditSubcategoryViewModel>(model);
            if (editResult)
            {
                this.TempData["Alert"] = "Successfully edited subcategory.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem editing the subcategory.";
            }

            return this.RedirectToAction(nameof(this.All));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var deleteResult = await this.subcategoriesService.DeleteAsync(id);
            if (deleteResult)
            {
                this.TempData["Alert"] = "Successfully deleted subcategory.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem deleting the subcategory.";
            }

            return this.RedirectToAction(nameof(this.All));
        }
    }
}
