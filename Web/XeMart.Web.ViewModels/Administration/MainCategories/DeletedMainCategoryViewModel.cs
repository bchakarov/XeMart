namespace XeMart.Web.ViewModels.Administration.MainCategories
{
    using System.Globalization;

    using AutoMapper;

    using XeMart.Common;
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class DeletedMainCategoryViewModel : MainCategoryViewModel, IHaveCustomMappings
    {
        public string DeletedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<MainCategory, DeletedMainCategoryViewModel>()
            .ForMember(
                source => source.DeletedOn,
                destination => destination.MapFrom(member => member.DeletedOn.Value.ToString(GlobalConstants.DateTimeFormat, CultureInfo.InvariantCulture)));
        }
    }
}
