namespace XeMart.Web.ViewModels.Administration.HomePageSlides
{
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class SlideViewModel : IMapFrom<HomePageSlide>
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public string LinkUrl { get; set; }

        public int Position { get; set; }
    }
}
