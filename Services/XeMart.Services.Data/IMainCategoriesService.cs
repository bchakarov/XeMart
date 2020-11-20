namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using XeMart.Data.Models;

    public interface IMainCategoriesService
    {
        public Task CreateAsync(MainCategory supplier);

        public IEnumerable<MainCategory> All();

        public Task EditAsync(int id, string name, string fontAwesomeIcon, string imageUrl);

        public Task<bool> DeleteAsync(int id);

        public MainCategory GetById(int id);
    }
}
