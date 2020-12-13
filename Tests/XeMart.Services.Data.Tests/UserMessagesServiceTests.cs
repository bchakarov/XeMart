namespace XeMart.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Moq;

    using XeMart.Data.Common.Repositories;
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;
    using XeMart.Web.ViewModels;
    using XeMart.Web.ViewModels.Administration.UserMessages;
    using XeMart.Web.ViewModels.Home;

    using Xunit;

    [Collection("Sequential")]
    public class UserMessagesServiceTests
    {
        [Fact]
        public void GetAllGenericShouldReturnCorrectCountUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<UserMessage>>();

            var messages = new List<UserMessage>
            {
                new UserMessage { Id = "TestId1" },
                new UserMessage { Id = "TestId2" },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(messages.AsQueryable());

            var service = new UserMessagesService(repository.Object);
            Assert.Equal(2, service.GetAll<UserMessageViewModel>().Count());

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetAllGenericShouldMapCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<UserMessage>>();

            var messages = new List<UserMessage>
            {
                new UserMessage { Id = "TestId1", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 12), Subject = "TestSubject1", Email = "TestEmail1", Message = "TestMessage1", IP = "TestIp1", IsRead = true },
                new UserMessage { Id = "TestId2", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 13), Subject = "TestSubject2", Email = "TestEmail2", Message = "TestMessage2", IP = "TestIp2", IsRead = false },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(messages.AsQueryable());

            var service = new UserMessagesService(repository.Object);
            Assert.Equal("TestId2", service.GetAll<UserMessageViewModel>().FirstOrDefault().Id);
            Assert.Equal("31-Dec-2020 12:12", service.GetAll<UserMessageViewModel>().FirstOrDefault().CreatedOn);
            Assert.Equal("TestSubject2", service.GetAll<UserMessageViewModel>().FirstOrDefault().Subject);
            Assert.Equal("TestEmail2", service.GetAll<UserMessageViewModel>().FirstOrDefault().Email);
            Assert.Equal("TestMessage2", service.GetAll<UserMessageViewModel>().FirstOrDefault().Message);
            Assert.Equal("TestIp2", service.GetAll<UserMessageViewModel>().FirstOrDefault().IP);
            Assert.False(service.GetAll<UserMessageViewModel>().FirstOrDefault().IsRead);

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(7));
        }

        [Fact]
        public void GetUnreadMessagesGenericShouldReturnCorrectCountUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<UserMessage>>();

            var messages = new List<UserMessage>
            {
                new UserMessage { Id = "TestId1", IsRead = false },
                new UserMessage { Id = "TestId2", IsRead = false },
                new UserMessage { Id = "TestId3", IsRead = true },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(messages.AsQueryable());

            var service = new UserMessagesService(repository.Object);
            Assert.Equal(2, service.GetUnreadMessages<UserMessageViewModel>().Count());

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetUnreadMessagesGenericShouldMapCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<UserMessage>>();

            var messages = new List<UserMessage>
            {
                new UserMessage { Id = "TestId1", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 12), Subject = "TestSubject1", Email = "TestEmail1", Message = "TestMessage1", IP = "TestIp1", IsRead = false },
                new UserMessage { Id = "TestId2", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 13), Subject = "TestSubject2", Email = "TestEmail2", Message = "TestMessage2", IP = "TestIp2", IsRead = false },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(messages.AsQueryable());

            var service = new UserMessagesService(repository.Object);
            Assert.Equal("TestId2", service.GetUnreadMessages<UserMessageViewModel>().FirstOrDefault().Id);
            Assert.Equal("31-Dec-2020 12:12", service.GetUnreadMessages<UserMessageViewModel>().FirstOrDefault().CreatedOn);
            Assert.Equal("TestSubject2", service.GetUnreadMessages<UserMessageViewModel>().FirstOrDefault().Subject);
            Assert.Equal("TestEmail2", service.GetUnreadMessages<UserMessageViewModel>().FirstOrDefault().Email);
            Assert.Equal("TestMessage2", service.GetUnreadMessages<UserMessageViewModel>().FirstOrDefault().Message);
            Assert.Equal("TestIp2", service.GetUnreadMessages<UserMessageViewModel>().FirstOrDefault().IP);
            Assert.False(service.GetUnreadMessages<UserMessageViewModel>().FirstOrDefault().IsRead);

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(7));
        }

        [Fact]
        public void GetAllDeletedGenericShouldReturnCorrectCountUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<UserMessage>>();

            var messages = new List<UserMessage>
            {
                new UserMessage { Id = "TestId1", IsDeleted = false },
                new UserMessage { Id = "TestId2", IsDeleted = true },
                new UserMessage { Id = "TestId3", IsDeleted = true },
            };

            repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(messages.AsQueryable());

            var service = new UserMessagesService(repository.Object);
            Assert.Equal(2, service.GetAllDeleted<UserMessageViewModel>().Count());

            repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Once);
        }

        [Fact]
        public void GetAllDeletedGenericShouldMapCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<UserMessage>>();

            var messages = new List<UserMessage>
            {
                new UserMessage { Id = "TestId1", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 12), DeletedOn = new DateTime(2020, 12, 31, 13, 13, 13), Subject = "TestSubject1", Email = "TestEmail1", Message = "TestMessage1", IP = "TestIp1", IsDeleted = true },
                new UserMessage { Id = "TestId2", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 13), DeletedOn = new DateTime(2020, 12, 31, 13, 13, 14), Subject = "TestSubject2", Email = "TestEmail2", Message = "TestMessage2", IP = "TestIp2", IsDeleted = true },
            };

            repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(messages.AsQueryable());

            var service = new UserMessagesService(repository.Object);
            Assert.Equal("TestId2", service.GetAllDeleted<DeletedUserMessagesViewModel>().FirstOrDefault().Id);
            Assert.Equal("31-Dec-2020 12:12", service.GetAllDeleted<DeletedUserMessagesViewModel>().FirstOrDefault().CreatedOn);
            Assert.Equal("31-Dec-2020 13:13", service.GetAllDeleted<DeletedUserMessagesViewModel>().FirstOrDefault().DeletedOn);
            Assert.Equal("TestSubject2", service.GetAllDeleted<DeletedUserMessagesViewModel>().FirstOrDefault().Subject);
            Assert.Equal("TestEmail2", service.GetAllDeleted<DeletedUserMessagesViewModel>().FirstOrDefault().Email);
            Assert.Equal("TestMessage2", service.GetAllDeleted<DeletedUserMessagesViewModel>().FirstOrDefault().Message);
            Assert.Equal("TestIp2", service.GetAllDeleted<DeletedUserMessagesViewModel>().FirstOrDefault().IP);
            Assert.False(service.GetAllDeleted<DeletedUserMessagesViewModel>().FirstOrDefault().IsRead);

            repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Exactly(8));
        }

        [Fact]
        public void GetByIdGenericShouldMapCorrectlyWithDeletedMessageUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<UserMessage>>();

            var messages = new List<UserMessage>
            {
                new UserMessage { Id = "TestId1", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 12), Subject = "TestSubject1", Email = "TestEmail1", Message = "TestMessage1", IP = "TestIp1", IsDeleted = false, IsRead = false },
                new UserMessage { Id = "TestId2", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 13), DeletedOn = new DateTime(2020, 12, 31, 13, 13, 14), Subject = "TestSubject2", Email = "TestEmail2", Message = "TestMessage2", IP = "TestIp2", IsDeleted = true, IsRead = false },
            };

            repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(messages.AsQueryable());

            var service = new UserMessagesService(repository.Object);
            Assert.Equal("TestId2", service.GetById<DeletedUserMessagesViewModel>("TestId2").Id);
            Assert.Equal("31-Dec-2020 12:12", service.GetById<DeletedUserMessagesViewModel>("TestId2").CreatedOn);
            Assert.Equal("31-Dec-2020 13:13", service.GetById<DeletedUserMessagesViewModel>("TestId2").DeletedOn);
            Assert.Equal("TestSubject2", service.GetById<DeletedUserMessagesViewModel>("TestId2").Subject);
            Assert.Equal("TestEmail2", service.GetById<DeletedUserMessagesViewModel>("TestId2").Email);
            Assert.Equal("TestMessage2", service.GetById<DeletedUserMessagesViewModel>("TestId2").Message);
            Assert.Equal("TestIp2", service.GetById<DeletedUserMessagesViewModel>("TestId2").IP);
            Assert.False(service.GetById<DeletedUserMessagesViewModel>("TestId2").IsRead);

            repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Exactly(8));
        }

        [Fact]
        public void GetByIdGenericShouldMapCorrectlyWithUndeletedMessageUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<UserMessage>>();

            var messages = new List<UserMessage>
            {
                new UserMessage { Id = "TestId1", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 12), Subject = "TestSubject1", Email = "TestEmail1", Message = "TestMessage1", IP = "TestIp1", IsDeleted = false, IsRead = false },
                new UserMessage { Id = "TestId2", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 13), DeletedOn = new DateTime(2020, 12, 31, 13, 13, 14), Subject = "TestSubject2", Email = "TestEmail2", Message = "TestMessage2", IP = "TestIp2", IsDeleted = true, IsRead = false },
            };

            repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(messages.AsQueryable());

            var service = new UserMessagesService(repository.Object);
            Assert.Equal("TestId1", service.GetById<UserMessageViewModel>("TestId1").Id);
            Assert.Equal("31-Dec-2020 12:12", service.GetById<UserMessageViewModel>("TestId1").CreatedOn);
            Assert.Equal("TestSubject1", service.GetById<UserMessageViewModel>("TestId1").Subject);
            Assert.Equal("TestEmail1", service.GetById<UserMessageViewModel>("TestId1").Email);
            Assert.Equal("TestMessage1", service.GetById<UserMessageViewModel>("TestId1").Message);
            Assert.Equal("TestIp1", service.GetById<UserMessageViewModel>("TestId1").IP);
            Assert.False(service.GetById<UserMessageViewModel>("TestId1").IsRead);

            repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Exactly(7));
        }

        [Fact]
        public async Task CreateAsyncGenericShouldIncreaseCountUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<UserMessage>>();

            var messages = new List<UserMessage>
            {
                new UserMessage { Id = "TestId1", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 12), Subject = "TestSubject1", Email = "TestEmail1", Message = "TestMessage1", IP = "TestIp1" },
                new UserMessage { Id = "TestId2", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 13), Subject = "TestSubject2", Email = "TestEmail2", Message = "TestMessage2", IP = "TestIp2" },
            };

            repository.Setup(r => r.AddAsync(It.IsAny<UserMessage>())).Callback((UserMessage item) => messages.Add(item));
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new UserMessagesService(repository.Object);
            var model = new ContactFormInputViewModel
            {
                Subject = "TestSubject3",
                Email = "TestEmail3",
                Message = "TestMessage3",
            };
            await service.CreateAsync<ContactFormInputViewModel>(model, "TestIp3");

            Assert.Equal(3, messages.Count);

            repository.Verify(x => x.AddAsync(It.IsAny<UserMessage>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task CreateAsyncGenericShouldWorkCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<UserMessage>>();

            var messages = new List<UserMessage>
            {
                new UserMessage { Id = "TestId1", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 12), Subject = "TestSubject1", Email = "TestEmail1", Message = "TestMessage1", IP = "TestIp1" },
                new UserMessage { Id = "TestId2", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 13), Subject = "TestSubject2", Email = "TestEmail2", Message = "TestMessage2", IP = "TestIp2" },
            };

            repository.Setup(r => r.AddAsync(It.IsAny<UserMessage>())).Callback((UserMessage item) => messages.Add(item));
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new UserMessagesService(repository.Object);
            var model = new ContactFormInputViewModel
            {
                Subject = "TestSubject3",
                Email = "TestEmail3",
                Message = "TestMessage3",
            };
            await service.CreateAsync<ContactFormInputViewModel>(model, "TestIp3");

            Assert.Equal("TestSubject3", messages.Last().Subject);
            Assert.Equal("TestEmail3", messages.Last().Email);
            Assert.Equal("TestMessage3", messages.Last().Message);
            Assert.Equal("TestIp3", messages.Last().IP);
            Assert.False(messages.Last().IsRead);

            repository.Verify(x => x.AddAsync(It.IsAny<UserMessage>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task SetIsReadAsyncShouldReturnFalseWithInvalidMessageIdUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<UserMessage>>();

            var messages = new List<UserMessage>
            {
                new UserMessage { Id = "TestId1", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 12), Subject = "TestSubject1", Email = "TestEmail1", Message = "TestMessage1", IP = "TestIp1" },
                new UserMessage { Id = "TestId2", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 13), Subject = "TestSubject2", Email = "TestEmail2", Message = "TestMessage2", IP = "TestIp2" },
            };

            repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(messages.AsQueryable());

            var service = new UserMessagesService(repository.Object);

            Assert.False(await service.SetIsReadAsync("InvalidId", false));

            repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Once);
        }

        [Fact]
        public async Task SetIsReadAsyncShouldWorkCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<UserMessage>>();

            var messages = new List<UserMessage>
            {
                new UserMessage { Id = "TestId1", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 12), Subject = "TestSubject1", Email = "TestEmail1", Message = "TestMessage1", IP = "TestIp1", IsRead = false },
                new UserMessage { Id = "TestId2", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 13), Subject = "TestSubject2", Email = "TestEmail2", Message = "TestMessage2", IP = "TestIp2" },
            };

            repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(messages.AsQueryable());
            repository.Setup(r => r.Update(It.IsAny<UserMessage>())).Callback((UserMessage item) => messages.FirstOrDefault(x => x.Id == item.Id).IsRead = item.IsRead);
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new UserMessagesService(repository.Object);

            Assert.True(await service.SetIsReadAsync("TestId1", true));
            Assert.True(messages.FirstOrDefault(x => x.Id == "TestId1").IsRead);

            repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Once);
            repository.Verify(x => x.Update(It.IsAny<UserMessage>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task DeleteAsyncShouldReturnFalseWithInvalidMessageIdUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<UserMessage>>();

            var messages = new List<UserMessage>
            {
                new UserMessage { Id = "TestId1", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 12), Subject = "TestSubject1", Email = "TestEmail1", Message = "TestMessage1", IP = "TestIp1" },
                new UserMessage { Id = "TestId2", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 13), Subject = "TestSubject2", Email = "TestEmail2", Message = "TestMessage2", IP = "TestIp2" },
            };

            repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(messages.AsQueryable());

            var service = new UserMessagesService(repository.Object);

            Assert.False(await service.DeleteAsync("InvalidId"));

            repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsyncShouldWorkCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<UserMessage>>();

            var messages = new List<UserMessage>
            {
                new UserMessage { Id = "TestId1", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 12), Subject = "TestSubject1", Email = "TestEmail1", Message = "TestMessage1", IP = "TestIp1", IsRead = false },
                new UserMessage { Id = "TestId2", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 13), Subject = "TestSubject2", Email = "TestEmail2", Message = "TestMessage2", IP = "TestIp2" },
            };

            repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(messages.AsQueryable());
            repository.Setup(r => r.Delete(It.IsAny<UserMessage>())).Callback((UserMessage item) => messages.Remove(item));
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new UserMessagesService(repository.Object);

            Assert.True(await service.DeleteAsync("TestId1"));
            Assert.Single(messages);

            repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Once);
            repository.Verify(x => x.Delete(It.IsAny<UserMessage>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task UndeleteAsyncShouldReturnFalseWithInvalidMessageIdUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<UserMessage>>();

            var messages = new List<UserMessage>
            {
                new UserMessage { Id = "TestId1", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 12), Subject = "TestSubject1", Email = "TestEmail1", Message = "TestMessage1", IP = "TestIp1" },
                new UserMessage { Id = "TestId2", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 13), Subject = "TestSubject2", Email = "TestEmail2", Message = "TestMessage2", IP = "TestIp2" },
            };

            repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(messages.AsQueryable());

            var service = new UserMessagesService(repository.Object);

            Assert.False(await service.UndeleteAsync("InvalidId"));

            repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Once);
        }

        [Fact]
        public async Task UndeleteAsyncShouldWorkCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<UserMessage>>();

            var messages = new List<UserMessage>
            {
                new UserMessage { Id = "TestId1", IsDeleted = true, DeletedOn = new DateTime(2020, 12, 31, 12, 12, 12), Subject = "TestSubject1", Email = "TestEmail1", Message = "TestMessage1", IP = "TestIp1", IsRead = false },
                new UserMessage { Id = "TestId2", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 13), Subject = "TestSubject2", Email = "TestEmail2", Message = "TestMessage2", IP = "TestIp2" },
            };

            repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(messages.AsQueryable());
            repository.Setup(r => r.Undelete(It.IsAny<UserMessage>())).Callback((UserMessage item) =>
            {
                var foundMessage = messages.FirstOrDefault(x => x.Id == item.Id);
                foundMessage.IsDeleted = false;
                foundMessage.DeletedOn = null;
            });
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new UserMessagesService(repository.Object);

            Assert.True(await service.UndeleteAsync("TestId1"));
            Assert.False(messages.FirstOrDefault(x => x.Id == "TestId1").IsDeleted);
            Assert.Null(messages.FirstOrDefault(x => x.Id == "TestId1").DeletedOn);

            repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Once);
            repository.Verify(x => x.Undelete(It.IsAny<UserMessage>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());
        }
    }
}
