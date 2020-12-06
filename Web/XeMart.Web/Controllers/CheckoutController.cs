namespace XeMart.Web.Controllers
{
    using System.Collections.Generic;
    using System.Security.Claims;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using Stripe.Checkout;

    using XeMart.Services.Data;
    using XeMart.Web.ViewModels.Orders;

    [Authorize]
    [Route("/api/checkout")]
    [ApiController]
    public class CheckoutController : BaseController
    {
        private readonly string domain;
        private readonly IOrdersService ordersService;
        private readonly string userId;

        public CheckoutController(
            IHttpContextAccessor contextAccessor,
            IOrdersService ordersService)
        {
            this.domain = $"{contextAccessor.HttpContext.Request.Scheme}://{contextAccessor.HttpContext.Request.Host}";
            this.ordersService = ordersService;
            this.userId = contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [HttpPost]
        public ActionResult Create()
        {
            var orderId = this.ordersService.GetProcessingOrderByUserId(this.userId).Id;
            var order = this.ordersService.GetById<OrderViewModel>(orderId);

            var items = new List<SessionLineItemOptions>();
            foreach (var product in order.Products)
            {
                items.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long?)(product.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = product.ProductName,
                            Images = new List<string> { this.domain + product.ImageUrl },
                        },
                    },
                    Quantity = product.Quantity,
                });
            }

            // Add shipping price
            items.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long?)(order.DeliveryPrice * 100),
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Shipping",
                    },
                },
                Quantity = 1,
            });

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                  "card",
                },
                LineItems = items,
                Mode = "payment",
                SuccessUrl = this.domain + "/Orders/Complete",
                CancelUrl = this.domain + "/Orders/Create",
                Metadata = new Dictionary<string, string> { { "order_id", orderId } },
            };
            var service = new SessionService();
            Session session = service.Create(options);
            return this.Json(new { id = session.Id });
        }
    }
}
