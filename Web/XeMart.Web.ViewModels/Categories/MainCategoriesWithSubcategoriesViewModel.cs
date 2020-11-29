namespace XeMart.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class MainCategoriesWithSubcategoriesViewModel : IMapFrom<MainCategory>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string FontAwesomeIcon { get; set; }

        public IEnumerable<SubcategoryNameViewModel> Subcategories { get; set; }
    }
}
