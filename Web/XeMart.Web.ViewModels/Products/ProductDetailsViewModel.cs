namespace XeMart.Web.ViewModels.Products
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class ProductDetailsViewModel : IMapFrom<Product>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public Subcategory Subcategory { get; set; }

        public MainCategory SubcategoryMainCategory { get; set; }

        public IEnumerable<ProductImage> Images { get; set; }

        public IEnumerable<UserProductReview> Reviews { get; set; }

        [IgnoreMap]
        public double AverageRating => Math.Round(this.Reviews.Average(x => x.Rating), 2);

        [IgnoreMap]
        public double AverageRatingRounded => Math.Round(this.AverageRating * 2, MidpointRounding.AwayFromZero) / 2;

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Product, ProductDetailsViewModel>()
            .ForMember(
                source => source.Reviews,
                destination => destination.MapFrom(member => member.Reviews.OrderByDescending(x => x.CreatedOn)));
        }
    }
}
