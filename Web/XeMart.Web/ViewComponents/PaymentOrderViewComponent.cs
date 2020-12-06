namespace XeMart.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;

    using XeMart.Services.Data;
    using XeMart.Web.ViewModels.Payment;

    public class PaymentOrderViewComponent : ViewComponent
    {
        private readonly IOrdersService ordersService;

        public PaymentOrderViewComponent(IOrdersService ordersService)
        {
            this.ordersService = ordersService;
        }

        public IViewComponentResult Invoke(string orderId)
        {
            var paymentType = this.ordersService.GetPaymentTypeById(orderId);

            var viewModel = new PaymentViewModel
            {
                PaymentType = paymentType,
            };

            return this.View(viewModel);
        }
    }
}
