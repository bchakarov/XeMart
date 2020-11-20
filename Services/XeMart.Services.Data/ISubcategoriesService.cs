namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using XeMart.Data.Models;

    public interface ISubcategoriesService
    {
        public Task CreateAsync(Subcategory subcategory);

        public IEnumerable<Subcategory> All();

        public Task EditAsync(int id);

        public Task<bool> DeleteAsync(int id);

        public Subcategory GetById(int id);
    }
}
