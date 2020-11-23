namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISuppliersService
    {
        public Task CreateAsync<T>(T supplier);

        public IEnumerable<T> GetAll<T>();

        public Task<bool> MakeDafaultAsync(int id);

        public Task<bool> EditAsync<T>(T model);

        public Task<bool> DeleteAsync(int id);

        public T GetById<T>(int id);
    }
}
