namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using XeMart.Data.Models;

    public interface ISuppliersService
    {
        public Task Create(Supplier supplier);

        public IEnumerable<Supplier> All();

        public Task<bool> MakeDafault(int id);

        public Task Edit(int id, string name, decimal priceToHome, decimal priceToOffice);

        public Task<bool> Delete(int id);

        public Supplier GetById(int id);

        public Supplier GetDefaultSupplier();
    }
}
