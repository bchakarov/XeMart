namespace XeMart.Web.ViewModels.Favourites
{
    using System;
    using System.Linq;

    using AutoMapper;

    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class FavouriteProductViewModel : IMapFrom<UserFavouriteProduct>, IHaveCustomMappings
    {
        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; }

        public string ImageUrl { get; set; }

        public double AverageRating { get; set; }

        public virtual void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<UserFavouriteProduct, FavouriteProductViewModel>()
            .ForMember(
                source => source.ImageUrl,
                destination => destination.MapFrom(member => member.Product.Images.FirstOrDefault().ImageUrl))
            .ForMember(
                source => source.AverageRating,
                destination => destination.MapFrom(member => (!member.Product.Reviews.Any()) ? 0 : Math.Round(member.Product.Reviews.Average(x => x.Rating), 2)));
        }
    }
}
