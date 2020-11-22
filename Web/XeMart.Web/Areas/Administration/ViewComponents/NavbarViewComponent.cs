namespace XeMart.Web.Areas.Administration.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;

    using XeMart.Services;
    using XeMart.Services.Data;
    using XeMart.Web.ViewModels.Administration.UserMessages;
    using XeMart.Web.ViewModels.Administration.ViewComponents;

    public class NavbarViewComponent : ViewComponent
    {
        private readonly IPartnersService partnersService;
        private readonly IUserMessagesService userMessagesService;
        private readonly ITimeSpanService timeSpanService;

        public NavbarViewComponent(
            IPartnersService partnersService,
            IUserMessagesService userMessagesService,
            ITimeSpanService timeSpanService)
        {
            this.partnersService = partnersService;
            this.userMessagesService = userMessagesService;
            this.timeSpanService = timeSpanService;
        }

        public IViewComponentResult Invoke()
        {
            var partnerRequestsCount = this.partnersService.GetRequestsCount();
            var unreadUserMessages = this.userMessagesService.GetUnreadMessages<UserMessageNavbarViewModel>();

            foreach (var unreadMessage in unreadUserMessages)
            {
                unreadMessage.TimePassedSinceSubmission = this.timeSpanService.GetTimeSince(unreadMessage.CreatedOn);
            }

            var viewModel = new NavbarViewModel
            {
                PartnerRequestsCount = partnerRequestsCount,
                UnreadUserMessages = unreadUserMessages,
            };

            return this.View(viewModel);
        }
    }
}
