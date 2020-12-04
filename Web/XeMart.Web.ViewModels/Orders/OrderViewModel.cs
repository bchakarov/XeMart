namespace XeMart.Web.ViewModels.Orders
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;

    using XeMart.Data.Models;
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

        public IEnumerable<OrderProductsViewModel> Products { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Order, OrderViewModel>()
            .ForMember(
                source => source.Address,
                destination => destination.MapFrom(member => $"{member.Address.Street} {member.Address.City.Name}, {member.Address.City.ZIPCode}, {member.Address.City.Country.Name}"));
        }
    }
}
