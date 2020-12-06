namespace XeMart.Web.ViewModels.Orders
{
    using System.Collections.Generic;

    public class OrderListViewModel : BasePagingViewModel
    {
        public IEnumerable<OrderSummaryViewModel> Orders { get; set; }
    }
}
