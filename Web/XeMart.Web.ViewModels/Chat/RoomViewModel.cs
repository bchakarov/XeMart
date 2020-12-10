namespace XeMart.Web.ViewModels.Chat
{
    using System.Globalization;
    using System.Linq;

    using AutoMapper;

    using XeMart.Common;
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class RoomViewModel : IMapFrom<ChatRoom>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string CreatedOn { get; set; }

        public string OwnerUsername { get; set; }

        public string LastMessage { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ChatRoom, RoomViewModel>()
           .ForMember(
               source => source.CreatedOn,
               destination => destination.MapFrom(member => member.CreatedOn.ToString(GlobalConstants.DateTimeFormat, CultureInfo.InvariantCulture)))
           .ForMember(
               source => source.LastMessage,
               destination => destination.MapFrom(member => (!member.Messages.Any()) ? null : member.Messages.OrderByDescending(x => x.CreatedOn).First().Message));
        }
    }
}
