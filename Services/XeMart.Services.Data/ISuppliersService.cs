namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using XeMart.Data.Models.Enums;

    public interface ISuppliersService
    {
        public Task CreateAsync<T>(T supplier);

        public IEnumerable<T> GetAll<T>();

        public IEnumerable<T> GetAllDeleted<T>();

        public Task<bool> MakeDafaultAsync(int id);

        public Task<bool> EditAsync<T>(T model);

        public Task<bool> DeleteAsync(int id);

        public Task<bool> UndeleteAsync(int id);

        public T GetById<T>(int id);

        public decimal GetDeliveryPrice(int id, DeliveryType deliveryType);
    }
}
