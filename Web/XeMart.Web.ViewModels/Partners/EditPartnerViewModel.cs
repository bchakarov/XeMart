namespace XeMart.Web.ViewModels.Partners
{
    using Microsoft.AspNetCore.Http;

    using XeMart.Data.Models;
    using XeMart.Services.Mapping;
    using XeMart.Web.Infrastructure.ValidationAttributes;

    public class EditPartnerViewModel : CreatePartnerInputViewModel, IMapFrom<Partner>, IMapTo<Partner>
    {
        public int Id { get; set; }

        public string LogoUrl { get; set; }

        [ImageAttribute(2 * 1024 * 1024)]
        public IFormFile Logo { get; set; }
    }
}
