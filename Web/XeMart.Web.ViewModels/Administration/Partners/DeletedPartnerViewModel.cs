namespace XeMart.Web.ViewModels.Administration.Partners
{
    using System.Globalization;

    using AutoMapper;

    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class DeletedPartnerViewModel : PartnerViewModel, IHaveCustomMappings
    {
        public string DeletedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Partner, DeletedPartnerViewModel>()
            .ForMember(
                source => source.DeletedOn,
                destination => destination.MapFrom(member => member.DeletedOn.Value.ToString("f", CultureInfo.InvariantCulture)));
        }
    }
}
