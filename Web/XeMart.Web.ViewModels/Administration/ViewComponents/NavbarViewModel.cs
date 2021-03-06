﻿namespace XeMart.Web.ViewModels.Administration.ViewComponents
{
    using System.Collections.Generic;

    using AutoMapper;

    using XeMart.Web.ViewModels.Administration.UserMessages;

    public class NavbarViewModel
    {
        public NavbarViewModel()
        {
            this.MaxMessagesToDisplay = 5;
        }

        public int UnprocessedOrdersCount { get; set; }

        public int PartnerRequestsCount { get; set; }

        public IEnumerable<UserMessageNavbarViewModel> UnreadUserMessages { get; set; }

        [IgnoreMap]
        public int MaxMessagesToDisplay { get; set; }
    }
}
