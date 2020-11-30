namespace XeMart.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using XeMart.Services.Data;
    using XeMart.Web.ViewModels.Favourites;

    [Authorize]
    public class FavouritesController : BaseController
    {
        private readonly IFavouritesService favouritesService;

        public FavouritesController(IFavouritesService favouritesService)
        {
            this.favouritesService = favouritesService;
        }

        public IActionResult All()
        {
            var favourites = this.favouritesService.GetAll<FavouriteProductViewModel>(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            return this.View(favourites);
        }

        public async Task<IActionResult> Add(string id)
        {
            var addResult = await this.favouritesService.AddAsync(id, this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (addResult)
            {
                this.TempData["Alert"] = "Successfully added product to favourites.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem adding the product to favourites.";
            }

            return this.RedirectToAction(nameof(this.All));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var deleteResult = await this.favouritesService.DeleteAsync(id, this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (deleteResult)
            {
                this.TempData["Alert"] = "Successfully removed product from favourites.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem removing the product from favourites.";
            }

            return this.RedirectToAction(nameof(this.All));
        }
    }
}
