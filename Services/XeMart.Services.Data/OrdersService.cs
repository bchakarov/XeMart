﻿namespace XeMart.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using XeMart.Data.Common.Repositories;
    using XeMart.Data.Models;
    using XeMart.Data.Models.Enums;
    using XeMart.Services.Mapping;
    using XeMart.Services.Messaging;
    using XeMart.Web.ViewModels.Orders;
    using XeMart.Web.ViewModels.ShoppingCart;

    public class OrdersService : IOrdersService
    {
        private readonly IDeletableEntityRepository<Order> ordersRepository;
        private readonly IRepository<OrderProduct> orderProductsRepository;
        private readonly IProductsService productsService;
        private readonly ISuppliersService suppliersService;
        private readonly IShoppingCartService shoppingCartService;
        private readonly IEmailSender emailSender;
        private readonly IViewRenderService viewRenderService;

        public OrdersService(
            IDeletableEntityRepository<Order> ordersRepository,
            IRepository<OrderProduct> orderProductsRepository,
            IProductsService productsService,
            ISuppliersService suppliersService,
            IShoppingCartService shoppingCartService,
            IEmailSender emailSender,
            IViewRenderService viewRenderService)
        {
            this.ordersRepository = ordersRepository;
            this.orderProductsRepository = orderProductsRepository;
            this.productsService = productsService;
            this.suppliersService = suppliersService;
            this.shoppingCartService = shoppingCartService;
            this.emailSender = emailSender;
            this.viewRenderService = viewRenderService;
        }

        public async Task CreateAsync<T>(T model, string userId)
        {
            await this.CancelAnyProcessingOrders(userId);

            var order = AutoMapperConfig.MapperInstance.Map<Order>(model);
            order.DeliveryPrice = this.suppliersService.GetDeliveryPrice(order.SupplierId, order.DeliveryType);
            order.UserId = userId;
            await this.ordersRepository.AddAsync(order);
            await this.ordersRepository.SaveChangesAsync();
        }

        public async Task<string> CompleteOrderAsync(string userId)
        {
            var order = this.GetProcessingOrderByUserId(userId);
            if (order == null)
            {
                return null;
            }

            var shoppingCartProducts = await this.shoppingCartService.GetAllProductsAsync<ShoppingCartProductViewModel>(true, null, userId);
            if (shoppingCartProducts == null || shoppingCartProducts.Count() == 0)
            {
                return null;
            }

            foreach (var shoppingCartProduct in shoppingCartProducts)
            {
                var orderProduct = new OrderProduct
                {
                    Order = order,
                    ProductId = shoppingCartProduct.ProductId,
                    Quantity = shoppingCartProduct.Quantity,
                    Price = shoppingCartProduct.ProductPrice,
                };

                if (!this.OrderHasProduct(order.Id, shoppingCartProduct.ProductId))
                {
                    order.Products.Add(orderProduct);
                }
            }

            order.TotalPrice = order.Products.Sum(x => x.Quantity * x.Price) + order.DeliveryPrice;

            if (order.PaymentType == PaymentType.CashOnDelivery || order.PaymentStatus == PaymentStatus.Paid)
            {
                await this.shoppingCartService.DeleteAllProductsAsync(userId);
                order.Status = OrderStatus.Unprocessed;

                // Send Email With Order Details
                var orderViewModel = this.GetById<OrderViewModel>(order.Id);
                orderViewModel.Status = order.Status;
                orderViewModel.TotalPrice = order.TotalPrice;

                var emailContent = await this.viewRenderService.RenderToStringAsync("/Views/Orders/OrderRegisterEmailModel.cshtml", orderViewModel);
                await this.emailSender.SendEmailAsync("xemart@abv.bg", "XeMart", orderViewModel.Email, "Successfully registered order", emailContent);
            }

            this.ordersRepository.Update(order);
            await this.ordersRepository.SaveChangesAsync();

            return order.Id;
        }

        public async Task<bool> SetOrderStatusAsync(string id, string status)
        {
            var order = this.GetOrderById(id);
            if (order == null)
            {
                return false;
            }

            var statusResult = Enum.TryParse<OrderStatus>(status, out var statusParsed);
            if (!statusResult)
            {
                return false;
            }

            order.Status = statusParsed;
            if (statusParsed == OrderStatus.Delivered)
            {
                order.IsDelivered = true;
                order.DeliveredOn = DateTime.UtcNow;
                order.PaymentStatus = PaymentStatus.Paid;
            }
            else
            {
                order.IsDelivered = false;
                order.DeliveredOn = null;
            }

            this.ordersRepository.Update(order);
            await this.ordersRepository.SaveChangesAsync();

            return true;
        }

        public IEnumerable<T> TakeOrdersByUserId<T>(string userId, int page, int ordersToTake) =>
            this.ordersRepository.AllAsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedOn)
            .Skip((page - 1) * ordersToTake)
            .Take(ordersToTake)
            .To<T>().ToList();

        public IEnumerable<T> TakeOrdersByStatus<T>(OrderStatus status, int page, int ordersToTake) =>
            this.ordersRepository.AllAsNoTracking()
            .Where(x => x.Status == status)
            .OrderByDescending(x => x.CreatedOn)
            .Skip((page - 1) * ordersToTake)
            .Take(ordersToTake)
            .To<T>().ToList();

        public IEnumerable<T> TakeProcessingAndUnprocessedOrders<T>(int page, int ordersToTake) =>
            this.ordersRepository.AllAsNoTracking()
            .Where(x => x.Status == OrderStatus.Processing || x.Status == OrderStatus.Unprocessed)
            .OrderByDescending(x => x.CreatedOn)
            .Skip((page - 1) * ordersToTake)
            .Take(ordersToTake)
            .To<T>().ToList();

        public IEnumerable<T> TakeDeletedOrders<T>(int page, int ordersToTake) =>
            this.ordersRepository.AllAsNoTrackingWithDeleted()
            .Where(x => x.IsDeleted)
            .OrderByDescending(x => x.DeletedOn)
            .Skip((page - 1) * ordersToTake)
            .Take(ordersToTake)
            .To<T>().ToList();

        public IEnumerable<T> GetMostBoughtProducts<T>(int productsToTake)
        {
            var productIds = this.orderProductsRepository.AllAsNoTracking()
                .Where(x => x.Order.Status == OrderStatus.Delivered)
                .GroupBy(x => x.ProductId)
                .Select(x => new { ProductId = x.Key, Total = x.Count() })
                .OrderByDescending(x => x.Total)
                .Take(productsToTake)
                .ToList();

            var products = new List<T>();
            foreach (var product in productIds)
            {
                var mappedProduct = this.productsService.GetById<T>(product.ProductId);
                products.Add(mappedProduct);
            }

            return products;
        }

        public int GetOrdersCountByUserId(string userId) =>
            this.ordersRepository.AllAsNoTracking()
            .Count(x => x.UserId == userId);

        public int GetOrdersCountByStatus(OrderStatus status) =>
            this.ordersRepository.AllAsNoTracking()
            .Count(x => x.Status == status);

        public int GetDeletedOrdersCount() =>
            this.ordersRepository.AllAsNoTrackingWithDeleted()
            .Count(x => x.IsDeleted);

        public T GetById<T>(string id) =>
            this.ordersRepository.AllAsNoTracking()
            .Where(x => x.Id == id)
            .To<T>().FirstOrDefault();

        public Order GetProcessingOrderByUserId(string userId) =>
            this.ordersRepository.All().Include(x => x.Products)
            .FirstOrDefault(x => x.UserId == userId && x.Status == OrderStatus.Processing);

        public PaymentType GetPaymentTypeById(string id) =>
            this.ordersRepository.AllAsNoTracking()
            .FirstOrDefault(x => x.Id == id)
            .PaymentType;

        public bool UserHasOrder(string userId, string orderId) =>
            this.ordersRepository.AllAsNoTracking()
            .Any(x => x.UserId == userId && x.Id == orderId);

        public async Task FulfillOrderById(string orderId, string stripeId)
        {
            var order = this.GetOrderById(orderId);

            order.PaymentStatus = PaymentStatus.Paid;
            order.StripeId = stripeId;

            this.ordersRepository.Update(order);
            await this.ordersRepository.SaveChangesAsync();
        }

        public async Task CancelAnyProcessingOrders(string userId)
        {
            var order = this.GetProcessingOrderByUserId(userId);
            if (order != null)
            {
                order.Status = OrderStatus.Cancelled;
                this.ordersRepository.Update(order);
                await this.ordersRepository.SaveChangesAsync();
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var order = this.GetOrderById(id);
            if (order == null)
            {
                return false;
            }

            this.ordersRepository.Delete(order);
            await this.ordersRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UndeleteAsync(string id)
        {
            var order = this.GetDeletedOrderById(id);
            if (order == null)
            {
                return false;
            }

            this.ordersRepository.Undelete(order);
            await this.ordersRepository.SaveChangesAsync();

            return true;
        }

        private Order GetOrderById(string id) =>
            this.ordersRepository.All()
            .FirstOrDefault(x => x.Id == id);

        private Order GetDeletedOrderById(string id) =>
            this.ordersRepository.AllAsNoTrackingWithDeleted()
            .Where(x => x.IsDeleted && x.Id == id)
            .FirstOrDefault();

        private bool OrderHasProduct(string orderId, string productId) =>
            this.ordersRepository.AllAsNoTracking()
            .Any(x => x.Id == orderId && x.Products.Any(x => x.ProductId == productId));
    }
}
