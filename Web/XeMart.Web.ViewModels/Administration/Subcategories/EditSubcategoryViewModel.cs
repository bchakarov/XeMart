namespace XeMart.Web.ViewModels.Administration.Subcategories
{
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class EditSubcategoryViewModel : CreateSubcategoryInputViewModel, IMapFrom<Subcategory>
    {
        public int Id { get; set; }
    }
}
