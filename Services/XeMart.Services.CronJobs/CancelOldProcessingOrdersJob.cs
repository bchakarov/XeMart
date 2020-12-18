namespace XeMart.Services.CronJobs
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using XeMart.Data.Common.Repositories;
    using XeMart.Data.Models;
    using XeMart.Data.Models.Enums;

    public class CancelOldProcessingOrdersJob
    {
        private readonly IDeletableEntityRepository<Order> ordersRepository;

        public CancelOldProcessingOrdersJob(IDeletableEntityRepository<Order> ordersRepository)
        {
            this.ordersRepository = ordersRepository;
        }

        public async Task Work()
        {
            var processingOrders = this.ordersRepository.All().Where(x => x.Status == OrderStatus.Processing).ToList();
            var oldProcessingOrders = processingOrders.Where(x => DateTime.UtcNow.Subtract(x.CreatedOn).TotalMinutes > 5);

            foreach (var order in oldProcessingOrders)
            {
                order.Status = OrderStatus.Cancelled;
                this.ordersRepository.Update(order);
            }

            await this.ordersRepository.SaveChangesAsync();
        }
    }
}
