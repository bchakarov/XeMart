namespace XeMart.Web.ViewModels.Categories
{
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class SubcategoryNameAndImageViewModel : IMapFrom<Subcategory>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }
    }
}
