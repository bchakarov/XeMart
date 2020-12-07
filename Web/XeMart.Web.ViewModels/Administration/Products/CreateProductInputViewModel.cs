namespace XeMart.Web.ViewModels.Administration.Products
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    using XeMart.Data.Models;
    using XeMart.Services.Mapping;
    using XeMart.Web.Infrastructure.ValidationAttributes;

    public class CreateProductInputViewModel : IMapTo<Product>
    {
        [Required]
        [MinLength(3, ErrorMessage = "The name must be at least 3 characters long.")]
        [MaxLength(100, ErrorMessage = "The name can be maximum 100 characters long.")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(1, (double)int.MaxValue)]
        [Display(Name = "Price (USD)")]
        public decimal Price { get; set; }

        [Display(Name = "Subcategory")]
        public int SubcategoryId { get; set; }

        public IEnumerable<Subcategory> Subcategories { get; set; }

        [Display(Name = "Add Images")]
        [MultipleImageAttribute]
        public IEnumerable<IFormFile> UploadedImages { get; set; }
    }
}
