namespace XeMart.Web.ViewModels.Administration.HomePageSlides
{
    using Microsoft.AspNetCore.Http;

    using XeMart.Data.Models;
    using XeMart.Services.Mapping;
    using XeMart.Web.Infrastructure.ValidationAttributes;

    public class EditSlideViewModel : CreateSlideInputViewModel, IMapFrom<HomePageSlide>
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        [Image]
        public new IFormFile Image { get; set; }
    }
}
