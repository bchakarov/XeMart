namespace XeMart.Web.ViewModels.Orders
{
    using System.Collections.Generic;

    public class OrderHistoryViewModel : BasePagingViewModel
    {
        public IEnumerable<OrderSummaryViewModel> Orders { get; set; }
    }
}
