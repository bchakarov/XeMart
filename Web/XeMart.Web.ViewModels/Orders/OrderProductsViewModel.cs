namespace XeMart.Web.ViewModels.Orders
{
    using System.Linq;

    using AutoMapper;

    using XeMart.Common;
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class OrderProductsViewModel : IMapFrom<OrderProduct>, IHaveCustomMappings
    {
        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public string ImageUrl { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        [IgnoreMap]
        public decimal TotalPrice => this.Quantity * this.Price;

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<OrderProduct, OrderProductsViewModel>()
            .ForMember(
                source => source.ImageUrl,
                destination => destination.MapFrom(member => (!member.Product.Images.Any()) ? GlobalConstants.ImageNotFoundPath : member.Product.Images.FirstOrDefault().ImageUrl));
        }
    }
}
