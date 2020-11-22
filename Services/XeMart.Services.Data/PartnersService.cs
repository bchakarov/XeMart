namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using XeMart.Data.Common.Repositories;
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class PartnersService : IPartnersService
    {
        private readonly IDeletableEntityRepository<Partner> partnersRepository;

        public PartnersService(IDeletableEntityRepository<Partner> partnersRepository)
        {
            this.partnersRepository = partnersRepository;
        }

        public async Task<bool> CreateAsync<T>(T model, string managerId)
        {
            var partner = AutoMapperConfig.MapperInstance.Map<Partner>(model);
            partner.ManagerId = managerId;

            if (this.partnersRepository.AllAsNoTracking().Any(x => x.ManagerId == partner.ManagerId))
            {
                return false;
            }

            await this.partnersRepository.AddAsync(partner);
            await this.partnersRepository.SaveChangesAsync();

            return true;
        }

        public IEnumerable<T> AllApproved<T>() =>
            this.partnersRepository.AllAsNoTracking()
            .Where(x => x.IsApproved)
            .To<T>().ToList();

        public async Task<bool> EditAsync<T>(T model)
        {
            var partner = AutoMapperConfig.MapperInstance.Map<Partner>(model);
            if (this.GetById(partner.Id) == null)
            {
                return false;
            }

            this.partnersRepository.Update(partner);
            await this.partnersRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var partner = this.GetById(id);
            if (partner == null)
            {
                return false;
            }

            this.partnersRepository.Delete(partner);
            await this.partnersRepository.SaveChangesAsync();

            return true;
        }

        public T GetByManagerId<T>(string managerId) =>
            this.partnersRepository.AllAsNoTracking()
            .Where(x => x.ManagerId == managerId)
            .To<T>()
            .FirstOrDefault();

        private Partner GetById(int id) =>
            this.partnersRepository.AllAsNoTracking()
            .FirstOrDefault(x => x.Id == id);
    }
}
