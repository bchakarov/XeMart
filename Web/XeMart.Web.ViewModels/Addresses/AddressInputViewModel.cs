namespace XeMart.Web.ViewModels.Addresses
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using XeMart.Data.Models;

    public class AddressInputViewModel
    {
        [Required]
        [MinLength(5, ErrorMessage = "The street must be at least 5 characters long.")]
        [MaxLength(100, ErrorMessage = "The street can be maximum 100 characters long.")]
        public string Street { get; set; }

        public string Description { get; set; }

        [Display(Name = "Country")]
        public int CountryId { get; set; }

        public IEnumerable<Country> Countries { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "The city name must be at least 3 characters long.")]
        [MaxLength(100, ErrorMessage = "The city name can be maximum 100 characters long.")]
        public string City { get; set; }

        public string ZIPCode { get; set; }

        public string UserId { get; set; }
    }
}
