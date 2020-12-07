namespace XeMart.Web.ViewModels.Products
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using Ganss.XSS;

    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class ProductDetailsViewModel : IMapFrom<Product>, IHaveCustomMappings
    {
        private readonly HtmlSanitizer sanitizer;

        public ProductDetailsViewModel()
        {
            this.sanitizer = new HtmlSanitizer();
            this.sanitizer.AllowedTags.Add("iframe");
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription => this.sanitizer.Sanitize(this.Description);

        public decimal Price { get; set; }

        public Subcategory Subcategory { get; set; }

        public MainCategory SubcategoryMainCategory { get; set; }

        public IEnumerable<ProductImage> Images { get; set; }

        public IEnumerable<ProductReviewViewModel> Reviews { get; set; }

        public double AverageRating { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Product, ProductDetailsViewModel>()
            .ForMember(
                source => source.Reviews,
                destination => destination.MapFrom(member => member.Reviews.OrderByDescending(x => x.CreatedOn)))
            .ForMember(
                source => source.AverageRating,
                destination => destination.MapFrom(member => (!member.Reviews.Any()) ? 0 : Math.Round(member.Reviews.Average(x => x.Rating), 2)));
        }
    }
}
