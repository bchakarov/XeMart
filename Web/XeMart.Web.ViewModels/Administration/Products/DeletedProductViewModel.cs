namespace XeMart.Web.ViewModels.Administration.Products
{
    using System.Globalization;
    using System.Linq;

    using AutoMapper;

    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class DeletedProductViewModel : ProductViewModel, IHaveCustomMappings
    {
        public string DeletedOn { get; set; }

        public override void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Product, DeletedProductViewModel>()
            .ForMember(
                source => source.ImageUrl,
                destination => destination.MapFrom(member => member.Images.FirstOrDefault().ImageUrl))
            .ForMember(
                source => source.DeletedOn,
                destination => destination.MapFrom(member => member.DeletedOn.Value.ToString("f", CultureInfo.InvariantCulture)));
        }
    }
}
