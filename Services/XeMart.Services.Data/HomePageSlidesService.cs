namespace XeMart.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    using XeMart.Data.Common.Repositories;
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class HomePageSlidesService : IHomePageSlidesService
    {
        private const string AzureContainerName = "images";

        private readonly IRepository<HomePageSlide> homePageSlidesRepository;
        private readonly IImagesService imagesService;

        public HomePageSlidesService(
            IRepository<HomePageSlide> homePageSlidesRepository,
            IImagesService imagesService)
        {
            this.homePageSlidesRepository = homePageSlidesRepository;
            this.imagesService = imagesService;
        }

        public async Task CreateAsync<T>(T model, IFormFile image)
        {
            var slide = AutoMapperConfig.MapperInstance.Map<HomePageSlide>(model);

            slide.ImageUrl = await this.imagesService.UploadAzureBlobImageAsync(image, AzureContainerName);
            slide.Position = this.GetMaxPositionValue() + 1;

            await this.homePageSlidesRepository.AddAsync(slide);
            await this.homePageSlidesRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll<T>() =>
            this.homePageSlidesRepository.AllAsNoTracking()
            .OrderBy(x => x.Position)
            .To<T>().ToList();

        public T GetById<T>(int id) =>
            this.homePageSlidesRepository.AllAsNoTracking()
            .Where(x => x.Id == id)
            .To<T>()
            .FirstOrDefault();

        public async Task<bool> EditAsync<T>(T model, IFormFile image)
        {
            var newSlide = AutoMapperConfig.MapperInstance.Map<HomePageSlide>(model);

            var foundSlide = this.GetById(newSlide.Id);
            if (foundSlide == null)
            {
                return false;
            }

            foundSlide.Description = newSlide.Description;
            foundSlide.LinkUrl = newSlide.LinkUrl;

            if (image != null)
            {
                foundSlide.ImageUrl = await this.imagesService.UploadAzureBlobImageAsync(image, AzureContainerName);
            }

            this.homePageSlidesRepository.Update(foundSlide);
            await this.homePageSlidesRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> MoveUpAsync(int id)
        {
            var slide = this.GetById(id);
            if (slide == null || slide.Position <= 1)
            {
                return false;
            }

            slide.Position--;

            var foundSlide = this.GetByPosition(slide.Position);
            if (foundSlide != null)
            {
                foundSlide.Position++;
                this.homePageSlidesRepository.Update(foundSlide);
            }

            this.homePageSlidesRepository.Update(slide);
            await this.homePageSlidesRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> MoveDownAsync(int id)
        {
            var slide = this.GetById(id);
            if (slide == null)
            {
                return false;
            }

            var foundSlide = this.GetByPosition(slide.Position + 1);
            if (foundSlide == null)
            {
                return false;
            }

            slide.Position++;
            foundSlide.Position--;

            this.homePageSlidesRepository.Update(foundSlide);
            this.homePageSlidesRepository.Update(slide);
            await this.homePageSlidesRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var slide = this.GetById(id);
            if (slide == null)
            {
                return false;
            }

            this.homePageSlidesRepository.Delete(slide);
            await this.homePageSlidesRepository.SaveChangesAsync();

            return true;
        }

        private HomePageSlide GetById(int id) =>
            this.homePageSlidesRepository.All()
            .FirstOrDefault(x => x.Id == id);

        private HomePageSlide GetByPosition(int position) =>
            this.homePageSlidesRepository.All()
            .FirstOrDefault(x => x.Position == position);

        private int GetMaxPositionValue()
        {
            var slides = this.homePageSlidesRepository.AllAsNoTracking();
            if (!slides.Any())
            {
                return 0;
            }

            var maxPosition = slides.Max(x => x.Position);

            return maxPosition;
        }
    }
}
