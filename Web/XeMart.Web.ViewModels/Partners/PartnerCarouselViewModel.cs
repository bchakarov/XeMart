namespace XeMart.Web.ViewModels.Partners
{
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class PartnerCarouselViewModel : IMapFrom<Partner>
    {
        public string CompanyName { get; set; }

        public string CompanyUrl { get; set; }

        public string LogoUrl { get; set; }
    }
}
