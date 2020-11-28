namespace XeMart.Web.ViewModels.Administration.Subcategories
{
    using System.Globalization;

    using AutoMapper;

    using XeMart.Common;
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class DeletedSubcategoryViewModel : SubcategoryViewModel, IHaveCustomMappings
    {
        public string DeletedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Subcategory, DeletedSubcategoryViewModel>()
            .ForMember(
                source => source.DeletedOn,
                destination => destination.MapFrom(member => member.DeletedOn.Value.ToString(GlobalConstants.DateTimeFormat, CultureInfo.InvariantCulture)));
        }
    }
}
