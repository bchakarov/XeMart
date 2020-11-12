namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using XeMart.Data.Common.Repositories;
    using XeMart.Data.Models;

    public class SuppliersService : ISuppliersService
    {
        private readonly IDeletableEntityRepository<Supplier> suppliersRepository;

        public SuppliersService(IDeletableEntityRepository<Supplier> suppliersRepository)
        {
            this.suppliersRepository = suppliersRepository;
        }

        public async Task Create(Supplier supplier)
        {
            if (!this.All().Any(x => x.IsDefault))
            {
                supplier.IsDefault = true;
            }

            await this.suppliersRepository.AddAsync(supplier);
            await this.suppliersRepository.SaveChangesAsync();
        }

        public IEnumerable<Supplier> All() =>
            this.suppliersRepository.All();

        public async Task<bool> MakeDafault(int id)
        {
            var newDefaultSupplier = this.GetById(id);

            if (newDefaultSupplier == null)
            {
                return false;
            }

            var oldDefaultSupplier = this.All().FirstOrDefault(x => x.IsDefault == true);
            if (oldDefaultSupplier != null)
            {
                oldDefaultSupplier.IsDefault = false;
            }

            newDefaultSupplier.IsDefault = true;
            await this.suppliersRepository.SaveChangesAsync();
            return true;
        }

        public async Task Edit(int id, string name, decimal priceToHome, decimal priceToOffice)
        {
            var supplier = this.GetById(id);

            if (supplier == null)
            {
                return;
            }

            supplier.Name = name;
            supplier.PriceToHome = priceToHome;
            supplier.PriceToOffice = priceToOffice;

            await this.suppliersRepository.SaveChangesAsync();
        }

        public async Task<bool> Delete(int id)
        {
            var supplier = this.GetById(id);
            if (supplier == null)
            {
                return false;
            }

            this.suppliersRepository.Delete(supplier);
            await this.suppliersRepository.SaveChangesAsync();
            return true;
        }

        public Supplier GetById(int id) =>
            this.suppliersRepository.All().FirstOrDefault(x => x.Id == id);

        public Supplier GetDefaultSupplier() =>
            this.suppliersRepository.All().FirstOrDefault(x => x.IsDefault);
    }
}
