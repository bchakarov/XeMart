namespace XeMart.Web.ViewModels.Administration.MainCategories
{
    using AutoMapper;

    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class MainCategoryViewModel : IMapFrom<MainCategory>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string FontAwesomeIcon { get; set; }

        public string ImageUrl { get; set; }

        public int SubcategoriesCount { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<MainCategory, MainCategoryViewModel>()
            .ForMember(
                source => source.SubcategoriesCount,
                destination => destination.MapFrom(member => member.Subcategories.Count));
        }
    }
}
