namespace XeMart.Web.ViewModels.Categories
{
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class SubcategoryNameViewModel : IMapFrom<Subcategory>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
