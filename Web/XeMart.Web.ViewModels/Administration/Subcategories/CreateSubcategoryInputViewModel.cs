namespace XeMart.Web.ViewModels.Administration.Subcategories
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    using XeMart.Data.Models;
    using XeMart.Services.Mapping;
    using XeMart.Web.Infrastructure.ValidationAttributes;

    public class CreateSubcategoryInputViewModel : IMapTo<Subcategory>
    {
        [Required]
        [MinLength(3, ErrorMessage = "The name must be at least 3 characters long.")]
        [MaxLength(30, ErrorMessage = "The name can be maximum 30 characters long.")]
        public string Name { get; set; }

        [ImageAttribute]
        public IFormFile Image { get; set; }

        [Display(Name = "Main category")]
        public int MainCategoryId { get; set; }

        public IEnumerable<MainCategory> MainCategories { get; set; }
    }
}
