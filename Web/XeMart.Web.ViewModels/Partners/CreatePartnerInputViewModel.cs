namespace XeMart.Web.ViewModels.Partners
{
    using System.ComponentModel.DataAnnotations;

    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class CreatePartnerInputViewModel : IMapTo<Partner>
    {
        [Required]
        [MinLength(3, ErrorMessage = "The name must be at least 3 characters long.")]
        [MaxLength(20, ErrorMessage = "The name can be maximum 20 characters long.")]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Required]
        [Url]
        [Display(Name = "Company URL")]
        public string CompanyUrl { get; set; }
    }
}
