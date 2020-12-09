namespace XeMart.Web.Hubs
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    using XeMart.Data.Models;
    using XeMart.Services.Data;
    using XeMart.Web.ViewModels.Chat;

    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IChatService chatService;

        public ChatHub(IChatService chatService)
        {
            this.chatService = chatService;
        }

        public async Task LoadMessages()
        {
            var userId = this.Context.UserIdentifier;
            var roomId = await this.chatService.CreateOrGetRoomAsync(userId);
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, roomId);

            var messages = this.chatService.GetAllMessagesByRoomId<MessageViewModel>(roomId);
            await this.Clients.Caller.SendAsync("NewMessage", messages);
        }

        public async Task Send(string message)
        {
            var userId = this.Context.UserIdentifier;
            var roomId = await this.chatService.CreateOrGetRoomAsync(userId);

            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, roomId);
            await this.Clients.Group(roomId).SendAsync(
                "NewMessage",
                new MessageViewModel { Message = message, });

            await this.chatService.AddMessageAsync(roomId, message, userId);
        }
    }
}
