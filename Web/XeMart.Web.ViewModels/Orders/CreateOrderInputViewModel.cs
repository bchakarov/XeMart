namespace XeMart.Web.ViewModels.Orders
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using XeMart.Data.Models;
    using XeMart.Web.ViewModels.Addresses;

    public class CreateOrderInputViewModel
    {
        [Required]
        [MinLength(10, ErrorMessage = "The name must be at least 10 characters long.")]
        [MaxLength(100, ErrorMessage = "The name can be maximum 100 characters long.")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^[+]?[0-9]+$", ErrorMessage = "The phone can have only numbers and can begin with a plus symbol.")]
        public string Phone { get; set; }

        [Required]
        public string AddressId { get; set; }

        public IEnumerable<AddressViewModel> Addresses { get; set; }

        public IEnumerable<Country> Countries { get; set; }
    }
}
