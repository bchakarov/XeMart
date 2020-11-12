namespace XeMart.Web.ViewModels.Administration.Suppliers
{
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class SupplierViewModel : IMapFrom<Supplier>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal PriceToHome { get; set; }

        public decimal PriceToOffice { get; set; }

        public bool IsDefault { get; set; }
    }
}
