namespace XeMart.Web.ViewModels.Administration.UserMessages
{
    using System.Globalization;

    using AutoMapper;

    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class UserMessageViewModel : IMapFrom<UserMessage>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string CreatedOn { get; set; }

        public string Subject { get; set; }

        public string Email { get; set; }

        public string Message { get; set; }

        public string IP { get; set; }

        public bool IsRead { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<UserMessage, UserMessageViewModel>()
            .ForMember(
                source => source.CreatedOn,
                destination => destination.MapFrom(member => member.CreatedOn.ToString("f", CultureInfo.InvariantCulture)));
        }
    }
}
