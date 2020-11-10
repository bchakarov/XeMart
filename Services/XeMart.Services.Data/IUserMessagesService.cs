namespace XeMart.Services.Data
{
    using System.Threading.Tasks;

    using XeMart.Data.Models;

    public interface IUserMessagesService
    {
        public Task Add(UserMessage userMessage);

        public Task Delete(UserMessage userMessage);

        public Task SetIsRead(string id, bool isRead);
    }
}
