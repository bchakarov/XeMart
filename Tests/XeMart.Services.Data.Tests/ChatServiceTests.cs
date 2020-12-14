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
    using XeMart.Web.ViewModels.Chat;

    using Xunit;

    [Collection("Sequential")]
    public class ChatServiceTests
    {
        [Fact]
        public void GetRoomIdByOwnerIdShouldWorkCorrectlyUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<ChatRoom>>();

            var chatRoomsList = new List<ChatRoom>
            {
                new ChatRoom { Id = "TestId1", OwnerId = "TestOwnerId1" },
                new ChatRoom { Id = "TestId2", OwnerId = "TestOwnerId2" },
                new ChatRoom { Id = "TestId3", OwnerId = "TestOwnerId3" },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(chatRoomsList.AsQueryable());

            var service = new ChatService(repository.Object, null);
            Assert.Equal("TestId1", service.GetRoomIdByOwnerId("TestOwnerId1"));

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetRoomIdByOwnerIdShouldReturnNullWithInvalidOwnerIdUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<ChatRoom>>();

            var chatRoomsList = new List<ChatRoom>
            {
                new ChatRoom { Id = "TestId1", OwnerId = "TestOwnerId1" },
                new ChatRoom { Id = "TestId2", OwnerId = "TestOwnerId2" },
                new ChatRoom { Id = "TestId3", OwnerId = "TestOwnerId3" },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(chatRoomsList.AsQueryable());

            var service = new ChatService(repository.Object, null);
            Assert.Null(service.GetRoomIdByOwnerId("TestOwnerId4"));

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetAllRoomsGenericShouldReturnCorrectCountUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<ChatRoom>>();

            var owner = new ApplicationUser { Id = "TestOwnerId", UserName = "TestOwnerUsername" };
            var chatRoomsList = new List<ChatRoom>
            {
                new ChatRoom { Id = "TestId1", OwnerId = owner.Id, Owner = owner },
                new ChatRoom { Id = "TestId2", OwnerId = owner.Id, Owner = owner },
                new ChatRoom { Id = "TestId3", OwnerId = owner.Id, Owner = owner },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(chatRoomsList.AsQueryable());

            var service = new ChatService(repository.Object, null);
            Assert.Equal(3, service.GetAllRooms<RoomViewModel>().Count());

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetAllRoomsGenericShouldMapCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<ChatRoom>>();

            var owner = new ApplicationUser { Id = "TestOwnerId", UserName = "TestOwnerUsername" };

            var messagesList = new List<RoomMessage>
            {
                new RoomMessage { Id = 1, CreatedOn = new DateTime(2020, 12, 31, 12, 12, 12), Message = "TestMessage1" },
                new RoomMessage { Id = 2, CreatedOn = new DateTime(2020, 12, 31, 12, 12, 13), Message = "TestMessage2" },
                new RoomMessage { Id = 3, CreatedOn = new DateTime(2020, 12, 31, 12, 12, 14), Message = "TestMessage3" },
            };

            var chatRoomsList = new List<ChatRoom>
            {
                new ChatRoom { Id = "TestId1", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 12), OwnerId = owner.Id, Owner = owner, Messages = messagesList },
                new ChatRoom { Id = "TestId2", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 12), OwnerId = owner.Id, Owner = owner, Messages = messagesList },
                new ChatRoom { Id = "TestId3", CreatedOn = new DateTime(2020, 12, 31, 12, 12, 12), OwnerId = owner.Id, Owner = owner, Messages = messagesList },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(chatRoomsList.AsQueryable());

            var service = new ChatService(repository.Object, null);
            Assert.Equal("TestId1", service.GetAllRooms<RoomViewModel>().FirstOrDefault().Id);
            Assert.Equal("31-Dec-2020 12:12", service.GetAllRooms<RoomViewModel>().FirstOrDefault().CreatedOn);
            Assert.Equal("TestOwnerUsername", service.GetAllRooms<RoomViewModel>().FirstOrDefault().OwnerUsername);
            Assert.Equal("TestMessage3", service.GetAllRooms<RoomViewModel>().FirstOrDefault().LastMessage);

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(4));
        }

        [Fact]
        public void GetAllMessagesByRoomIdGenericShouldReturnCorrectCountUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var roomMessagesRepository = new Mock<IRepository<RoomMessage>>();

            var sender = new ApplicationUser { Id = "TestSenderId", UserName = "TestSenderName" };
            var owner = new ApplicationUser { Id = "TestOwnerId", UserName = "TestOwnerName" };

            var chatRoom = new ChatRoom { OwnerId = owner.Id, Owner = owner };

            var messagesList = new List<RoomMessage>
            {
                new RoomMessage { Id = 1, CreatedOn = DateTime.UtcNow, Message = "TestMessage1", Room = chatRoom, RoomId = "TestId1", Sender = sender, SenderId = sender.Id },
                new RoomMessage { Id = 2, CreatedOn = DateTime.UtcNow, Message = "TestMessage2", Room = chatRoom, RoomId = "TestId2", Sender = sender, SenderId = sender.Id },
                new RoomMessage { Id = 3, CreatedOn = DateTime.UtcNow, Message = "TestMessage3", Room = chatRoom, RoomId = "TestId1", Sender = sender, SenderId = sender.Id },
            };

            roomMessagesRepository.Setup(r => r.AllAsNoTracking()).Returns(messagesList.AsQueryable());

            var service = new ChatService(null, roomMessagesRepository.Object);
            Assert.Equal(2, service.GetAllMessagesByRoomId<MessageViewModel>("TestId1").Count());

            roomMessagesRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetAllMessagesByRoomIdGenericShouldMapCorrectlyUsingMoq2()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var roomMessagesRepository = new Mock<IRepository<RoomMessage>>();

            var sender = new ApplicationUser { Id = "TestSenderId", UserName = "TestSenderName" };
            var owner = new ApplicationUser { Id = "TestOwnerId", UserName = "TestOwnerName" };

            var chatRoom = new ChatRoom { OwnerId = owner.Id, Owner = owner };

            var messagesList = new List<RoomMessage>
            {
                new RoomMessage { Id = 1, CreatedOn = new DateTime(2020, 12, 31, 12, 12, 12), Message = "TestMessage1", Room = chatRoom, RoomId = "TestRoomId1", Sender = sender, SenderId = sender.Id },
                new RoomMessage { Id = 2, CreatedOn = DateTime.UtcNow, Message = "TestMessage2", Room = chatRoom, RoomId = "TestRoomId2", Sender = owner, SenderId = owner.Id },
                new RoomMessage { Id = 3, CreatedOn = DateTime.UtcNow, Message = "TestMessage3", Room = chatRoom, RoomId = "TestRoomId3", Sender = sender, SenderId = sender.Id },
            };

            roomMessagesRepository.Setup(r => r.AllAsNoTracking()).Returns(messagesList.AsQueryable());

            var service = new ChatService(null, roomMessagesRepository.Object);
            Assert.Equal("31-Dec-2020 12:12", service.GetAllMessagesByRoomId<MessageViewModel>("TestRoomId1").FirstOrDefault().CreatedOn);
            Assert.Equal("TestMessage1", service.GetAllMessagesByRoomId<MessageViewModel>("TestRoomId1").FirstOrDefault().Message);
            Assert.Equal("TestSenderName", service.GetAllMessagesByRoomId<MessageViewModel>("TestRoomId1").FirstOrDefault().SenderUsername);
            Assert.False(service.GetAllMessagesByRoomId<MessageViewModel>("TestRoomId1").FirstOrDefault().IsByRoomOwner);
            Assert.True(service.GetAllMessagesByRoomId<MessageViewModel>("TestRoomId2").FirstOrDefault().IsByRoomOwner);

            roomMessagesRepository.Verify(x => x.AllAsNoTracking(), Times.Exactly(5));
        }

        [Fact]
        public async Task CreateRoomAsyncGenericShouldReturnDefaultValueOfTWhenUserAlreadyHasARoomUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<ChatRoom>>();

            var chatRoomsList = new List<ChatRoom>
            {
                new ChatRoom { Id = "TestId1", CreatedOn = DateTime.UtcNow, OwnerId = "TestOwnerId1" },
                new ChatRoom { Id = "TestId2", CreatedOn = DateTime.UtcNow, OwnerId = "TestOwnerId2" },
                new ChatRoom { Id = "TestId3", CreatedOn = DateTime.UtcNow, OwnerId = "TestOwnerId3" },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(chatRoomsList.AsQueryable());

            var service = new ChatService(repository.Object, null);
            Assert.Null(await service.CreateRoomAsync<RoomViewModel>("TestOwnerId1"));

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task CreateRoomAsyncGenericShouldWorkCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<ChatRoom>>();

            var chatRoomsList = new List<ChatRoom>
            {
                new ChatRoom { Id = "TestId1", CreatedOn = DateTime.UtcNow, OwnerId = "TestOwnerId1" },
                new ChatRoom { Id = "TestId2", CreatedOn = DateTime.UtcNow, OwnerId = "TestOwnerId2" },
                new ChatRoom { Id = "TestId3", CreatedOn = DateTime.UtcNow, OwnerId = "TestOwnerId3" },
            };

            var owner = new ApplicationUser { UserName = "TestOwnerUsername" };

            repository.Setup(r => r.AllAsNoTracking()).Returns(chatRoomsList.AsQueryable());
            repository.Setup(r => r.AddAsync(It.IsAny<ChatRoom>())).Callback((ChatRoom item) =>
            {
                item.Id = "TestId4";
                item.CreatedOn = new DateTime(2020, 12, 31, 12, 12, 12);
                item.Owner = owner;
                chatRoomsList.Add(item);
            });
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new ChatService(repository.Object, null);
            var returnResult = await service.CreateRoomAsync<RoomViewModel>("TestOwnerId4");
            Assert.Equal("TestId4", returnResult.Id);
            Assert.Equal("31-Dec-2020 12:12", returnResult.CreatedOn);
            Assert.Equal("TestOwnerUsername", returnResult.OwnerUsername);
            Assert.Equal(4, chatRoomsList.Count);
            Assert.Equal("TestOwnerId4", chatRoomsList.ElementAt(3).OwnerId);

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(2));
            repository.Verify(x => x.AddAsync(It.IsAny<ChatRoom>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task AddMessageAsyncGenericShouldReturnDefaultValueOfTWhenRoomDoesNotExistUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<ChatRoom>>();

            var chatRoomsList = new List<ChatRoom>
            {
                new ChatRoom { Id = "TestId1", CreatedOn = DateTime.UtcNow, OwnerId = "TestOwnerId1" },
                new ChatRoom { Id = "TestId2", CreatedOn = DateTime.UtcNow, OwnerId = "TestOwnerId2" },
                new ChatRoom { Id = "TestId3", CreatedOn = DateTime.UtcNow, OwnerId = "TestOwnerId3" },
            };

            repository.Setup(r => r.All()).Returns(chatRoomsList.AsQueryable());

            var service = new ChatService(repository.Object, null);
            Assert.Null(await service.AddMessageAsync<MessageViewModel>("TestId4", null, null));

            repository.Verify(x => x.All(), Times.Once);
        }

        [Fact]
        public async Task AddMessageAsyncGenericShouldWorkCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<ChatRoom>>();

            var chatRoomsList = new List<ChatRoom>
            {
                new ChatRoom { Id = "TestId1", CreatedOn = DateTime.UtcNow, OwnerId = "TestOwnerId1" },
                new ChatRoom { Id = "TestId2", CreatedOn = DateTime.UtcNow, OwnerId = "TestOwnerId2" },
                new ChatRoom { Id = "TestId3", CreatedOn = DateTime.UtcNow, OwnerId = "TestOwnerId3", Messages = new List<RoomMessage>() },
            };

            repository.Setup(r => r.All()).Returns(chatRoomsList.AsQueryable());
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new ChatService(repository.Object, null);
            var returnResult = await service.AddMessageAsync<MessageViewModel>("TestId3", "TestMessage", "TestSenderId");
            Assert.Equal("01-Jan-0001 00:00", returnResult.CreatedOn);
            Assert.Equal("TestMessage", returnResult.Message);
            Assert.Null(returnResult.SenderUsername);
            Assert.False(returnResult.IsByRoomOwner);
            Assert.Equal(1, chatRoomsList.ElementAt(2).Messages.Count);
            Assert.Equal("TestMessage", chatRoomsList.ElementAt(2).Messages.Last().Message);
            Assert.Equal("TestSenderId", chatRoomsList.ElementAt(2).Messages.Last().SenderId);

            repository.Verify(x => x.All(), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());
        }
    }
}
