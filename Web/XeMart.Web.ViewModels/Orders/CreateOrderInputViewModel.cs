namespace XeMart.Web.ViewModels.Orders
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using XeMart.Data.Models;
    using XeMart.Data.Models.Enums;
    using XeMart.Services.Mapping;
    using XeMart.Web.ViewModels.Addresses;
    using XeMart.Web.ViewModels.Administration.Suppliers;

    public class CreateOrderInputViewModel : IMapTo<Order>
    {
        [Required]
        [MinLength(10, ErrorMessage = "The name must be at least 10 characters long.")]
        [MaxLength(100, ErrorMessage = "The name can be maximum 100 characters long.")]
        [Display(Name = "Full Name")]
        public string UserFullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^[+]?[0-9]+$", ErrorMessage = "The phone can have only numbers and can begin with a plus symbol.")]
        public string Phone { get; set; }

        public int SupplierId { get; set; }

        public IEnumerable<SupplierViewModel> Suppliers { get; set; }

        [Required]
        public DeliveryType DeliveryType { get; set; }

        [Required]
        public string AddressId { get; set; }

        public IEnumerable<AddressViewModel> Addresses { get; set; }

        public IEnumerable<Country> Countries { get; set; }

        public PaymentType PaymentType { get; set; }
    }
}
