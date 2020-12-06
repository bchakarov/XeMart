namespace XeMart.Web.ViewModels.Orders
{
    using XeMart.Data.Models;
    using XeMart.Data.Models.Enums;
    using XeMart.Services.Mapping;

    public class OrderPaymentStatusViewModel : IMapFrom<Order>
    {
        public string Id { get; set; }

        public PaymentStatus PaymentStatus { get; set; }
    }
}
