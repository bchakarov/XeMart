namespace XeMart.Web.ViewModels.Administration.MainCategories
{
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class MainCategoryViewModel : IMapFrom<MainCategory>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string FontAwesomeIcon { get; set; }

        public int SubcategoriesCount { get; set; }
    }
}
