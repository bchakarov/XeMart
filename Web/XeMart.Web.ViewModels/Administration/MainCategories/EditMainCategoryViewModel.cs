﻿namespace XeMart.Web.ViewModels.Administration.MainCategories
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    using XeMart.Data.Models;
    using XeMart.Services.Mapping;
    using XeMart.Web.ViewModels.ValidationAttributes;

    public class EditMainCategoryViewModel : IMapFrom<MainCategory>
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "The name must be at least 3 characters long.")]
        [MaxLength(20, ErrorMessage = "The name can be maximum 20 characters long.")]
        public string Name { get; set; }

        [Display(Name = "Icon")]
        public string FontAwesomeIcon { get; set; }

        public string ImageUrl { get; set; }

        [ImageAttribute(2 * 1024 * 1024)]
        public IFormFile Image { get; set; }
    }
}
