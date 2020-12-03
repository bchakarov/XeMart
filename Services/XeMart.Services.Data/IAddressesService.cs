namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using XeMart.Web.ViewModels.Addresses;

    public interface IAddressesService
    {
        public Task<bool> CreateAsync(AddressInputViewModel model);

        public IEnumerable<T> GetAll<T>(string userId);

        public Task<bool> DeleteAsync(string id);
    }
}
