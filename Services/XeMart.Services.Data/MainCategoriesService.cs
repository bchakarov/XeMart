namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    using XeMart.Data.Common.Repositories;
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class MainCategoriesService : IMainCategoriesService
    {
        private readonly IDeletableEntityRepository<MainCategory> mainCategoriesRepository;
        private readonly IImagesService imagesService;

        public MainCategoriesService(
            IDeletableEntityRepository<MainCategory> mainCategoriesRepository,
            IImagesService imagesService)
        {
            this.mainCategoriesRepository = mainCategoriesRepository;
            this.imagesService = imagesService;
        }

        public async Task CreateAsync<T>(T model, IFormFile image, string fullDirectoryPath, string webRootPath)
        {
            var mainCategory = AutoMapperConfig.MapperInstance.Map<MainCategory>(model);

            if (image != null)
            {
                var imageUrl = await this.imagesService.UploadLocalImageAsync(image, fullDirectoryPath);
                mainCategory.ImageUrl = imageUrl.Replace(webRootPath, string.Empty).Replace("\\", "/");
            }

            await this.mainCategoriesRepository.AddAsync(mainCategory);
            await this.mainCategoriesRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll<T>() =>
            this.mainCategoriesRepository.AllAsNoTracking()
            .To<T>().ToList();

        public IEnumerable<MainCategory> GetAll() =>
            this.mainCategoriesRepository.AllAsNoTracking()
            .ToList();

        public IEnumerable<T> GetAllDeleted<T>() =>
            this.mainCategoriesRepository.AllAsNoTrackingWithDeleted()
            .Where(x => x.IsDeleted)
            .To<T>().ToList();

        public async Task<bool> EditAsync<T>(T model, IFormFile image, string fullDirectoryPath, string webRootPath)
        {
            var newMainCategory = AutoMapperConfig.MapperInstance.Map<MainCategory>(model);

            var foundMainCategory = this.GetById(newMainCategory.Id);
            if (foundMainCategory == null)
            {
                return false;
            }

            foundMainCategory.Name = newMainCategory.Name;
            foundMainCategory.FontAwesomeIcon = newMainCategory.FontAwesomeIcon;

            if (image != null)
            {
                var imageUrl = await this.imagesService.UploadLocalImageAsync(image, fullDirectoryPath);
                foundMainCategory.ImageUrl = imageUrl.Replace(webRootPath, string.Empty).Replace("\\", "/");
            }

            this.mainCategoriesRepository.Update(foundMainCategory);
            await this.mainCategoriesRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
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

        public async Task<bool> UndeleteAsync(int id)
        {
            var mainCategory = this.GetDeletedById(id);
            if (mainCategory == null)
            {
                return false;
            }

            this.mainCategoriesRepository.Undelete(mainCategory);
            await this.mainCategoriesRepository.SaveChangesAsync();

            return true;
        }

        public T GetById<T>(int id) =>
             this.mainCategoriesRepository.AllAsNoTracking()
            .Where(x => x.Id == id)
            .To<T>()
            .FirstOrDefault();

        private MainCategory GetById(int id) =>
             this.mainCategoriesRepository.AllAsNoTracking().Include(x => x.Subcategories)
            .FirstOrDefault(x => x.Id == id);

        private MainCategory GetDeletedById(int id) =>
            this.mainCategoriesRepository.AllAsNoTrackingWithDeleted()
            .Where(x => x.IsDeleted && x.Id == id)
            .FirstOrDefault();
    }
}
