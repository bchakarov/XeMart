namespace XeMart.Web.ViewModels.Administration.Subcategories
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class CreateSubcategoryInputViewModel : IMapTo<Subcategory>
    {
        [Required]
        [MinLength(3, ErrorMessage = "The name must be at least 3 characters long.")]
        [MaxLength(20, ErrorMessage = "The name can be maximum 20 characters long.")]
        public string Name { get; set; }

        [Display(Name = "Main category")]
        public int MainCategoryId { get; set; }

        public IEnumerable<MainCategory> MainCategories { get; set; }
    }
}
