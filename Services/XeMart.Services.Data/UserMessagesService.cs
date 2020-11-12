namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using XeMart.Data.Common.Repositories;
    using XeMart.Data.Models;

    public class UserMessagesService : IUserMessagesService
    {
        private readonly IDeletableEntityRepository<UserMessage> userMessagesRepository;

        public UserMessagesService(IDeletableEntityRepository<UserMessage> userMessagesRepository)
        {
            this.userMessagesRepository = userMessagesRepository;
        }

        public async Task Add(UserMessage userMessage)
        {
            await this.userMessagesRepository.AddAsync(userMessage);
            await this.userMessagesRepository.SaveChangesAsync();
        }

        public IEnumerable<UserMessage> All() =>
            this.userMessagesRepository.AllAsNoTracking();

        public async Task SetIsRead(string id, bool isRead)
        {
            var userMessage = this.GetById(id);
            if (userMessage == null)
            {
                return;
            }

            userMessage.IsRead = isRead;
            await this.userMessagesRepository.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            var userMessage = this.GetById(id);
            if (userMessage == null)
            {
                return;
            }

            this.userMessagesRepository.Delete(userMessage);
            await this.userMessagesRepository.SaveChangesAsync();
        }

        public UserMessage GetById(string id) =>
            this.userMessagesRepository.All().FirstOrDefault(m => m.Id == id);
    }
}
