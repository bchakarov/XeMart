namespace XeMart.Web.ViewModels.Administration.Suppliers
{
    using System.ComponentModel.DataAnnotations;

    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class CreateSupplierInputViewModel : IMapTo<Supplier>
    {
        [Required]
        [MinLength(3, ErrorMessage = "The name must be at least 3 characters long.")]
        [MaxLength(20, ErrorMessage = "The name can be maximum 20 characters long.")]
        public string Name { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The field \"{0}\" should be a decimal between {1} and {2}")]
        [Display(Name = "Price for home delivery")]
        public decimal PriceToHome { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The field \"{0}\" should be a decimal between {1} and {2}")]
        [Display(Name = "Price for office delivery")]
        public decimal PriceToOffice { get; set; }
    }
}
