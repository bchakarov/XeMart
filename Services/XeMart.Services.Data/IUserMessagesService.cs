namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserMessagesService
    {
        public Task CreateAsync<T>(T model, string ip);

        public IEnumerable<T> All<T>();

        public IEnumerable<T> AllDeleted<T>();

        public IEnumerable<T> GetUnreadMessages<T>();

        public Task<bool> SetIsReadAsync(string id, bool isRead);

        public Task<bool> DeleteAsync(string id);

        public Task<bool> UndeleteAsync(string id);

        public T GetById<T>(string id);
    }
}
