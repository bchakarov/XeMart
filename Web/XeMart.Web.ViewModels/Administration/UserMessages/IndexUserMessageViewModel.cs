namespace XeMart.Web.ViewModels.Administration.UserMessages
{
    using System.Collections.Generic;

    public class IndexUserMessageViewModel<T>
    {
        public IEnumerable<T> UserMessageViewModelCollection { get; set; }

        public T UserMessageViewModel { get; set; }
    }
}
