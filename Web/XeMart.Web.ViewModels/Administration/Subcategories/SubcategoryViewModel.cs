namespace XeMart.Web.ViewModels.Administration.Subcategories
{
    using AutoMapper;

    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class SubcategoryViewModel : IMapFrom<Subcategory>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string MainCategoryName { get; set; }

        public int ProductsCount { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Subcategory, SubcategoryViewModel>()
            .ForMember(
                source => source.MainCategoryName,
                destination => destination.MapFrom(member => member.MainCategory.Name));
        }
    }
}
