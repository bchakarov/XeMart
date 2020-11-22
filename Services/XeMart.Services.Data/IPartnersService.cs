namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPartnersService
    {
        public Task<bool> CreateAsync<T>(T model, string managerId);

        public IEnumerable<T> AllApproved<T>();

        public Task<bool> EditAsync<T>(T model);

        public Task<bool> DeleteAsync(int id);

        public T GetByManagerId<T>(string managerId);
    }
}
