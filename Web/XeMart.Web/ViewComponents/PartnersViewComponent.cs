namespace XeMart.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;

    using XeMart.Common;
    using XeMart.Services.Data;
    using XeMart.Web.ViewModels.Partners;

    public class PartnersViewComponent : ViewComponent
    {
        private readonly IPartnersService partnersService;

        public PartnersViewComponent(IPartnersService partnersService)
        {
            this.partnersService = partnersService;
        }

        public IViewComponentResult Invoke()
        {
            var partners = this.partnersService.GetAllApproved<PartnerCarouselViewModel>();
            foreach (var partner in partners)
            {
                if (string.IsNullOrEmpty(partner.LogoUrl))
                {
                    partner.LogoUrl = GlobalConstants.ImageNotFoundPath;
                }
            }

            return this.View(partners);
        }
    }
}
