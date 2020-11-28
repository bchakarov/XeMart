namespace XeMart.Web.ViewModels.Categories
{
    using System.Linq;

    using AutoMapper;

    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class CategoriesSidebarViewModel : IMapFrom<MainCategory>, IHaveCustomMappings
    {
        public string Name { get; set; }

        public string[] SubcategoryNames { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<MainCategory, CategoriesSidebarViewModel>()
            .ForMember(
                source => source.SubcategoryNames,
                destination => destination.MapFrom(member => member.Subcategories.Select(x => x.Name).ToArray()));
        }
    }
}
