namespace XeMart.Web.Controllers
{
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Stripe;

    using XeMart.Services.Data;

    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public class StripeWebHookController : BaseController
    {
        public const string Secret = "whsec_aGJ8jtdUEnMcam2vywIOEDm4DtYk4VHg";
        private readonly IOrdersService ordersService;

        public StripeWebHookController(IOrdersService ordersService)
        {
            this.ordersService = ordersService;
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
                  Secret);

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
