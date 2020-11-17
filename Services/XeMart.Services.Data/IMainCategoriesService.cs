namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using XeMart.Data.Models;

    public interface IMainCategoriesService
    {
        public Task Create(MainCategory supplier);

        public IEnumerable<MainCategory> All();

        public Task Edit(int id, string name, string fontAwesomeIcon, string imageUrl);

        public Task<bool> Delete(int id);

        public MainCategory GetById(int id);
    }
}
