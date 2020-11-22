namespace XeMart.Web.ViewModels.Administration.UserMessages
{
    using System;

    using AutoMapper;

    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class UserMessageNavbarViewModel : IMapFrom<UserMessage>
    {
        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Subject { get; set; }

        public string Email { get; set; }

        [IgnoreMap]
        public string TimePassedSinceSubmission { get; set; }
    }
}
