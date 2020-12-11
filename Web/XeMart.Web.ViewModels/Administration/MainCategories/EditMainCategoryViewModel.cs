namespace XeMart.Web.ViewModels.Administration.MainCategories
{
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class EditMainCategoryViewModel : CreateMainCategoryInputViewModel, IMapFrom<MainCategory>
    {
        public int Id { get; set; }
    }
}
