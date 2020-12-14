namespace XeMart.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Internal;

    using Moq;

    using XeMart.Data.Common.Repositories;
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;
    using XeMart.Web.ViewModels;
    using XeMart.Web.ViewModels.Administration.HomePageSlides;

    using Xunit;

    [Collection("Sequential")]
    public class HomePageSlidesServiceTests
    {
        [Fact]
        public void GetAllGenericShouldReturnCorrectCountUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IRepository<HomePageSlide>>();

            var slides = new List<HomePageSlide>
            {
                new HomePageSlide { Id = 1 },
                new HomePageSlide { Id = 2 },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(slides.AsQueryable());

            var service = new HomePageSlidesService(repository.Object, null);
            Assert.Equal(2, service.GetAll<SlideViewModel>().Count());

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetAllGenericShouldMapCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IRepository<HomePageSlide>>();

            var slides = new List<HomePageSlide>
            {
                new HomePageSlide { Id = 1, Position = 2 },
                new HomePageSlide { Id = 2, Position = 3 },
                new HomePageSlide { Id = 3, Position = 1, ImageUrl = "TestImageUrl", LinkUrl = "TestLinkUrl" },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(slides.AsQueryable());

            var service = new HomePageSlidesService(repository.Object, null);
            Assert.Equal(2, service.GetAll<SlideViewModel>().LastOrDefault().Id);
            Assert.Equal(3, service.GetAll<SlideViewModel>().FirstOrDefault().Id);
            Assert.Equal(1, service.GetAll<SlideViewModel>().FirstOrDefault().Position);
            Assert.Equal("TestImageUrl", service.GetAll<SlideViewModel>().FirstOrDefault().ImageUrl);
            Assert.Equal("TestLinkUrl", service.GetAll<SlideViewModel>().FirstOrDefault().LinkUrl);

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(5));
        }

        [Fact]
        public void GetByIdGenericShouldMapCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IRepository<HomePageSlide>>();

            var slides = new List<HomePageSlide>
            {
                new HomePageSlide { Id = 1, Position = 2 },
                new HomePageSlide { Id = 2, Position = 3 },
                new HomePageSlide { Id = 3, Position = 1, ImageUrl = "TestImageUrl", LinkUrl = "TestLinkUrl" },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(slides.AsQueryable());

            var service = new HomePageSlidesService(repository.Object, null);
            Assert.Equal(3, service.GetById<SlideViewModel>(3).Id);
            Assert.Equal(1, service.GetById<SlideViewModel>(3).Position);
            Assert.Equal("TestImageUrl", service.GetById<SlideViewModel>(3).ImageUrl);
            Assert.Equal("TestLinkUrl", service.GetById<SlideViewModel>(3).LinkUrl);

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(4));
        }

        [Fact]
        public async Task CreateAsyncGenericShouldIncreaseCountUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var slidesRepository = new Mock<IRepository<HomePageSlide>>();
            var imagesService = new Mock<IImagesService>();

            var slides = new List<HomePageSlide>
            {
                new HomePageSlide { Id = 1 },
                new HomePageSlide { Id = 2 },
            };

            slidesRepository.Setup(r => r.AllAsNoTracking()).Returns(slides.AsQueryable());
            slidesRepository.Setup(r => r.AddAsync(It.IsAny<HomePageSlide>())).Callback((HomePageSlide item) => slides.Add(item));
            slidesRepository.Setup(r => r.SaveChangesAsync()).Verifiable();

            imagesService.Setup(r => r.UploadAzureBlobImageAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
                .Returns(async (IFormFile image, string path) => await Task.FromResult(path + "/" + image.FileName));

            var service = new HomePageSlidesService(slidesRepository.Object, imagesService.Object);
            var model = new CreateSlideInputViewModel { };
            await service.CreateAsync(model, new FormFile(null, 0, 0, "test", "test.png"));
            Assert.Equal(3, slides.Count());

            slidesRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
            slidesRepository.Verify(x => x.AddAsync(It.IsAny<HomePageSlide>()), Times.Once);
            slidesRepository.Verify(x => x.SaveChangesAsync());

            imagesService.Verify(x => x.UploadAzureBlobImageAsync(It.IsAny<IFormFile>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsyncGenericShouldWorkCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var slidesRepository = new Mock<IRepository<HomePageSlide>>();
            var imagesService = new Mock<IImagesService>();

            var slides = new List<HomePageSlide>
            {
                new HomePageSlide { Id = 1 },
                new HomePageSlide { Id = 2 },
            };

            slidesRepository.Setup(r => r.AllAsNoTracking()).Returns(slides.AsQueryable());
            slidesRepository.Setup(r => r.AddAsync(It.IsAny<HomePageSlide>())).Callback((HomePageSlide item) => slides.Add(item));
            slidesRepository.Setup(r => r.SaveChangesAsync()).Verifiable();

            imagesService.Setup(r => r.UploadAzureBlobImageAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
                .Returns(async (IFormFile image, string path) => await Task.FromResult(path + "/" + image.FileName));

            var service = new HomePageSlidesService(slidesRepository.Object, imagesService.Object);
            var model = new CreateSlideInputViewModel { Description = "TestDescription", LinkUrl = "TestLinkUrl" };
            await service.CreateAsync(model, new FormFile(null, 0, 0, "test", "test.png"));
            Assert.Equal("TestDescription", slides.LastOrDefault().Description);
            Assert.Equal("TestLinkUrl", slides.LastOrDefault().LinkUrl);
            Assert.Equal("images/test.png", slides.LastOrDefault().ImageUrl);

            slidesRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
            slidesRepository.Verify(x => x.AddAsync(It.IsAny<HomePageSlide>()), Times.Once);
            slidesRepository.Verify(x => x.SaveChangesAsync());

            imagesService.Verify(x => x.UploadAzureBlobImageAsync(It.IsAny<IFormFile>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task EditAsyncGenericShouldReturnFalseWithInvalidSlideIdUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var slidesRepository = new Mock<IRepository<HomePageSlide>>();

            var slides = new List<HomePageSlide>
            {
                new HomePageSlide { Id = 1 },
                new HomePageSlide { Id = 2 },
            };

            slidesRepository.Setup(r => r.All()).Returns(slides.AsQueryable());

            var service = new HomePageSlidesService(slidesRepository.Object, null);
            var model = new EditSlideViewModel { Id = 3 };
            Assert.False(await service.EditAsync(model, null));

            slidesRepository.Verify(x => x.All(), Times.Once);
        }

        [Fact]
        public async Task EditAsyncGenericShouldWorkCorrectlyWithNoImageUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var slidesRepository = new Mock<IRepository<HomePageSlide>>();

            var slides = new List<HomePageSlide>
            {
                new HomePageSlide { Id = 1 },
                new HomePageSlide { Id = 2, Description = "TestDescription", LinkUrl = "TestLinkUrl" },
            };

            slidesRepository.Setup(r => r.All()).Returns(slides.AsQueryable());
            slidesRepository.Setup(r => r.Update(It.IsAny<HomePageSlide>())).Callback((HomePageSlide item) =>
            {
                var foundSlide = slides.FirstOrDefault(x => x.Id == item.Id);
                foundSlide.Description = item.Description;
                foundSlide.LinkUrl = item.LinkUrl;
            });
            slidesRepository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new HomePageSlidesService(slidesRepository.Object, null);
            var model = new EditSlideViewModel { Id = 2, Description = "TestDescriptionEdited", LinkUrl = "TestLinkUrlEdited" };

            Assert.True(await service.EditAsync(model, null));
            Assert.Equal("TestDescriptionEdited", slides.FirstOrDefault(x => x.Id == 2).Description);
            Assert.Equal("TestLinkUrlEdited", slides.FirstOrDefault(x => x.Id == 2).LinkUrl);

            slidesRepository.Verify(x => x.All(), Times.Once);
            slidesRepository.Verify(x => x.Update(It.IsAny<HomePageSlide>()), Times.Once);
            slidesRepository.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task EditAsyncGenericShouldWorkCorrectlyWithImageUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var slidesRepository = new Mock<IRepository<HomePageSlide>>();
            var imagesService = new Mock<IImagesService>();

            var slides = new List<HomePageSlide>
            {
                new HomePageSlide { Id = 1 },
                new HomePageSlide { Id = 2, Description = "TestDescription", LinkUrl = "TestLinkUrl" },
            };

            slidesRepository.Setup(r => r.All()).Returns(slides.AsQueryable());
            slidesRepository.Setup(r => r.Update(It.IsAny<HomePageSlide>())).Callback((HomePageSlide item) =>
            {
                var foundSlide = slides.FirstOrDefault(x => x.Id == item.Id);
                foundSlide.Description = item.Description;
                foundSlide.LinkUrl = item.LinkUrl;
            });
            slidesRepository.Setup(r => r.SaveChangesAsync()).Verifiable();

            imagesService.Setup(r => r.UploadAzureBlobImageAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
                .Returns(async (IFormFile image, string path) => await Task.FromResult(path + "/" + image.FileName));

            var service = new HomePageSlidesService(slidesRepository.Object, imagesService.Object);
            var model = new EditSlideViewModel { Id = 2, Description = "TestDescriptionEdited", LinkUrl = "TestLinkUrlEdited" };

            Assert.True(await service.EditAsync(model, new FormFile(null, 0, 0, "test", "test.png")));
            Assert.Equal("TestDescriptionEdited", slides.FirstOrDefault(x => x.Id == 2).Description);
            Assert.Equal("TestLinkUrlEdited", slides.FirstOrDefault(x => x.Id == 2).LinkUrl);
            Assert.Equal("images/test.png", slides.FirstOrDefault(x => x.Id == 2).ImageUrl);

            slidesRepository.Verify(x => x.All(), Times.Once);
            slidesRepository.Verify(x => x.Update(It.IsAny<HomePageSlide>()), Times.Once);
            slidesRepository.Verify(x => x.SaveChangesAsync());

            imagesService.Verify(x => x.UploadAzureBlobImageAsync(It.IsAny<IFormFile>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsyncShouldReturnFalseWithInvalidSlideIdUsingMoq()
        {
            var slidesRepository = new Mock<IRepository<HomePageSlide>>();

            var slides = new List<HomePageSlide>
            {
                new HomePageSlide { Id = 1 },
                new HomePageSlide { Id = 2 },
            };

            slidesRepository.Setup(r => r.All()).Returns(slides.AsQueryable());

            var service = new HomePageSlidesService(slidesRepository.Object, null);
            Assert.False(await service.DeleteAsync(3));

            slidesRepository.Verify(x => x.All(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsyncShouldWorkCorrectlyIdUsingMoq()
        {
            var slidesRepository = new Mock<IRepository<HomePageSlide>>();

            var slides = new List<HomePageSlide>
            {
                new HomePageSlide { Id = 1 },
                new HomePageSlide { Id = 2 },
            };

            slidesRepository.Setup(r => r.All()).Returns(slides.AsQueryable());
            slidesRepository.Setup(r => r.Delete(It.IsAny<HomePageSlide>())).Callback((HomePageSlide item) => slides.Remove(item));
            slidesRepository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new HomePageSlidesService(slidesRepository.Object, null);
            Assert.True(await service.DeleteAsync(2));
            Assert.Single(slides);

            slidesRepository.Verify(x => x.All(), Times.Once);
            slidesRepository.Verify(x => x.Delete(It.IsAny<HomePageSlide>()), Times.Once);
            slidesRepository.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task MoveUpAsyncShouldReturnFalseWithInvalidSlideIdUsingMoq()
        {
            var slidesRepository = new Mock<IRepository<HomePageSlide>>();

            var slides = new List<HomePageSlide>
            {
                new HomePageSlide { Id = 1 },
                new HomePageSlide { Id = 2 },
            };

            slidesRepository.Setup(r => r.All()).Returns(slides.AsQueryable());

            var service = new HomePageSlidesService(slidesRepository.Object, null);
            Assert.False(await service.MoveUpAsync(3));

            slidesRepository.Verify(x => x.All(), Times.Once);
        }

        [Fact]
        public async Task MoveUpAsyncShouldReturnFalseWhenSlidePositionIsEqualToOneUsingMoq()
        {
            var slidesRepository = new Mock<IRepository<HomePageSlide>>();

            var slides = new List<HomePageSlide>
            {
                new HomePageSlide { Id = 1 },
                new HomePageSlide { Id = 2, Position = 1 },
            };

            slidesRepository.Setup(r => r.All()).Returns(slides.AsQueryable());

            var service = new HomePageSlidesService(slidesRepository.Object, null);
            Assert.False(await service.MoveUpAsync(2));

            slidesRepository.Verify(x => x.All(), Times.Once);
        }

        [Fact]
        public async Task MoveUpAsyncShouldWorkCorrectlyUsingMoq()
        {
            var slidesRepository = new Mock<IRepository<HomePageSlide>>();

            var slides = new List<HomePageSlide>
            {
                new HomePageSlide { Id = 1, Position = 1 },
                new HomePageSlide { Id = 2, Position = 2 },
            };

            slidesRepository.Setup(r => r.All()).Returns(slides.AsQueryable());
            slidesRepository.Setup(r => r.Update(It.IsAny<HomePageSlide>())).Callback((HomePageSlide item) =>
            {
                var foundSlide = slides.FirstOrDefault(x => x.Id == item.Id);
                foundSlide.Position = item.Position;
            });
            slidesRepository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new HomePageSlidesService(slidesRepository.Object, null);
            Assert.True(await service.MoveUpAsync(2));
            Assert.Equal(2, slides.FirstOrDefault().Position);
            Assert.Equal(1, slides.LastOrDefault().Position);

            slidesRepository.Verify(x => x.All(), Times.Exactly(2));
            slidesRepository.Verify(x => x.Update(It.IsAny<HomePageSlide>()), Times.Exactly(2));
            slidesRepository.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task MoveDownAsyncShouldReturnFalseWithInvalidSlideIdUsingMoq()
        {
            var slidesRepository = new Mock<IRepository<HomePageSlide>>();

            var slides = new List<HomePageSlide>
            {
                new HomePageSlide { Id = 1, Position = 1 },
                new HomePageSlide { Id = 2, Position = 2 },
            };

            slidesRepository.Setup(r => r.All()).Returns(slides.AsQueryable());

            var service = new HomePageSlidesService(slidesRepository.Object, null);
            Assert.False(await service.MoveDownAsync(3));

            slidesRepository.Verify(x => x.All(), Times.Once);
        }

        [Fact]
        public async Task MoveDownAsyncShouldReturnFalseWhenThereIsNoBottomSlideUsingMoq()
        {
            var slidesRepository = new Mock<IRepository<HomePageSlide>>();

            var slides = new List<HomePageSlide>
            {
                new HomePageSlide { Id = 1, Position = 1 },
                new HomePageSlide { Id = 2, Position = 2 },
            };

            slidesRepository.Setup(r => r.All()).Returns(slides.AsQueryable());

            var service = new HomePageSlidesService(slidesRepository.Object, null);
            Assert.False(await service.MoveDownAsync(2));

            slidesRepository.Verify(x => x.All(), Times.Exactly(2));
        }

        [Fact]
        public async Task MoveDownAsyncShouldWorkCorrectlyUsingMoq()
        {
            var slidesRepository = new Mock<IRepository<HomePageSlide>>();

            var slides = new List<HomePageSlide>
            {
                new HomePageSlide { Id = 1, Position = 1 },
                new HomePageSlide { Id = 2, Position = 2 },
            };

            slidesRepository.Setup(r => r.All()).Returns(slides.AsQueryable());
            slidesRepository.Setup(r => r.Update(It.IsAny<HomePageSlide>())).Callback((HomePageSlide item) =>
            {
                var foundSlide = slides.FirstOrDefault(x => x.Id == item.Id);
                foundSlide.Position = item.Position;
            });
            slidesRepository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new HomePageSlidesService(slidesRepository.Object, null);
            Assert.True(await service.MoveDownAsync(1));
            Assert.Equal(2, slides.FirstOrDefault().Position);
            Assert.Equal(1, slides.LastOrDefault().Position);

            slidesRepository.Verify(x => x.All(), Times.Exactly(2));
            slidesRepository.Verify(x => x.Update(It.IsAny<HomePageSlide>()), Times.Exactly(2));
            slidesRepository.Verify(x => x.SaveChangesAsync());
        }
    }
}
