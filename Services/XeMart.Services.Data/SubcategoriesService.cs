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

    public class SubcategoriesService : ISubcategoriesService
    {
        private readonly IDeletableEntityRepository<Subcategory> subcategoriesRepository;
        private readonly IImagesService imagesService;

        public SubcategoriesService(
            IDeletableEntityRepository<Subcategory> subcategoriesRepository,
            IImagesService imagesService)
        {
            this.subcategoriesRepository = subcategoriesRepository;
            this.imagesService = imagesService;
        }

        public async Task CreateAsync<T>(T model, IFormFile image, string directoryPath, string webRootPath)
        {
            var subcategory = AutoMapperConfig.MapperInstance.Map<Subcategory>(model);

            if (image != null)
            {
                var imageUrl = await this.imagesService.UploadLocalImageAsync(image, webRootPath + directoryPath);
                subcategory.ImageUrl = imageUrl.Replace(webRootPath, string.Empty).Replace("\\", "/");
            }

            await this.subcategoriesRepository.AddAsync(subcategory);
            await this.subcategoriesRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll<T>() =>
            this.subcategoriesRepository.AllAsNoTracking()
            .To<T>().ToList();

        public IEnumerable<Subcategory> GetAll() =>
            this.subcategoriesRepository.AllAsNoTracking()
            .ToList();

        public IEnumerable<T> GetAllDeleted<T>() =>
            this.subcategoriesRepository.AllAsNoTrackingWithDeleted()
            .Where(x => x.IsDeleted)
            .To<T>().ToList();

        public async Task<bool> EditAsync<T>(T model, IFormFile image, string directoryPath, string webRootPath)
        {
            var newSubcategory = AutoMapperConfig.MapperInstance.Map<Subcategory>(model);

            var foundSubcategory = this.GetById(newSubcategory.Id);
            if (foundSubcategory == null)
            {
                return false;
            }

            foundSubcategory.Name = newSubcategory.Name;
            foundSubcategory.MainCategoryId = newSubcategory.MainCategoryId;

            if (image != null)
            {
                var imageUrl = await this.imagesService.UploadLocalImageAsync(image, webRootPath + directoryPath);
                foundSubcategory.ImageUrl = imageUrl.Replace(webRootPath, string.Empty).Replace("\\", "/");
            }

            this.subcategoriesRepository.Update(foundSubcategory);
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

        public async Task<bool> UndeleteAsync(int id)
        {
            var subcategory = this.GetDeletedById(id);
            if (subcategory == null)
            {
                return false;
            }

            this.subcategoriesRepository.Undelete(subcategory);
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

        private Subcategory GetDeletedById(int id) =>
            this.subcategoriesRepository.AllAsNoTrackingWithDeleted()
            .Where(x => x.IsDeleted && x.Id == id)
            .FirstOrDefault();
    }
}
