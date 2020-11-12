namespace XeMart.Web.ViewModels.Administration.UserMessages
{
    using System.Collections.Generic;

    public class IndexUserMessageViewModel
    {
        public IEnumerable<UserMessageViewModel> UserMessageViewModelCollection { get; set; }

        public UserMessageViewModel UserMessageViewModel { get; set; }
    }
}
