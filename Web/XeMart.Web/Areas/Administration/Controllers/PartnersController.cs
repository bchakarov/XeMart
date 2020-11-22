namespace XeMart.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using XeMart.Services.Data;
    using XeMart.Web.ViewModels.Administration.Partners;
    using XeMart.Web.ViewModels.Partners;

    public class PartnersController : AdministrationController
    {
        private readonly IPartnersService partnersService;

        public PartnersController(IPartnersService partnersService)
        {
            this.partnersService = partnersService;
        }

        public IActionResult Approved()
        {
            var suppliers = this.partnersService.AllApproved<PartnerViewModel>();
            return this.View(suppliers);
        }

        public IActionResult Requests()
        {
            var suppliers = this.partnersService.AllRequests<PartnerViewModel>();
            return this.View(suppliers);
        }

        public async Task<IActionResult> Approve(int id)
        {
            var approveResult = await this.partnersService.ApproveAsync(id);
            if (approveResult)
            {
                this.TempData["Alert"] = "Successfully approved partner.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem approving the partner.";
            }

            return this.RedirectToAction(nameof(this.Approved));
        }

        public async Task<IActionResult> Unapprove(int id)
        {
            var unapproveResult = await this.partnersService.UnapproveAsync(id);
            if (unapproveResult)
            {
                this.TempData["Alert"] = "Successfully unapproved partner.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem unapproving the partner.";
            }

            return this.RedirectToAction(nameof(this.Requests));
        }

        public IActionResult Edit(int id)
        {
            var partner = this.partnersService.GetById<EditPartnerViewModel>(id);
            if (partner == null)
            {
                this.TempData["Error"] = "Partner not found.";
                return this.RedirectToAction(nameof(this.Requests));
            }

            return this.View(partner);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditPartnerViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var editResult = await this.partnersService.EditAsync<EditPartnerViewModel>(model, model.Logo);
            if (editResult)
            {
                this.TempData["Alert"] = "Successfully edited partner.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem editing the partner.";
            }

            return this.RedirectToAction(nameof(this.Approved));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var deleteResult = await this.partnersService.DeleteAsync(id);
            if (deleteResult)
            {
                this.TempData["Alert"] = "Successfully deleted partner.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem deleting the partner.";
            }

            return this.RedirectToAction(nameof(this.Requests));
        }
    }
}
