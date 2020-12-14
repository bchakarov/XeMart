namespace XeMart.Web.ViewModels.Administration.HomePageSlides
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class CreateSlideInputViewModel : IMapTo<HomePageSlide>
    {
        [Required]
        public IFormFile Image { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string LinkUrl { get; set; }
    }
}
