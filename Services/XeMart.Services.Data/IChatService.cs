namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IChatService
    {
        public Task<T> CreateRoomAsync<T>(string userId);

        public Task<T> AddMessageAsync<T>(string roomId, string message, string senderId);

        public IEnumerable<T> GetAllRooms<T>();

        public IEnumerable<T> GetAllMessagesByRoomId<T>(string roomId);

        public string GetRoomIdByOwnerId(string ownerId);
    }
}
