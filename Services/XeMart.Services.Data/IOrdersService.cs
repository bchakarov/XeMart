namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using XeMart.Data.Models.Enums;

    public interface IOrdersService
    {
        public Task CreateAsync<T>(T model, string userId);

        public Task<string> CompleteOrderAsync(string userId);

        public IEnumerable<T> GetAll<T>(string userId);

        public T GetById<T>(string id);

        public PaymentType GetPaymentTypeById(string id);

        public bool UserHasOrder(string userId, string orderId);
    }
}
