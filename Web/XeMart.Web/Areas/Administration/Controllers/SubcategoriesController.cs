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

            var subcategory = AutoMapperConfig.MapperInstance.Map<Subcategory>(model);

            await this.subcategoriesService.CreateAsync(subcategory);

            this.TempData["Alert"] = "Successfully created subcategory.";

            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult All()
        {
            var subcategories = this.subcategoriesService.All();

            var subcategoryViewModels = AutoMapperConfig.MapperInstance.Map<IEnumerable<SubcategoryViewModel>>(subcategories);

            return this.View(subcategoryViewModels);
        }

        public IActionResult Edit(int id)
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit()
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            this.TempData["Alert"] = "Successfully edited subcategory.";

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
                this.TempData["Error"] = "Cannot delete subcategories with products in it.";
            }

            return this.RedirectToAction(nameof(this.All));
        }
    }
}
