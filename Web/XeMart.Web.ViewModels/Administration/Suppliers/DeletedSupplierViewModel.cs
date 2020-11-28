namespace XeMart.Web.ViewModels.Administration.Suppliers
{
    using System.Globalization;

    using AutoMapper;

    using XeMart.Common;
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class DeletedSupplierViewModel : SupplierViewModel, IHaveCustomMappings
    {
        public string DeletedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Supplier, DeletedSupplierViewModel>()
            .ForMember(
                source => source.DeletedOn,
                destination => destination.MapFrom(member => member.DeletedOn.Value.ToString(GlobalConstants.DateTimeFormat, CultureInfo.InvariantCulture)));
        }
    }
}
