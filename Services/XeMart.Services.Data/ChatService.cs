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

        public async Task<T> CreateRoomAsync<T>(string userId)
        {
            if (this.GetRoomIdByOwnerId(userId) != null)
            {
                return default;
            }

            var newRoom = new ChatRoom { OwnerId = userId };
            await this.chatRoomsRepository.AddAsync(newRoom);
            await this.chatRoomsRepository.SaveChangesAsync();

            return this.GetRoomById<T>(newRoom.Id);
        }

        public async Task<T> AddMessageAsync<T>(string roomId, string message, string senderId)
        {
            var room = this.GetRoomById(roomId);
            if (room == null)
            {
                return default;
            }

            var roomMessage = new RoomMessage
            {
                Message = message,
                SenderId = senderId,
            };

            room.Messages.Add(roomMessage);
            this.chatRoomsRepository.Update(room);
            await this.chatRoomsRepository.SaveChangesAsync();
            return this.GetMessageById<T>(roomMessage.Id);
        }

        public IEnumerable<T> GetAllRooms<T>() =>
            this.chatRoomsRepository.AllAsNoTracking()
            .To<T>().ToList();

        public IEnumerable<T> GetAllMessagesByRoomId<T>(string roomId) =>
            this.roomMessagesRepository.AllAsNoTracking()
            .Where(x => x.RoomId == roomId)
            .To<T>().ToList();

        public string GetRoomIdByOwnerId(string ownerId) =>
            this.chatRoomsRepository.AllAsNoTracking()
            .FirstOrDefault(x => x.OwnerId == ownerId)?.Id;

        private ChatRoom GetRoomById(string roomId) =>
            this.chatRoomsRepository.All()
            .FirstOrDefault(x => x.Id == roomId);

        private T GetRoomById<T>(string roomId) =>
            this.chatRoomsRepository.AllAsNoTracking()
            .Where(x => x.Id == roomId)
            .To<T>().FirstOrDefault();

        private T GetMessageById<T>(int messageId) =>
            this.roomMessagesRepository.AllAsNoTracking()
            .Where(x => x.Id == messageId)
            .To<T>().FirstOrDefault();
    }
}
