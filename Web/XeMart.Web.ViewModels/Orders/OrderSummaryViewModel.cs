namespace XeMart.Web.ViewModels.Orders
{
    using System.Globalization;

    using AutoMapper;

    using XeMart.Common;
    using XeMart.Data.Models;
    using XeMart.Data.Models.Enums;
    using XeMart.Services.Mapping;

    public class OrderSummaryViewModel : IMapFrom<Order>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string CreatedOn { get; set; }

        public OrderStatus Status { get; set; }

        public PaymentType PaymentType { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public decimal TotalPrice { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Order, OrderSummaryViewModel>()
            .ForMember(
                source => source.CreatedOn,
                destination => destination.MapFrom(member => member.CreatedOn.ToString(GlobalConstants.DateTimeFormat, CultureInfo.InvariantCulture)));
        }
    }
}
