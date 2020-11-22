namespace XeMart.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using XeMart.Common;
    using XeMart.Services.Data;
    using XeMart.Web.ViewModels.Partners;

    public class PartnersController : BaseController
    {
        private readonly IPartnersService partnersService;

        public PartnersController(IPartnersService partnersService)
        {
            this.partnersService = partnersService;
        }

        [Authorize]
        public IActionResult Create()
        {
            if (this.User.IsInRole(GlobalConstants.PartnerRoleName))
            {
                return this.RedirectToAction(nameof(this.Edit));
            }

            return this.View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreatePartnerInputViewModel model)
        {
            if (this.User.IsInRole(GlobalConstants.PartnerRoleName))
            {
                return this.RedirectToAction(nameof(this.Edit));
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var createResult = await this.partnersService.CreateAsync<CreatePartnerInputViewModel>(model, this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (createResult)
            {
                this.TempData["Alert"] = "Successfully sent partner request.";
            }
            else
            {
                this.TempData["Error"] = "You have already sent a partner request.";
            }

            return this.RedirectToAction(nameof(this.Create));
        }

        [Authorize(Roles = GlobalConstants.PartnerRoleName)]
        public IActionResult Edit()
        {
            var partner = this.partnersService.GetByManagerId<EditPartnerViewModel>(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (partner == null)
            {
                this.TempData["Error"] = "Partner not found.";
                return this.RedirectToAction(nameof(this.Create));
            }

            return this.View(partner);
        }

        [Authorize(Roles = GlobalConstants.PartnerRoleName)]
        [HttpPost]
        public IActionResult Edit(EditPartnerViewModel model)
        {
            return this.View();
        }
    }
}
