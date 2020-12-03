namespace XeMart.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using XeMart.Services.Data;
    using XeMart.Web.ViewModels.Addresses;

    public class AddressesController : BaseController
    {
        private readonly IAddressesService addressesService;

        public AddressesController(IAddressesService addressesService)
        {
            this.addressesService = addressesService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddressInputViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Create", "Orders");
            }

            var createResult = await this.addressesService.CreateAsync(model);
            if (createResult)
            {
                this.TempData["Alert"] = "Successfully added address.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem adding the address.";
            }

            return this.RedirectToAction("Create", "Orders");
        }

        public async Task<IActionResult> Delete(string id)
        {
            var deleteResult = await this.addressesService.DeleteAsync(id);
            if (deleteResult)
            {
                this.TempData["Alert"] = "Successfully deleted address.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem deleting the address.";
            }

            return this.RedirectToAction("Create", "Orders");
        }
    }
}
