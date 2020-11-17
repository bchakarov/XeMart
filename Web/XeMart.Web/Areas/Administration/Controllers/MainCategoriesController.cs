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

        public MainCategoriesController(
            IMainCategoriesService mainCategoriesService,
            IImagesService imagesService,
            IWebHostEnvironment webHostEnvironment)
        {
            this.mainCategoriesService = mainCategoriesService;
            this.imagesService = imagesService;
            this.webHostEnvironment = webHostEnvironment;
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
                var path = this.webHostEnvironment.WebRootPath + GlobalConstants.MainCategoriesDirectoryPath + model.Image.FileName;
                await this.imagesService.UploadImage(model.Image, path);
                mainCategory.ImageUrl = path.Replace(this.webHostEnvironment.WebRootPath, string.Empty).Replace("\\", "/");
            }

            await this.mainCategoriesService.Create(mainCategory);

            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult All()
        {
            var mainCategories = this.mainCategoriesService.All();

            var mainCategoryViewModels = AutoMapperConfig.MapperInstance.Map<IEnumerable<MainCategoryViewModel>>(mainCategories);

            return this.View(mainCategoryViewModels);
        }

        //public IActionResult Edit(int id)
        //{
        //    var mainCategory = this.mainCategoriesService.GetById(id);

        //    var editViewModel = AutoMapperConfig.MapperInstance.Map<EditMainCategoryViewModel>(mainCategory);

        //    return this.View(editViewModel);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Edit(EditMainCategoryViewModel model)
        //{
        //    if (!this.ModelState.IsValid)
        //    {
        //        return this.View(model);
        //    }

        //    await this.mainCategoriesService.Edit(model.Id, model.Name, model.PriceToHome, model.PriceToOffice);

        //    return this.RedirectToAction(nameof(this.All));
        //}

        public async Task<IActionResult> Delete(int id)
        {
            await this.mainCategoriesService.Delete(id);

            return this.RedirectToAction(nameof(this.All));
        }
    }
}
