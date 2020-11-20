namespace XeMart.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using XeMart.Data.Common.Repositories;
    using XeMart.Data.Models;

    public class SubcategoriesService : ISubcategoriesService
    {
        private readonly IDeletableEntityRepository<Subcategory> subcategoriesRepository;

        public SubcategoriesService(IDeletableEntityRepository<Subcategory> subcategoriesRepository)
        {
            this.subcategoriesRepository = subcategoriesRepository;
        }

        public async Task CreateAsync(Subcategory subcategory)
        {
            await this.subcategoriesRepository.AddAsync(subcategory);
            await this.subcategoriesRepository.SaveChangesAsync();
        }

        public IEnumerable<Subcategory> All() =>
            this.subcategoriesRepository.All().ToList();

        public async Task EditAsync(int id)
        {
            throw new NotImplementedException();
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

        public Subcategory GetById(int id) =>
             this.subcategoriesRepository.All().FirstOrDefault(x => x.Id == id);
    }
}
