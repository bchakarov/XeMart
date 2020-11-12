namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using XeMart.Data.Models;

    public interface IUserMessagesService
    {
        public Task Add(UserMessage userMessage);

        public IEnumerable<UserMessage> All();

        public IEnumerable<UserMessage> AllWithDeleted();

        public Task SetIsRead(string id, bool isRead);

        public Task Delete(string id);

        public Task Undelete(string id);

        public UserMessage GetById(string id);
    }
}
