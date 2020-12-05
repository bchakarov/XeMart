namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using XeMart.Data.Common.Repositories;
    using XeMart.Data.Models;
    using XeMart.Data.Models.Enums;
    using XeMart.Services.Mapping;
    using XeMart.Web.ViewModels.ShoppingCart;

    public class OrdersService : IOrdersService
    {
        private readonly IDeletableEntityRepository<Order> ordersRepository;
        private readonly ISuppliersService suppliersService;
        private readonly IShoppingCartService shoppingCartService;

        public OrdersService(
            IDeletableEntityRepository<Order> ordersRepository,
            ISuppliersService suppliersService,
            IShoppingCartService shoppingCartService)
        {
            this.ordersRepository = ordersRepository;
            this.suppliersService = suppliersService;
            this.shoppingCartService = shoppingCartService;
        }

        public async Task CreateAsync<T>(T model, string userId)
        {
            var order = this.GetProcessingOrder(userId);
            if (order == null)
            {
                order = AutoMapperConfig.MapperInstance.Map<Order>(model);
                order.DeliveryPrice = this.suppliersService.GetDeliveryPrice(order.SupplierId, order.DeliveryType);
                order.UserId = userId;
                await this.ordersRepository.AddAsync(order);
                await this.ordersRepository.SaveChangesAsync();
            }
            else
            {
                order.DeliveryPrice = this.suppliersService.GetDeliveryPrice(order.SupplierId, order.DeliveryType);
                this.ordersRepository.Update(order);
                await this.ordersRepository.SaveChangesAsync();
            }
        }

        public async Task<string> CompleteOrderAsync(string userId)
        {
            var order = this.GetProcessingOrder(userId);
            if (order == null)
            {
                return null;
            }

            var shoppingCartProducts = await this.shoppingCartService.GetAllProducts<ShoppingCartProductViewModel>(true, null, userId);
            if (shoppingCartProducts == null || shoppingCartProducts.Count() == 0)
            {
                return null;
            }

            var orderProducts = new List<OrderProduct>();

            foreach (var shoppingCartProduct in shoppingCartProducts)
            {
                var orderProduct = new OrderProduct
                {
                    Order = order,
                    ProductId = shoppingCartProduct.ProductId,
                    Quantity = shoppingCartProduct.Quantity,
                    Price = shoppingCartProduct.ProductPrice,
                };

                orderProducts.Add(orderProduct);
            }

            await this.shoppingCartService.DeleteAllProductsAsync(userId);

            order.Status = OrderStatus.Unprocessed;
            order.Products = orderProducts;
            order.TotalPrice = order.Products.Sum(x => x.Quantity * x.Price) + order.DeliveryPrice;

            this.ordersRepository.Update(order);
            await this.ordersRepository.SaveChangesAsync();

            return order.Id;
        }

        public IEnumerable<T> TakeOrdersByUserId<T>(string userId, int page, int ordersToTake) =>
            this.ordersRepository.AllAsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedOn)
            .Skip((page - 1) * ordersToTake)
            .Take(ordersToTake)
            .To<T>().ToList();

        public int GetCountByUserId(string userId) =>
            this.ordersRepository.AllAsNoTracking()
            .Count(x => x.UserId == userId);

        public T GetById<T>(string id) =>
            this.ordersRepository.AllAsNoTracking()
            .Where(x => x.Id == id)
            .To<T>().FirstOrDefault();

        public PaymentType GetPaymentTypeById(string id) =>
            this.ordersRepository.AllAsNoTracking()
            .FirstOrDefault(x => x.Id == id)
            .PaymentType;

        public bool UserHasOrder(string userId, string orderId) =>
            this.ordersRepository.AllAsNoTracking()
            .Any(x => x.UserId == userId && x.Id == orderId);

        private Order GetProcessingOrder(string userId) =>
            this.ordersRepository.All()
            .FirstOrDefault(x => x.UserId == userId && x.Status == OrderStatus.Processing);
    }
}
