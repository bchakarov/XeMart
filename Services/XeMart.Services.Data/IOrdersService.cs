namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using XeMart.Data.Models;
    using XeMart.Data.Models.Enums;

    public interface IOrdersService
    {
        public Task CreateAsync<T>(T model, string userId);

        public Task<string> CompleteOrderAsync(string userId);

        public Task<bool> SetOrderStatusAsync(string id, string status);

        public IEnumerable<T> TakeOrdersByUserId<T>(string userId, int page, int ordersToTake);

        public IEnumerable<T> TakeOrdersByStatus<T>(OrderStatus status, int page, int ordersToTake);

        public IEnumerable<T> TakeProcessingAndUnprocessedOrders<T>(int page, int ordersToTake);

        public IEnumerable<T> TakeDeletedOrders<T>(int page, int ordersToTake);

        public int GetOrdersCountByUserId(string userId);

        public int GetOrdersCountByStatus(OrderStatus status);

        public int GetDeletedOrdersCount();

        public T GetById<T>(string id);

        public Order GetProcessingOrderByUserId(string userId);

        public PaymentType GetPaymentTypeById(string id);

        public bool UserHasOrder(string userId, string orderId);

        public Task FulfillOrderById(string id);

        public Task CancelAnyProcessingOrders(string userId);

        public Task<bool> DeleteAsync(string id);

        public Task<bool> UndeleteAsync(string id);
    }
}
