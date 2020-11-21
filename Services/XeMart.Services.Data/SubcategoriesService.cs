namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using XeMart.Data.Common.Repositories;
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class SubcategoriesService : ISubcategoriesService
    {
        private readonly IDeletableEntityRepository<Subcategory> subcategoriesRepository;

        public SubcategoriesService(IDeletableEntityRepository<Subcategory> subcategoriesRepository)
        {
            this.subcategoriesRepository = subcategoriesRepository;
        }

        public async Task CreateAsync<T>(T model)
        {
            var subcategory = AutoMapperConfig.MapperInstance.Map<Subcategory>(model);
            await this.subcategoriesRepository.AddAsync(subcategory);
            await this.subcategoriesRepository.SaveChangesAsync();
        }

        public IEnumerable<T> All<T>() =>
            this.subcategoriesRepository.AllAsNoTracking()
            .To<T>().ToList();

        public IEnumerable<Subcategory> All() =>
            this.subcategoriesRepository.AllAsNoTracking()
            .ToList();

        public async Task<bool> EditAsync<T>(T model)
        {
            var subcategory = AutoMapperConfig.MapperInstance.Map<Subcategory>(model);
            if (this.GetById(subcategory.Id) == null)
            {
                return false;
            }

            this.subcategoriesRepository.Update(subcategory);
            await this.subcategoriesRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var subcategory = this.GetById(id);
            if (subcategory == null || subcategory.Products.Any())
            {
                return false;
            }

            this.subcategoriesRepository.Delete(subcategory);
            await this.subcategoriesRepository.SaveChangesAsync();

            return true;
        }

        public T GetById<T>(int id) =>
             this.subcategoriesRepository.AllAsNoTracking()
            .Where(x => x.Id == id)
            .To<T>()
            .FirstOrDefault();

        private Subcategory GetById(int id) =>
             this.subcategoriesRepository.AllAsNoTracking().Include(x => x.Products)
            .FirstOrDefault(x => x.Id == id);
    }
}
