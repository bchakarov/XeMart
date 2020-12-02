namespace XeMart.Web.ViewModels.ShoppingCart
{
    using System;
    using System.Linq;

    using AutoMapper;

    using XeMart.Common;
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class ShoppingCartProductViewModel : IMapFrom<ShoppingCartProduct>, IHaveCustomMappings
    {
        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public string ImageUrl { get; set; }

        public decimal ProductPrice { get; set; }

        public int Quantity { get; set; }

        public double AverageRating { get; set; }

        [IgnoreMap]
        public decimal TotalPrice => this.Quantity * this.ProductPrice;

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ShoppingCartProduct, ShoppingCartProductViewModel>()
            .ForMember(
                source => source.AverageRating,
                destination => destination.MapFrom(member => (!member.Product.Reviews.Any()) ? 0 : Math.Round(member.Product.Reviews.Average(x => x.Rating), 2)))
            .ForMember(
                source => source.ImageUrl,
                destination => destination.MapFrom(member => (!member.Product.Images.Any()) ? GlobalConstants.ImageNotFoundPath : member.Product.Images.FirstOrDefault().ImageUrl));
        }
    }
}
