namespace XeMart.Web.ViewModels.Partners
{
    using Microsoft.AspNetCore.Http;

    using XeMart.Data.Models;
    using XeMart.Services.Mapping;
    using XeMart.Web.Infrastructure.ValidationAttributes;

    public class EditPartnerViewModel : CreatePartnerInputViewModel, IMapFrom<Partner>
    {
        public int Id { get; set; }

        public string LogoUrl { get; set; }

        [ImageAttribute]
        public IFormFile Logo { get; set; }
    }
}
