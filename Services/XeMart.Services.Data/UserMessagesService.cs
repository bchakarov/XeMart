namespace XeMart.Services.Data
{
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

        public async Task Delete(UserMessage userMessage)
        {
            this.userMessagesRepository.Delete(userMessage);
            await this.userMessagesRepository.SaveChangesAsync();
        }

        public async Task SetIsRead(string id, bool isRead)
        {
            var userMessage = await this.userMessagesRepository.GetByIdWithDeletedAsync(id);
            userMessage.IsRead = isRead;
            await this.userMessagesRepository.SaveChangesAsync();
        }
    }
}
