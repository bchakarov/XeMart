namespace XeMart.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;

    using XeMart.Services;
    using XeMart.Services.Data;
    using XeMart.Web.ViewModels.Orders;

    public class OrderDetailsViewComponent : ViewComponent
    {
        private readonly IOrdersService ordersService;
        private readonly IStringService stringService;

        public OrderDetailsViewComponent(
            IOrdersService ordersService,
            IStringService stringService)
        {
            this.ordersService = ordersService;
            this.stringService = stringService;
        }

        public IViewComponentResult Invoke(string orderId)
        {
            var order = this.ordersService.GetById<OrderViewModel>(orderId);
            order.Id = order.Id.Substring(0, 8);

            foreach (var product in order.Products)
            {
                product.ProductName = this.stringService.TruncateAtWord(product.ProductName, 30);
            }

            return this.View(order);
        }
    }
}
