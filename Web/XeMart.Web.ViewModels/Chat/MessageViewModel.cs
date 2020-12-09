namespace XeMart.Web.ViewModels.Chat
{
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class MessageViewModel : IMapFrom<RoomMessage>
    {
        public string Message { get; set; }
    }
}
