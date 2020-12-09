namespace XeMart.Web.ViewModels.Orders
{
    using System.Collections.Generic;

    public class OrderListViewModel : PagingViewModel
    {
        public IEnumerable<OrderSummaryViewModel> Orders { get; set; }
    }
}
