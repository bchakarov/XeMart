namespace XeMart.Web.ViewModels.Orders
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;

    using AutoMapper;

    using XeMart.Common;
    using XeMart.Data.Models;
    using XeMart.Data.Models.Enums;
    using XeMart.Services.Mapping;

    public class OrderViewModel : IMapFrom<Order>, IHaveCustomMappings
    {
        [Display(Name = "Order Id")]
        public string Id { get; set; }

        [Display(Name = "Full Name")]
        public string UserFullName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public decimal DeliveryPrice { get; set; }

        public decimal TotalPrice { get; set; }

        [Display(Name = "Created On")]
        public string CreatedOn { get; set; }

        [Display(Name = "Payment Type")]
        public PaymentType PaymentType { get; set; }

        [Display(Name = "Payment Status")]
        public PaymentStatus PaymentStatus { get; set; }

        public bool IsDelivered { get; set; }

        [Display(Name = "Delivered On")]
        public string DeliveredOn { get; set; }

        [Display(Name = "Order Status")]
        public OrderStatus Status { get; set; }

        [Display(Name = "Supplier")]
        public string SupplierName { get; set; }

        [Display(Name = "Delivery Type")]
        public DeliveryType DeliveryType { get; set; }

        public IEnumerable<OrderProductsViewModel> Products { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Order, OrderViewModel>()
            .ForMember(
                source => source.Address,
                destination => destination.MapFrom(member => $"{member.Address.Street} {member.Address.City.Name}, {member.Address.City.ZIPCode}, {member.Address.City.Country.Name}"))
            .ForMember(
                source => source.CreatedOn,
                destination => destination.MapFrom(member => member.CreatedOn.ToString(GlobalConstants.DateTimeFormat, CultureInfo.InvariantCulture)))
            .ForMember(
                source => source.DeliveredOn,
                destination => destination.MapFrom(member => (member.DeliveredOn == null) ? null : member.DeliveredOn.Value.ToString(GlobalConstants.DateTimeFormat, CultureInfo.InvariantCulture)));
        }
    }
}
