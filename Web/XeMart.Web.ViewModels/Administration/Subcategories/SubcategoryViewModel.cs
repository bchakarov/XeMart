namespace XeMart.Web.ViewModels.Administration.Subcategories
{
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class SubcategoryViewModel : IMapFrom<Subcategory>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public string MainCategoryName { get; set; }

        public int ProductsCount { get; set; }
    }
}
