namespace XeMart.Web.ViewModels.Home
{
    using System.Collections.Generic;

    using XeMart.Web.ViewModels.HomePageSlides;
    using XeMart.Web.ViewModels.Products;

    public class IndexViewModel
    {
        public IEnumerable<ProductSidebarViewModel> MostBoughtProducts { get; set; }

        public IEnumerable<ProductViewModel> NewestProducts { get; set; }

        public IEnumerable<ProductSidebarViewModel> TopRatedProducts { get; set; }

        public IEnumerable<SlideHomeViewModel> Slides { get; set; }
    }
}
