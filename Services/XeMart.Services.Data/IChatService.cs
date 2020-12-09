namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IChatService
    {
        public Task<string> CreateOrGetRoomAsync(string userId);

        public Task AddMessageAsync(string roomId, string message, string senderId);

        public IEnumerable<T> GetAllMessagesByRoomId<T>(string roomId);
    }
}
