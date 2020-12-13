namespace XeMart.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Internal;
    using Microsoft.AspNetCore.Identity;

    using Moq;

    using XeMart.Data.Common.Repositories;
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;
    using XeMart.Web.ViewModels;
    using XeMart.Web.ViewModels.Administration.Partners;
    using XeMart.Web.ViewModels.Partners;
    using Xunit;

    [Collection("Sequential")]
    public class PartnersServiceTests
    {
        [Fact]
        public void GetAllApprovedGenericShouldReturnCorrectCountUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Partner>>();

            var manager = new ApplicationUser { Email = "TestEmail" };
            var partners = new List<Partner>
            {
                new Partner { Id = 1, IsApproved = true, Manager = manager },
                new Partner { Id = 2, IsApproved = true, Manager = manager },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(partners.AsQueryable());

            var service = new PartnersService(repository.Object, null, null);
            Assert.Equal(2, service.GetAllApproved<PartnerViewModel>().Count());

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetAllApprovedGenericShouldMapCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Partner>>();

            var manager = new ApplicationUser { Email = "TestEmail" };
            var partners = new List<Partner>
            {
                new Partner { Id = 1, IsApproved = true, Manager = manager, CompanyName = "TestCompany", CompanyUrl = "TestUrl" },
                new Partner { Id = 2, IsApproved = true, Manager = manager },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(partners.AsQueryable());

            var service = new PartnersService(repository.Object, null, null);
            Assert.Equal(1, service.GetAllApproved<PartnerViewModel>().FirstOrDefault().Id);
            Assert.Equal("TestCompany", service.GetAllApproved<PartnerViewModel>().FirstOrDefault().CompanyName);
            Assert.Equal("TestUrl", service.GetAllApproved<PartnerViewModel>().FirstOrDefault().CompanyUrl);
            Assert.Equal("TestEmail", service.GetAllApproved<PartnerViewModel>().FirstOrDefault().ManagerEmail);

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(4));
        }

        [Fact]
        public void GetAllRequestsGenericShouldReturnCorrectCountUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Partner>>();

            var manager = new ApplicationUser { Email = "TestEmail" };
            var partners = new List<Partner>
            {
                new Partner { Id = 1, IsApproved = false, Manager = manager },
                new Partner { Id = 2, IsApproved = true, Manager = manager },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(partners.AsQueryable());

            var service = new PartnersService(repository.Object, null, null);
            Assert.Single(service.GetAllRequests<PartnerViewModel>());

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetAllRequestsGenericShouldMapCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Partner>>();

            var manager = new ApplicationUser { Email = "TestEmail" };
            var partners = new List<Partner>
            {
                new Partner { Id = 1, IsApproved = false, Manager = manager, CompanyName = "TestCompany", CompanyUrl = "TestUrl" },
                new Partner { Id = 2, IsApproved = true, Manager = manager },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(partners.AsQueryable());

            var service = new PartnersService(repository.Object, null, null);
            Assert.Equal(1, service.GetAllRequests<PartnerViewModel>().FirstOrDefault().Id);
            Assert.Equal("TestCompany", service.GetAllRequests<PartnerViewModel>().FirstOrDefault().CompanyName);
            Assert.Equal("TestUrl", service.GetAllRequests<PartnerViewModel>().FirstOrDefault().CompanyUrl);
            Assert.Equal("TestEmail", service.GetAllRequests<PartnerViewModel>().FirstOrDefault().ManagerEmail);

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(4));
        }

        [Fact]
        public void GetAllDeletedGenericShouldReturnCorrectCountUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Partner>>();

            var manager = new ApplicationUser { Email = "TestEmail" };
            var partners = new List<Partner>
            {
                new Partner { Id = 1, IsApproved = false, Manager = manager, IsDeleted = true, DeletedOn = DateTime.UtcNow },
                new Partner { Id = 2, IsApproved = true, Manager = manager },
            };

            repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(partners.AsQueryable());

            var service = new PartnersService(repository.Object, null, null);
            Assert.Single(service.GetAllDeleted<PartnerViewModel>());

            repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Once);
        }

        [Fact]
        public void GetAllDeletedGenericShouldMapCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Partner>>();

            var manager = new ApplicationUser { Email = "TestEmail" };
            var partners = new List<Partner>
            {
                new Partner { Id = 1, IsApproved = false, IsDeleted = true, DeletedOn = DateTime.UtcNow, Manager = manager, CompanyName = "TestCompany", CompanyUrl = "TestUrl" },
                new Partner { Id = 2, IsApproved = true, Manager = manager },
            };

            repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(partners.AsQueryable());

            var service = new PartnersService(repository.Object, null, null);
            Assert.Equal(1, service.GetAllDeleted<PartnerViewModel>().FirstOrDefault().Id);
            Assert.Equal("TestCompany", service.GetAllDeleted<PartnerViewModel>().FirstOrDefault().CompanyName);
            Assert.Equal("TestUrl", service.GetAllDeleted<PartnerViewModel>().FirstOrDefault().CompanyUrl);
            Assert.Equal("TestEmail", service.GetAllDeleted<PartnerViewModel>().FirstOrDefault().ManagerEmail);

            repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Exactly(4));
        }

        [Fact]
        public void GetRequestsCountShouldWorkCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Partner>>();

            var manager = new ApplicationUser { Email = "TestEmail" };
            var partners = new List<Partner>
            {
                new Partner { Id = 1, IsApproved = false, Manager = manager, CompanyName = "TestCompany", CompanyUrl = "TestUrl" },
                new Partner { Id = 2, IsApproved = true, Manager = manager },
                new Partner { Id = 3, IsApproved = true, Manager = manager },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(partners.AsQueryable());

            var service = new PartnersService(repository.Object, null, null);
            Assert.Equal(1, service.GetRequestsCount());

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetByIdGenericShouldMapCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Partner>>();

            var manager = new ApplicationUser { Email = "TestEmail" };
            var partners = new List<Partner>
            {
                new Partner { Id = 1, IsApproved = false, IsDeleted = true, DeletedOn = DateTime.UtcNow, Manager = manager, CompanyName = "TestCompany", CompanyUrl = "TestUrl" },
                new Partner { Id = 2, IsApproved = true, Manager = manager },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(partners.AsQueryable());

            var service = new PartnersService(repository.Object, null, null);
            Assert.Equal(1, service.GetById<PartnerViewModel>(1).Id);
            Assert.Equal("TestCompany", service.GetById<PartnerViewModel>(1).CompanyName);
            Assert.Equal("TestUrl", service.GetById<PartnerViewModel>(1).CompanyUrl);
            Assert.Equal("TestEmail", service.GetById<PartnerViewModel>(1).ManagerEmail);

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(4));
        }

        [Fact]
        public void GetByManagerIdGenericShouldMapCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Partner>>();

            var manager = new ApplicationUser { Email = "TestEmail", Id = "TestManagerId" };
            var partners = new List<Partner>
            {
                new Partner { Id = 1, IsApproved = false, IsDeleted = true, DeletedOn = DateTime.UtcNow, Manager = manager, ManagerId = manager.Id, CompanyName = "TestCompany", CompanyUrl = "TestUrl" },
                new Partner { Id = 2, IsApproved = true, Manager = manager },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(partners.AsQueryable());

            var service = new PartnersService(repository.Object, null, null);
            Assert.Equal(1, service.GetByManagerId<PartnerViewModel>("TestManagerId").Id);
            Assert.Equal("TestCompany", service.GetByManagerId<PartnerViewModel>("TestManagerId").CompanyName);
            Assert.Equal("TestUrl", service.GetByManagerId<PartnerViewModel>("TestManagerId").CompanyUrl);
            Assert.Equal("TestEmail", service.GetByManagerId<PartnerViewModel>("TestManagerId").ManagerEmail);

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(4));
        }

        [Fact]
        public async Task DeleteAsyncShouldReturnFalseWithInvalidPartnerIdUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Partner>>();

            var manager = new ApplicationUser { Email = "TestEmail", Id = "TestManagerId" };
            var partners = new List<Partner>
            {
                new Partner { Id = 1, IsApproved = false, Manager = manager, ManagerId = manager.Id, CompanyName = "TestCompany", CompanyUrl = "TestUrl" },
                new Partner { Id = 2, IsApproved = true, Manager = manager },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(partners.AsQueryable());

            var service = new PartnersService(repository.Object, null, null);

            Assert.False(await service.DeleteAsync(42));

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsyncShouldWorkCorrectlyUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<Partner>>();

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            var managers = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "TestManagerId1", Email = "TestEmail1", PartnerId = 1 },
                new ApplicationUser { Id = "TestManagerId2", Email = "TestEmail2" },
            };
            var partners = new List<Partner>
            {
                new Partner { Id = 1, IsApproved = false, Manager = managers.FirstOrDefault(), ManagerId = managers.FirstOrDefault().Id, CompanyName = "TestCompany", CompanyUrl = "TestUrl" },
                new Partner { Id = 2, IsApproved = true },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(partners.AsQueryable());
            repository.Setup(r => r.Delete(It.IsAny<Partner>())).Callback((Partner item) => partners.Remove(item));
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            userManager.Setup(r => r.FindByIdAsync(It.IsAny<string>()))
                .Returns(async (string userId) => await Task.FromResult<ApplicationUser>(managers.FirstOrDefault(x => x.Id == userId)));
            userManager.Setup(r => r.UpdateAsync(It.IsAny<ApplicationUser>()))
                .Returns(async (ApplicationUser user) =>
                {
                    var foundManager = managers.FirstOrDefault(x => x.Id == user.Id);
                    foundManager.PartnerId = user.PartnerId;
                    return await Task.FromResult(IdentityResult.Success);
                });

            var service = new PartnersService(repository.Object, userManager.Object, null);

            Assert.True(await service.DeleteAsync(1));
            Assert.Single(partners);
            Assert.Null(managers.FirstOrDefault().PartnerId);

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
            repository.Verify(x => x.Delete(It.IsAny<Partner>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());

            userManager.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Once);
            userManager.Verify(x => x.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Once);
        }

        [Fact]
        public async Task UndeleteAsyncShouldReturnFalseWithInvalidPartnerIdUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Partner>>();

            var manager = new ApplicationUser { Email = "TestEmail", Id = "TestManagerId" };
            var partners = new List<Partner>
            {
                new Partner { Id = 1, IsApproved = false, Manager = manager, ManagerId = manager.Id, CompanyName = "TestCompany", CompanyUrl = "TestUrl" },
                new Partner { Id = 2, IsApproved = true, Manager = manager },
            };

            repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(partners.AsQueryable());

            var service = new PartnersService(repository.Object, null, null);

            Assert.False(await service.UndeleteAsync(42));

            repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Once);
        }

        [Fact]
        public async Task UndeleteAsyncShouldWorkCorrectlyUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<Partner>>();

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            var managers = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "TestManagerId1", Email = "TestEmail1", PartnerId = null },
                new ApplicationUser { Id = "TestManagerId2", Email = "TestEmail2" },
            };
            var partners = new List<Partner>
            {
                new Partner { Id = 1, IsApproved = false, IsDeleted = true, DeletedOn = DateTime.UtcNow, Manager = managers.FirstOrDefault(), ManagerId = managers.FirstOrDefault().Id, CompanyName = "TestCompany", CompanyUrl = "TestUrl" },
                new Partner { Id = 2, IsApproved = true },
            };

            repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(partners.AsQueryable());
            repository.Setup(r => r.Undelete(It.IsAny<Partner>())).Callback((Partner item) =>
            {
                var foundPartner = partners.FirstOrDefault(x => x.Id == item.Id);
                foundPartner.IsDeleted = false;
                foundPartner.DeletedOn = null;
            });
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            userManager.Setup(r => r.FindByIdAsync(It.IsAny<string>()))
                .Returns(async (string userId) => await Task.FromResult<ApplicationUser>(managers.FirstOrDefault(x => x.Id == userId)));
            userManager.Setup(r => r.UpdateAsync(It.IsAny<ApplicationUser>()))
                .Returns(async (ApplicationUser user) =>
                {
                    var foundManager = managers.FirstOrDefault(x => x.Id == user.Id);
                    foundManager.PartnerId = user.PartnerId;
                    return await Task.FromResult(IdentityResult.Success);
                });

            var service = new PartnersService(repository.Object, userManager.Object, null);

            Assert.True(await service.UndeleteAsync(1));
            Assert.False(partners.FirstOrDefault().IsDeleted);
            Assert.Null(partners.FirstOrDefault().DeletedOn);
            Assert.Equal(1, managers.FirstOrDefault().PartnerId);

            repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Once);
            repository.Verify(x => x.Undelete(It.IsAny<Partner>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());

            userManager.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Once);
            userManager.Verify(x => x.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Once);
        }

        [Fact]
        public async Task ApproveAsyncShouldReturnFalseWithInvalidPartnerIdUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Partner>>();

            var manager = new ApplicationUser { Email = "TestEmail", Id = "TestManagerId" };
            var partners = new List<Partner>
            {
                new Partner { Id = 1, IsApproved = false, Manager = manager, ManagerId = manager.Id, CompanyName = "TestCompany", CompanyUrl = "TestUrl" },
                new Partner { Id = 2, IsApproved = true, Manager = manager },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(partners.AsQueryable());

            var service = new PartnersService(repository.Object, null, null);

            Assert.False(await service.ApproveAsync(42));

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task ApproveAsyncShouldWorkCorrectlyUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<Partner>>();

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            var roles = new List<ApplicationRole>
            {
                new ApplicationRole { Id = "1", Name = "Admin" },
                new ApplicationRole { Id = "2", Name = "Partner" },
            };

            var managers = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "TestManagerId1", Email = "TestEmail1", PartnerId = 1 },
                new ApplicationUser { Id = "TestManagerId2", Email = "TestEmail2" },
            };
            var partners = new List<Partner>
            {
                new Partner { Id = 1, IsApproved = false, Manager = managers.FirstOrDefault(), ManagerId = managers.FirstOrDefault().Id, CompanyName = "TestCompany", CompanyUrl = "TestUrl" },
                new Partner { Id = 2, IsApproved = true },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(partners.AsQueryable());
            repository.Setup(r => r.Update(It.IsAny<Partner>())).Callback((Partner item) =>
            {
                var foundPartner = partners.FirstOrDefault(x => x.Id == item.Id);
                foundPartner.IsApproved = item.IsApproved;
                foundPartner.ApprovedOn = item.ApprovedOn;
            });
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            userManager.Setup(r => r.FindByIdAsync(It.IsAny<string>()))
                .Returns(async (string userId) => await Task.FromResult<ApplicationUser>(managers.FirstOrDefault(x => x.Id == userId)));
            userManager.Setup(r => r.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .Returns(async (ApplicationUser user, string roleName) =>
                {
                    var foundManager = managers.FirstOrDefault(x => x.Id == user.Id);
                    var foundRole = roles.FirstOrDefault(x => x.Name == roleName);
                    foundManager.Roles.Add(new IdentityUserRole<string> { RoleId = foundRole.Id });
                    return await Task.FromResult(IdentityResult.Success);
                });

            var service = new PartnersService(repository.Object, userManager.Object, null);

            Assert.True(await service.ApproveAsync(1));
            Assert.True(partners.FirstOrDefault(x => x.Id == 1).IsApproved);
            Assert.Single(managers.FirstOrDefault(x => x.Id == "TestManagerId1").Roles);

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
            repository.Verify(x => x.Update(It.IsAny<Partner>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());

            userManager.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Once);
            userManager.Verify(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task UnapproveAsyncShouldReturnFalseWithInvalidPartnerIdUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Partner>>();

            var manager = new ApplicationUser { Email = "TestEmail", Id = "TestManagerId" };
            var partners = new List<Partner>
            {
                new Partner { Id = 1, IsApproved = false, Manager = manager, ManagerId = manager.Id, CompanyName = "TestCompany", CompanyUrl = "TestUrl" },
                new Partner { Id = 2, IsApproved = true, Manager = manager },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(partners.AsQueryable());

            var service = new PartnersService(repository.Object, null, null);

            Assert.False(await service.UnapproveAsync(42));

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task UnapproveAsyncShouldWorkCorrectlyUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<Partner>>();

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            var roles = new List<ApplicationRole>
            {
                new ApplicationRole { Id = "1", Name = "Admin" },
                new ApplicationRole { Id = "2", Name = "Partner" },
            };

            var managers = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "TestManagerId1", Email = "TestEmail1", PartnerId = 1 },
                new ApplicationUser { Id = "TestManagerId2", Email = "TestEmail2" },
            };
            managers.FirstOrDefault().Roles.Add(new IdentityUserRole<string> { RoleId = "1" });

            var partners = new List<Partner>
            {
                new Partner { Id = 1, IsApproved = true, Manager = managers.FirstOrDefault(), ManagerId = managers.FirstOrDefault().Id, CompanyName = "TestCompany", CompanyUrl = "TestUrl" },
                new Partner { Id = 2, IsApproved = true },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(partners.AsQueryable());
            repository.Setup(r => r.Update(It.IsAny<Partner>())).Callback((Partner item) =>
            {
                var foundPartner = partners.FirstOrDefault(x => x.Id == item.Id);
                foundPartner.IsApproved = item.IsApproved;
                foundPartner.ApprovedOn = item.ApprovedOn;
            });
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            userManager.Setup(r => r.FindByIdAsync(It.IsAny<string>()))
                .Returns(async (string userId) => await Task.FromResult<ApplicationUser>(managers.FirstOrDefault(x => x.Id == userId)));
            userManager.Setup(r => r.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .Returns(async (ApplicationUser user, string roleName) =>
                {
                    var foundManager = managers.FirstOrDefault(x => x.Id == user.Id);
                    var foundRole = roles.FirstOrDefault(x => x.Name == roleName);
                    foundManager.Roles.Remove(foundManager.Roles.FirstOrDefault(x => x.RoleId == "1"));
                    return await Task.FromResult(IdentityResult.Success);
                });

            var service = new PartnersService(repository.Object, userManager.Object, null);

            Assert.True(await service.UnapproveAsync(1));
            Assert.False(partners.FirstOrDefault(x => x.Id == 1).IsApproved);
            Assert.Empty(managers.FirstOrDefault(x => x.Id == "TestManagerId1").Roles);

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
            repository.Verify(x => x.Update(It.IsAny<Partner>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());

            userManager.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Once);
            userManager.Verify(x => x.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsyncGenericShouldReturnFalseWithExistingOwnerIdUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Partner>>();

            var manager = new ApplicationUser { Email = "TestEmail", Id = "TestManagerId" };
            var partners = new List<Partner>
            {
                new Partner { Id = 1, IsApproved = false, Manager = manager, ManagerId = manager.Id, CompanyName = "TestCompany", CompanyUrl = "TestUrl" },
                new Partner { Id = 2, IsApproved = true, Manager = manager },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(partners.AsQueryable());

            var service = new PartnersService(repository.Object, null, null);

            var model = new CreatePartnerInputViewModel { };
            Assert.False(await service.CreateAsync<CreatePartnerInputViewModel>(model, "TestManagerId"));

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task CreateAsyncGenericShouldIncreaseCountUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Partner>>();

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            var managers = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "TestManagerId1", Email = "TestEmail1", PartnerId = 1 },
                new ApplicationUser { Id = "TestManagerId2", Email = "TestEmail2" },
            };

            var partners = new List<Partner>
            {
                new Partner { Id = 1, IsApproved = true, Manager = managers.FirstOrDefault(), ManagerId = managers.FirstOrDefault().Id, CompanyName = "TestCompany", CompanyUrl = "TestUrl" },
                new Partner { Id = 2, IsApproved = true },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(partners.AsQueryable());
            repository.Setup(r => r.AddAsync(It.IsAny<Partner>())).Callback((Partner item) => partners.Add(item));
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            userManager.Setup(r => r.FindByIdAsync(It.IsAny<string>()))
                .Returns(async (string userId) => await Task.FromResult<ApplicationUser>(managers.FirstOrDefault(x => x.Id == userId)));
            userManager.Setup(r => r.UpdateAsync(It.IsAny<ApplicationUser>()))
                .Returns(async (ApplicationUser user) =>
                {
                    var foundManager = managers.FirstOrDefault(x => x.Id == user.Id);
                    foundManager.PartnerId = user.PartnerId;
                    return await Task.FromResult(IdentityResult.Success);
                });

            var service = new PartnersService(repository.Object, userManager.Object, null);

            var model = new CreatePartnerInputViewModel { };
            Assert.True(await service.CreateAsync<CreatePartnerInputViewModel>(model, "TestManagerId2"));
            Assert.Equal(3, partners.Count);

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
            repository.Verify(x => x.AddAsync(It.IsAny<Partner>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());

            userManager.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Once);
            userManager.Verify(x => x.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsyncGenericShouldWorkCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Partner>>();

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            var managers = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "TestManagerId1", Email = "TestEmail1", PartnerId = 1 },
                new ApplicationUser { Id = "TestManagerId2", Email = "TestEmail2" },
            };

            var partners = new List<Partner>
            {
                new Partner { Id = 1, IsApproved = true, Manager = managers.FirstOrDefault(), ManagerId = managers.FirstOrDefault().Id, CompanyName = "TestCompany", CompanyUrl = "TestUrl" },
                new Partner { Id = 2, IsApproved = true },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(partners.AsQueryable());
            repository.Setup(r => r.AddAsync(It.IsAny<Partner>())).Callback((Partner item) => partners.Add(item));
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            userManager.Setup(r => r.FindByIdAsync(It.IsAny<string>()))
                .Returns(async (string userId) => await Task.FromResult<ApplicationUser>(managers.FirstOrDefault(x => x.Id == userId)));
            userManager.Setup(r => r.UpdateAsync(It.IsAny<ApplicationUser>()))
                .Returns(async (ApplicationUser user) =>
                {
                    var foundManager = managers.FirstOrDefault(x => x.Id == user.Id);
                    foundManager.PartnerId = user.PartnerId;
                    return await Task.FromResult(IdentityResult.Success);
                });

            var service = new PartnersService(repository.Object, userManager.Object, null);

            var model = new CreatePartnerInputViewModel
            {
                CompanyName = "TestCompany3",
                CompanyUrl = "TestUrl3",
            };
            Assert.True(await service.CreateAsync<CreatePartnerInputViewModel>(model, "TestManagerId2"));
            Assert.Equal("TestCompany3", partners.Last().CompanyName);
            Assert.Equal("TestUrl3", partners.Last().CompanyUrl);
            Assert.Equal("TestManagerId2", partners.Last().ManagerId);

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
            repository.Verify(x => x.AddAsync(It.IsAny<Partner>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());

            userManager.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Once);
            userManager.Verify(x => x.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Once);
        }

        [Fact]
        public async Task EditAsyncGenericShouldReturnFalseWithInvalidPartnerIdUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Partner>>();

            var manager = new ApplicationUser { Email = "TestEmail", Id = "TestManagerId" };
            var partners = new List<Partner>
            {
                new Partner { Id = 1, IsApproved = false, Manager = manager, ManagerId = manager.Id, CompanyName = "TestCompany", CompanyUrl = "TestUrl" },
                new Partner { Id = 2, IsApproved = true, Manager = manager },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(partners.AsQueryable());

            var service = new PartnersService(repository.Object, null, null);

            var model = new EditPartnerViewModel { };
            Assert.False(await service.EditAsync<EditPartnerViewModel>(model, null));

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task EditAsyncGenericShouldWorkCorrectlyWithNoLogoUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Partner>>();

            var manager = new ApplicationUser { Email = "TestEmail", Id = "TestManagerId" };
            var partners = new List<Partner>
            {
                new Partner { Id = 1, IsApproved = false, Manager = manager, ManagerId = manager.Id, CompanyName = "TestCompany", CompanyUrl = "TestUrl" },
                new Partner { Id = 2, IsApproved = true, Manager = manager },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(partners.AsQueryable());
            repository.Setup(r => r.Update(It.IsAny<Partner>())).Callback((Partner item) =>
            {
                var foundPartner = partners.FirstOrDefault(x => x.Id == item.Id);
                foundPartner.CompanyName = item.CompanyName;
                foundPartner.CompanyUrl = item.CompanyUrl;
            });
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new PartnersService(repository.Object, null, null);

            var model = new EditPartnerViewModel { Id = 1, CompanyName = "TestCompanyEdited", CompanyUrl = "TestUrlEdited" };
            Assert.True(await service.EditAsync<EditPartnerViewModel>(model, null));
            Assert.Equal("TestCompanyEdited", partners.FirstOrDefault(x => x.Id == 1).CompanyName);
            Assert.Equal("TestUrlEdited", partners.FirstOrDefault(x => x.Id == 1).CompanyUrl);

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
            repository.Verify(x => x.Update(It.IsAny<Partner>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task EditAsyncGenericShouldWorkCorrectlyWithLogoUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Partner>>();

            var imageService = new Mock<IImagesService>();

            var manager = new ApplicationUser { Email = "TestEmail", Id = "TestManagerId" };
            var partners = new List<Partner>
            {
                new Partner { Id = 1, IsApproved = false, Manager = manager, ManagerId = manager.Id, CompanyName = "TestCompany", CompanyUrl = "TestUrl" },
                new Partner { Id = 2, IsApproved = true, Manager = manager },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(partners.AsQueryable());
            repository.Setup(r => r.Update(It.IsAny<Partner>())).Callback((Partner item) =>
            {
                var foundPartner = partners.FirstOrDefault(x => x.Id == item.Id);
                foundPartner.CompanyName = item.CompanyName;
                foundPartner.CompanyUrl = item.CompanyUrl;
                foundPartner.LogoUrl = item.LogoUrl;
            });
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            imageService.Setup(r => r.UploadCloudinaryImageAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
                .Returns(async (IFormFile image, string path) => await Task.FromResult(path + "/" + image.FileName));

            var service = new PartnersService(repository.Object, null, imageService.Object);

            var model = new EditPartnerViewModel
            {
                Id = 1,
                CompanyName = "TestCompanyEdited",
                CompanyUrl = "TestUrlEdited",
            };

            Assert.True(await service.EditAsync<EditPartnerViewModel>(model, new FormFile(null, 0, 0, "test", "test.png")));
            Assert.Equal("TestCompanyEdited", partners.FirstOrDefault(x => x.Id == 1).CompanyName);
            Assert.Equal("TestUrlEdited", partners.FirstOrDefault(x => x.Id == 1).CompanyUrl);
            Assert.Equal("partners/test.png", partners.FirstOrDefault(x => x.Id == 1).LogoUrl);

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
            repository.Verify(x => x.Update(It.IsAny<Partner>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());

            imageService.Verify(x => x.UploadCloudinaryImageAsync(It.IsAny<IFormFile>(), It.IsAny<string>()), Times.Once);
        }
    }
}
