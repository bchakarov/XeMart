namespace XeMart.Web.ViewModels.Administration.UserMessages
{
    using System.Globalization;

    using AutoMapper;

    using XeMart.Common;
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class DeletedUserMessagesViewModel : IMapFrom<UserMessage>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string CreatedOn { get; set; }

        public string Subject { get; set; }

        public string Email { get; set; }

        public string Message { get; set; }

        public string IP { get; set; }

        public bool IsRead { get; set; }

        public string DeletedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<UserMessage, DeletedUserMessagesViewModel>()
            .ForMember(
                source => source.CreatedOn,
                destination => destination.MapFrom(member => member.CreatedOn.ToString(GlobalConstants.DateTimeFormat, CultureInfo.InvariantCulture)))
            .ForMember(
                source => source.DeletedOn,
                destination => destination.MapFrom(member => member.DeletedOn.Value.ToString(GlobalConstants.DateTimeFormat, CultureInfo.InvariantCulture)));
        }
    }
}
