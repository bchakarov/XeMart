namespace XeMart.Web.ViewModels.Administration.Products
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class CreateProductInputViewModel : IMapTo<Product>
    {
        [Required]
        [MinLength(3, ErrorMessage = "The name must be at least 3 characters long.")]
        [MaxLength(20, ErrorMessage = "The name can be maximum 20 characters long.")]
        public string Name { get; set; }

        [MaxLength(2000, ErrorMessage = "The desciption can be maximum 2000 characters long.")]
        public string Description { get; set; }

        [Range(1, (double)int.MaxValue)]
        public decimal Price { get; set; }

        [Display(Name = "Subcategory")]
        public int SubcategoryId { get; set; }

        public IEnumerable<Subcategory> Subcategories { get; set; }

        [Display(Name = "Add Images")]
        public IEnumerable<IFormFile> UploadedImages { get; set; }
    }
}
