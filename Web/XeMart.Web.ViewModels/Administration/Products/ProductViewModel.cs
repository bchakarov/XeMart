namespace XeMart.Web.ViewModels.Administration.Products
{
    using System.Linq;

    using AutoMapper;

    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class ProductViewModel : IMapFrom<Product>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public string SubcategoryName { get; set; }

        public virtual void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Product, ProductViewModel>()
            .ForMember(
                source => source.ImageUrl,
                destination => destination.MapFrom(member => member.Images.FirstOrDefault().ImageUrl));
        }
    }
}
