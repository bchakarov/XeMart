namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using XeMart.Data.Common.Repositories;
    using XeMart.Data.Models;

    public class MainCategoriesService : IMainCategoriesService
    {
        private readonly IDeletableEntityRepository<MainCategory> mainCategoriesRepository;

        public MainCategoriesService(IDeletableEntityRepository<MainCategory> mainCategoriesRepository)
        {
            this.mainCategoriesRepository = mainCategoriesRepository;
        }

        public async Task Create(MainCategory mainCategory)
        {
            await this.mainCategoriesRepository.AddAsync(mainCategory);
            await this.mainCategoriesRepository.SaveChangesAsync();
        }

        public IEnumerable<MainCategory> All() =>
            this.mainCategoriesRepository.All();

        public async Task Edit(int id, string name, string fontAwesomeIcon, string imageUrl)
        {
            var mainCategory = this.GetById(id);

            if (mainCategory == null)
            {
                return;
            }

            mainCategory.Name = name;
            mainCategory.FontAwesomeIcon = fontAwesomeIcon;
            if (!string.IsNullOrWhiteSpace(imageUrl))
            {
                mainCategory.ImageUrl = imageUrl;
            }

            await this.mainCategoriesRepository.SaveChangesAsync();
        }

        public async Task<bool> Delete(int id)
        {
            var mainCategory = this.GetById(id);
            if (mainCategory == null || mainCategory.Subcategories.Any())
            {
                return false;
            }

            this.mainCategoriesRepository.Delete(mainCategory);
            await this.mainCategoriesRepository.SaveChangesAsync();
            return true;
        }

        public MainCategory GetById(int id) =>
             this.mainCategoriesRepository.All().FirstOrDefault(x => x.Id == id);
    }
}
