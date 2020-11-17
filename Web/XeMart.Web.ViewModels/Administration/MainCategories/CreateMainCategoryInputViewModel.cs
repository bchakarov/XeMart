namespace XeMart.Web.ViewModels.Administration.MainCategories
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    using XeMart.Web.ViewModels.ValidationAttributes;

    public class CreateMainCategoryInputViewModel
    {
        [Required]
        [MinLength(3, ErrorMessage = "The name must be at least 3 characters long.")]
        [MaxLength(20, ErrorMessage = "The name can be maximum 20 characters long.")]
        public string Name { get; set; }

        [Display(Name = "Icon")]
        public string FontAwesomeIcon { get; set; }

        [ImageAttribute(2 * 1024 * 1024)]
        public IFormFile Image { get; set; }
    }
}
