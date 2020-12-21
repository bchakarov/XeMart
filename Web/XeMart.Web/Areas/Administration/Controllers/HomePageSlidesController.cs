namespace XeMart.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using XeMart.Services.Data;
    using XeMart.Web.ViewModels.Administration.HomePageSlides;

    public class HomePageSlidesController : AdministrationController
    {
        private readonly IHomePageSlidesService homePageSlidesService;

        public HomePageSlidesController(IHomePageSlidesService homePageSlidesService)
        {
            this.homePageSlidesService = homePageSlidesService;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSlideInputViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.homePageSlidesService.CreateAsync(model, model.Image);

            this.TempData["Alert"] = "Successfully created slide.";

            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult All()
        {
            var products = this.homePageSlidesService.GetAll<SlideViewModel>();
            return this.View(products);
        }

        public IActionResult Edit(int id)
        {
            var slide = this.homePageSlidesService.GetById<EditSlideViewModel>(id);
            if (slide == null)
            {
                this.TempData["Error"] = "Slide not found.";
                return this.RedirectToAction(nameof(this.All));
            }

            return this.View(slide);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditSlideViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                var slide = this.homePageSlidesService.GetById<EditSlideViewModel>(model.Id);
                if (slide == null)
                {
                    this.TempData["Error"] = "Slide not found.";
                    return this.RedirectToAction(nameof(this.All));
                }

                return this.View(slide);
            }

            var editResult = await this.homePageSlidesService.EditAsync(model, model.Image);
            if (editResult)
            {
                this.TempData["Alert"] = "Successfully edited slide.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem editing the slide.";
            }

            return this.RedirectToAction(nameof(this.All));
        }

        public async Task<IActionResult> MoveUp(int id)
        {
            var moveResult = await this.homePageSlidesService.MoveUpAsync(id);
            if (moveResult)
            {
                this.TempData["Alert"] = "Successfully moved slide.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem moving the slide.";
            }

            return this.RedirectToAction(nameof(this.All));
        }

        public async Task<IActionResult> MoveDown(int id)
        {
            var moveResult = await this.homePageSlidesService.MoveDownAsync(id);
            if (moveResult)
            {
                this.TempData["Alert"] = "Successfully moved slide.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem moving the slide.";
            }

            return this.RedirectToAction(nameof(this.All));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var deleteResult = await this.homePageSlidesService.DeleteAsync(id);
            if (deleteResult)
            {
                this.TempData["Alert"] = "Successfully deleted slide.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem deleting the slide.";
            }

            return this.RedirectToAction(nameof(this.All));
        }
    }
}
