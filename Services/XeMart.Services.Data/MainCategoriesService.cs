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

        public async Task CreateAsync<T>(T model, IFormFile image, string imagePath, string webRootPath)
        {
            var mainCategory = AutoMapperConfig.MapperInstance.Map<MainCategory>(model);

            if (image != null)
            {
                imagePath += image.FileName;
                var imageUrl = await this.imagesService.UploadLocalImageAsync(image, imagePath);
                mainCategory.ImageUrl = imageUrl.Replace(webRootPath, string.Empty).Replace("\\", "/");
            }

            await this.mainCategoriesRepository.AddAsync(mainCategory);
            await this.mainCategoriesRepository.SaveChangesAsync();
        }

        public IEnumerable<T> All<T>() =>
            this.mainCategoriesRepository.AllAsNoTracking()
            .To<T>().ToList();

        public IEnumerable<MainCategory> All() =>
            this.mainCategoriesRepository.AllAsNoTracking()
            .ToList();

        public async Task<bool> EditAsync<T>(T model, IFormFile image, string imagePath, string webRootPath)
        {
            var mainCategory = AutoMapperConfig.MapperInstance.Map<MainCategory>(model);
            if (this.GetById(mainCategory.Id) == null)
            {
                return false;
            }

            if (image != null)
            {
                imagePath += image.FileName;
                var imageUrl = await this.imagesService.UploadLocalImageAsync(image, imagePath);
                mainCategory.ImageUrl = imageUrl.Replace(webRootPath, string.Empty).Replace("\\", "/");
            }

            this.mainCategoriesRepository.Update(mainCategory);
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

        public T GetById<T>(int id) =>
             this.mainCategoriesRepository.AllAsNoTracking()
            .Where(x => x.Id == id)
            .To<T>()
            .FirstOrDefault();

        private MainCategory GetById(int id) =>
             this.mainCategoriesRepository.AllAsNoTracking().Include(x => x.Subcategories)
            .FirstOrDefault(x => x.Id == id);
    }
}
