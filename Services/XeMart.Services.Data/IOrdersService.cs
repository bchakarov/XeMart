namespace XeMart.Services.Data
{
    using System.Threading.Tasks;

    using XeMart.Data.Models.Enums;

    public interface IOrdersService
    {
        public Task CreateAsync<T>(T model, string userId);

        public Task<string> CompleteOrderAsync(string userId);

        public T GetById<T>(string id);

        public PaymentType GetPaymentTypeById(string id);
    }
}
