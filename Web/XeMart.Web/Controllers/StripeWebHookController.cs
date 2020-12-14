namespace XeMart.Web.Controllers
{
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    using Stripe;

    using XeMart.Services.Data;

    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public class StripeWebHookController : BaseController
    {
        private readonly IOrdersService ordersService;
        private readonly IConfiguration configuration;

        public StripeWebHookController(IOrdersService ordersService, IConfiguration configuration)
        {
            this.ordersService = ordersService;
            this.configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(this.HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                  json,
                  this.Request.Headers["Stripe-Signature"],
                  this.configuration["Stripe:WebHookKey"]);

                // Handle the checkout.session.completed event
                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;

                    // Fulfill the purchase...
                    if (session.PaymentStatus == "paid")
                    {
                        await this.ordersService.FulfillOrderById(session.Metadata["order_id"], session.PaymentIntentId);
                    }
                }

                return this.Ok();
            }
            catch (StripeException)
            {
                return this.BadRequest();
            }
        }
    }
}
