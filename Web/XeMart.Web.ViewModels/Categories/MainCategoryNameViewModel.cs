namespace XeMart.Web.ViewModels.Categories
{
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class MainCategoryNameViewModel : IMapFrom<MainCategory>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
