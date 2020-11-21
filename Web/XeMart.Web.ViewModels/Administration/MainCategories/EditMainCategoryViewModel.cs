namespace XeMart.Web.ViewModels.Administration.MainCategories
{
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class EditMainCategoryViewModel : CreateMainCategoryInputViewModel, IMapFrom<MainCategory>, IMapTo<MainCategory>
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }
    }
}
