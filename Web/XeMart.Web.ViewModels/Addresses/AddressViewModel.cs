namespace XeMart.Web.ViewModels.Addresses
{
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class AddressViewModel : IMapFrom<Address>
    {
        public string Id { get; set; }

        public string Street { get; set; }

        public string Description { get; set; }

        public string CityName { get; set; }

        public string CityZIPCode { get; set; }

        public string CityCountryName { get; set; }
    }
}
