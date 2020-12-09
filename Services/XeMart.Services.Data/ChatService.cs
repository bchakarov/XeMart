namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using XeMart.Data.Common.Repositories;
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class ChatService : IChatService
    {
        private readonly IDeletableEntityRepository<ChatRoom> chatRoomsRepository;
        private readonly IRepository<RoomMessage> roomMessagesRepository;

        public ChatService(
            IDeletableEntityRepository<ChatRoom> chatRoomsRepository,
            IRepository<RoomMessage> roomMessagesRepository)
        {
            this.chatRoomsRepository = chatRoomsRepository;
            this.roomMessagesRepository = roomMessagesRepository;
        }

        public async Task<string> CreateOrGetRoomAsync(string userId)
        {
            var room = this.GetByOwnerId(userId);
            if (room == null)
            {
                var newRoom = new ChatRoom { OwnerId = userId };
                await this.chatRoomsRepository.AddAsync(newRoom);
                await this.chatRoomsRepository.SaveChangesAsync();
                return newRoom.Id;
            }

            return room.Id;
        }

        public async Task AddMessageAsync(string roomId, string message, string senderId)
        {
            var room = this.GetRoomById(roomId);
            if (room == null)
            {
                return;
            }

            var roomMessage = new RoomMessage
            {
                Message = message,
                SenderId = senderId,
            };

            room.Messages.Add(roomMessage);
            await this.chatRoomsRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllMessagesByRoomId<T>(string roomId) =>
            this.roomMessagesRepository.AllAsNoTracking()
            .Where(x => x.RoomId == roomId)
            .To<T>().ToList();

        private ChatRoom GetByOwnerId(string ownerId) =>
            this.chatRoomsRepository.AllAsNoTracking()
            .FirstOrDefault(x => x.OwnerId == ownerId);

        private ChatRoom GetRoomById(string roomId) =>
            this.chatRoomsRepository.All()
            .FirstOrDefault(x => x.Id == roomId);
    }
}
