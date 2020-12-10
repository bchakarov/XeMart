namespace XeMart.Web.ViewModels.Chat
{
    using System.Globalization;

    using AutoMapper;

    using XeMart.Common;
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class MessageViewModel : IMapFrom<RoomMessage>, IHaveCustomMappings
    {
        public string CreatedOn { get; set; }

        public string Message { get; set; }

        public string SenderUsername { get; set; }

        public bool IsByRoomOwner { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<RoomMessage, MessageViewModel>()
            .ForMember(
                source => source.CreatedOn,
                destination => destination.MapFrom(member => member.CreatedOn.ToString(GlobalConstants.DateTimeFormat, CultureInfo.InvariantCulture)))
            .ForMember(
                source => source.IsByRoomOwner,
                destination => destination.MapFrom(member => member.SenderId == member.Room.OwnerId));
        }
    }
}
