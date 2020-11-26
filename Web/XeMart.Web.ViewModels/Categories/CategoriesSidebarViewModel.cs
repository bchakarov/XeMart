namespace XeMart.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class CategoriesSidebarViewModel : IMapFrom<MainCategory>
    {
        public string Name { get; set; }

        public IEnumerable<Subcategory> Subcategories { get; set; }
    }
}
