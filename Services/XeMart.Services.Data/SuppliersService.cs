namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using XeMart.Data.Common.Repositories;
    using XeMart.Data.Models;
    using XeMart.Data.Models.Enums;
    using XeMart.Services.Mapping;

    public class SuppliersService : ISuppliersService
    {
        private readonly IDeletableEntityRepository<Supplier> suppliersRepository;

        public SuppliersService(IDeletableEntityRepository<Supplier> suppliersRepository)
        {
            this.suppliersRepository = suppliersRepository;
        }

        public async Task CreateAsync<T>(T model)
        {
            var supplier = AutoMapperConfig.MapperInstance.Map<Supplier>(model);
            if (this.GetDefaultSupplier() == null)
            {
                supplier.IsDefault = true;
            }

            await this.suppliersRepository.AddAsync(supplier);
            await this.suppliersRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll<T>() =>
            this.suppliersRepository.AllAsNoTracking()
            .To<T>().ToList();

        public IEnumerable<T> GetAllDeleted<T>() =>
            this.suppliersRepository.AllAsNoTrackingWithDeleted()
            .Where(x => x.IsDeleted)
            .To<T>().ToList();

        public async Task<bool> MakeDafaultAsync(int id)
        {
            var newDefaultSupplier = this.GetById(id);
            if (newDefaultSupplier == null)
            {
                return false;
            }

            var oldDefaultSupplier = this.GetDefaultSupplier();
            if (oldDefaultSupplier != null)
            {
                oldDefaultSupplier.IsDefault = false;
                this.suppliersRepository.Update(oldDefaultSupplier);
            }

            newDefaultSupplier.IsDefault = true;

            this.suppliersRepository.Update(newDefaultSupplier);
            await this.suppliersRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditAsync<T>(T model)
        {
            var newSupplier = AutoMapperConfig.MapperInstance.Map<Supplier>(model);

            var foundSupplier = this.GetById(newSupplier.Id);
            if (foundSupplier == null)
            {
                return false;
            }

            foundSupplier.Name = newSupplier.Name;
            foundSupplier.PriceToHome = newSupplier.PriceToHome;
            foundSupplier.PriceToOffice = newSupplier.PriceToOffice;

            this.suppliersRepository.Update(foundSupplier);
            await this.suppliersRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var supplier = this.GetById(id);
            if (supplier == null || supplier.IsDefault)
            {
                return false;
            }

            this.suppliersRepository.Delete(supplier);
            await this.suppliersRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UndeleteAsync(int id)
        {
            var supplier = this.GetDeletedById(id);
            if (supplier == null)
            {
                return false;
            }

            this.suppliersRepository.Undelete(supplier);
            await this.suppliersRepository.SaveChangesAsync();

            return true;
        }

        public T GetById<T>(int id) =>
            this.suppliersRepository.AllAsNoTracking()
            .Where(x => x.Id == id)
            .To<T>()
            .FirstOrDefault();

        public decimal GetDeliveryPrice(int id, DeliveryType deliveryType)
        {
            var supplier = this.suppliersRepository.AllAsNoTracking()
                .FirstOrDefault(x => x.Id == id);

            if (deliveryType == DeliveryType.Office)
            {
                return supplier.PriceToOffice;
            }

            return supplier.PriceToHome;
        }

        private Supplier GetById(int id) =>
            this.suppliersRepository.AllAsNoTracking()
            .FirstOrDefault(x => x.Id == id);

        private Supplier GetDefaultSupplier() =>
            this.suppliersRepository.AllAsNoTracking()
            .FirstOrDefault(x => x.IsDefault);

        private Supplier GetDeletedById(int id) =>
            this.suppliersRepository.AllAsNoTrackingWithDeleted()
            .Where(x => x.IsDeleted && x.Id == id)
            .FirstOrDefault();
    }
}
