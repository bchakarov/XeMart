namespace XeMart.Web.ViewModels.Administration.Suppliers
{
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class EditSupplierViewModel : CreateSupplierInputViewModel, IMapFrom<Supplier>
    {
        public int Id { get; set; }
    }
}
