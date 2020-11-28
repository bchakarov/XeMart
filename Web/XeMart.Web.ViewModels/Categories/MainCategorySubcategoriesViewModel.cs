namespace XeMart.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class MainCategorySubcategoriesViewModel : IMapFrom<MainCategory>
    {
        public string Name { get; set; }

        public IEnumerable<SubcategoryNameAndImageViewModel> Subcategories { get; set; }
    }
}
