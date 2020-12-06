namespace XeMart.Web.Areas.Administration.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;

    using XeMart.Data.Models.Enums;
    using XeMart.Services;
    using XeMart.Services.Data;
    using XeMart.Web.ViewModels.Administration.UserMessages;
    using XeMart.Web.ViewModels.Administration.ViewComponents;

    public class NavbarViewComponent : ViewComponent
    {
        private readonly IOrdersService ordersService;
        private readonly IPartnersService partnersService;
        private readonly IUserMessagesService userMessagesService;
        private readonly ITimeSpanService timeSpanService;

        public NavbarViewComponent(
            IOrdersService ordersService,
            IPartnersService partnersService,
            IUserMessagesService userMessagesService,
            ITimeSpanService timeSpanService)
        {
            this.ordersService = ordersService;
            this.partnersService = partnersService;
            this.userMessagesService = userMessagesService;
            this.timeSpanService = timeSpanService;
        }

        public IViewComponentResult Invoke()
        {
            var unprocessedOrdersCount = this.ordersService.GetOrdersCountByStatus(OrderStatus.Unprocessed) + this.ordersService.GetOrdersCountByStatus(OrderStatus.Processing);
            var partnerRequestsCount = this.partnersService.GetRequestsCount();
            var unreadUserMessages = this.userMessagesService.GetUnreadMessages<UserMessageNavbarViewModel>();

            foreach (var unreadMessage in unreadUserMessages)
            {
                unreadMessage.TimePassedSinceSubmission = this.timeSpanService.GetTimeSince(unreadMessage.CreatedOn);
            }

            var viewModel = new NavbarViewModel
            {
                UnprocessedOrdersCount = unprocessedOrdersCount,
                PartnerRequestsCount = partnerRequestsCount,
                UnreadUserMessages = unreadUserMessages,
            };

            return this.View(viewModel);
        }
    }
}
