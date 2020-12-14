namespace XeMart.Web.ViewModels.HomePageSlides
{
    using Ganss.XSS;

    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class SlideHomeViewModel : IMapFrom<HomePageSlide>
    {
        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        public string LinkUrl { get; set; }
    }
}
