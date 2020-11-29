namespace XeMart.Web.ViewModels.Products
{
    using System;
    using System.Linq;

    using AutoMapper;

    using XeMart.Common;
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class ProductViewModel : IMapFrom<Product>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public double AverageRating { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Product, ProductViewModel>()
            .ForMember(
                source => source.AverageRating,
                destination => destination.MapFrom(member => (!member.Reviews.Any()) ? 0 : Math.Round(member.Reviews.Average(x => x.Rating), 2)))
            .ForMember(
                source => source.ImageUrl,
                destination => destination.MapFrom(member => (!member.Images.Any()) ? GlobalConstants.ImageNotFoundPath : member.Images.FirstOrDefault().ImageUrl));
        }
    }
}
