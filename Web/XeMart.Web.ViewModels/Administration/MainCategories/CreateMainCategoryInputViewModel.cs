namespace XeMart.Web.ViewModels.Administration.MainCategories
{
    using System.ComponentModel.DataAnnotations;

    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class CreateMainCategoryInputViewModel : IMapTo<MainCategory>
    {
        [Required]
        [MinLength(3, ErrorMessage = "The name must be at least 3 characters long.")]
        [MaxLength(30, ErrorMessage = "The name can be maximum 30 characters long.")]
        public string Name { get; set; }

        [Display(Name = "Icon")]
        public string FontAwesomeIcon { get; set; }
    }
}
