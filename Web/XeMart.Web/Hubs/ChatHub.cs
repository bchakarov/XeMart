namespace XeMart.Web.Hubs
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.SignalR;

    using XeMart.Common;
    using XeMart.Data.Models;
    using XeMart.Services.Data;
    using XeMart.Web.ViewModels.Chat;

    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IChatService chatService;
        private readonly UserManager<ApplicationUser> userManager;

        public ChatHub(
            IChatService chatService,
            UserManager<ApplicationUser> userManager)
        {
            this.chatService = chatService;
            this.userManager = userManager;
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task LoadRooms()
        {
            var rooms = this.chatService.GetAllRooms<RoomViewModel>();
            await this.Clients.Caller.SendAsync("NewRoom", rooms);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task LoadRoomMessages(string roomId)
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, roomId);

            var messages = this.chatService.GetAllMessagesByRoomId<MessageViewModel>(roomId);
            await this.Clients.Caller.SendAsync("NewMessage", messages);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task SendToRoom(string roomId, string message)
        {
            var userId = this.Context.UserIdentifier;
            var newMessage = await this.chatService.AddMessageAsync<MessageViewModel>(roomId, message, userId);
            await this.Clients.Group(roomId).SendAsync("NewMessage", newMessage);
        }

        public async Task LoadMessages()
        {
            var userId = this.Context.UserIdentifier;
            var roomId = this.chatService.GetRoomIdByOwnerId(userId);
            if (roomId == null)
            {
                return;
            }

            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, roomId);

            var messages = this.chatService.GetAllMessagesByRoomId<MessageViewModel>(roomId);
            await this.Clients.Caller.SendAsync("NewMessage", messages);
        }

        public async Task Send(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            var userId = this.Context.UserIdentifier;
            var roomId = this.chatService.GetRoomIdByOwnerId(userId);

            if (roomId == null)
            {
                var room = await this.chatService.CreateRoomAsync<RoomViewModel>(userId);
                roomId = room.Id;
                await this.Groups.AddToGroupAsync(this.Context.ConnectionId, roomId);

                room.LastMessage = message;
                var admins = await this.userManager.GetUsersInRoleAsync(GlobalConstants.AdministratorRoleName);
                var adminIds = admins.Select(x => x.Id).ToList().AsReadOnly();
                await this.Clients.Users(adminIds).SendAsync("NewRoom", room);
            }

            var newMessage = await this.chatService.AddMessageAsync<MessageViewModel>(roomId, message, userId);
            await this.Clients.Group(roomId).SendAsync("NewMessage", newMessage);
        }
    }
}
