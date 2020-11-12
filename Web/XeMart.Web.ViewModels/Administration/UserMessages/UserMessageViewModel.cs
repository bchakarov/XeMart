namespace XeMart.Web.ViewModels.Administration.UserMessages
{
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class UserMessageViewModel : IMapFrom<UserMessage>
    {
        public string Id { get; set; }

        public string CreatedOn { get; set; }

        public string Subject { get; set; }

        public string Email { get; set; }

        public string Message { get; set; }

        public string IP { get; set; }

        public bool IsRead { get; set; }
    }
}
