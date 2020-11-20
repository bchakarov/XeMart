namespace XeMart.Web.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    using XeMart.Common;
    using XeMart.Data.Models;
    using XeMart.Services;
    using XeMart.Services.Data;
    using XeMart.Services.Mapping;
    using XeMart.Web.ViewModels.Administration.MainCategories;

    public class MainCategoriesController : AdministrationController
    {
        private readonly IMainCategoriesService mainCategoriesService;
        private readonly IImagesService imagesService;
        private readonly IWebHostEnvironment webHostEnvironment;

        private string path;

        public MainCategoriesController(
            IMainCategoriesService mainCategoriesService,
            IImagesService imagesService,
            IWebHostEnvironment webHostEnvironment)
        {
            this.mainCategoriesService = mainCategoriesService;
            this.imagesService = imagesService;
            this.webHostEnvironment = webHostEnvironment;

            this.path = this.webHostEnvironment.WebRootPath + GlobalConstants.MainCategoriesDirectoryPath;
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

            var mainCategory = new MainCategory
            {
                Name = model.Name,
                FontAwesomeIcon = model.FontAwesomeIcon,
            };

            if (model.Image != null)
            {
                this.path += model.Image.FileName;
                await this.imagesService.UploadImage(model.Image, this.path);
                mainCategory.ImageUrl = this.path.Replace(this.webHostEnvironment.WebRootPath, string.Empty).Replace("\\", "/");
            }

            await this.mainCategoriesService.CreateAsync(mainCategory);

            this.TempData["Alert"] = "Successfully created main category.";

            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult All()
        {
            var mainCategories = this.mainCategoriesService.All();

            var mainCategoryViewModels = AutoMapperConfig.MapperInstance.Map<IEnumerable<MainCategoryViewModel>>(mainCategories);

            return this.View(mainCategoryViewModels);
        }

        public IActionResult Edit(int id)
        {
            var mainCategory = this.mainCategoriesService.GetById(id);

            var editViewModel = AutoMapperConfig.MapperInstance.Map<EditMainCategoryViewModel>(mainCategory);

            return this.View(editViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditMainCategoryViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            if (model.Image != null)
            {
                this.path += model.Image.FileName;
                await this.imagesService.UploadImage(model.Image, this.path);
                model.ImageUrl = this.path.Replace(this.webHostEnvironment.WebRootPath, string.Empty).Replace("\\", "/");
            }

            await this.mainCategoriesService.EditAsync(model.Id, model.Name, model.FontAwesomeIcon, model.ImageUrl);

            this.TempData["Alert"] = "Successfully edited main category.";

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
                this.TempData["Error"] = "Cannot delete a main category with subcategories in it.";
            }

            return this.RedirectToAction(nameof(this.All));
        }
    }
}
