namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface IPartnersService
    {
        public Task<bool> CreateAsync<T>(T model, string managerId);

        public IEnumerable<T> GetAllApproved<T>();

        public IEnumerable<T> GetAllRequests<T>();

        public int GetRequestsCount();

        public Task<bool> ApproveAsync(int id);

        public Task<bool> UnapproveAsync(int id);

        public Task<bool> EditAsync<T>(T model, IFormFile logo);

        public Task<bool> DeleteAsync(int id);

        public T GetById<T>(int id);

        public T GetByManagerId<T>(string managerId);
    }
}
