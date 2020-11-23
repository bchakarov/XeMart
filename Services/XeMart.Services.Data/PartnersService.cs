namespace XeMart.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;

    using XeMart.Common;
    using XeMart.Data.Common.Repositories;
    using XeMart.Data.Models;
    using XeMart.Services.Mapping;

    public class PartnersService : IPartnersService
    {
        private const string LogoDirectoryPath = "partners";

        private readonly IDeletableEntityRepository<Partner> partnersRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IImagesService imagesService;

        public PartnersService(
            IDeletableEntityRepository<Partner> partnersRepository,
            UserManager<ApplicationUser> userManager,
            IImagesService imagesService)
        {
            this.partnersRepository = partnersRepository;
            this.userManager = userManager;
            this.imagesService = imagesService;
        }

        public async Task<bool> CreateAsync<T>(T model, string managerId)
        {
            var partner = AutoMapperConfig.MapperInstance.Map<Partner>(model);
            partner.ManagerId = managerId;

            if (this.partnersRepository.AllAsNoTracking().Any(x => x.ManagerId == partner.ManagerId))
            {
                return false;
            }

            await this.partnersRepository.AddAsync(partner);
            await this.partnersRepository.SaveChangesAsync();

            return true;
        }

        public IEnumerable<T> GetAllApproved<T>() =>
            this.partnersRepository.AllAsNoTracking()
            .Where(x => x.IsApproved)
            .To<T>().ToList();

        public IEnumerable<T> GetAllRequests<T>() =>
            this.partnersRepository.AllAsNoTracking()
            .Where(x => !x.IsApproved)
            .To<T>().ToList();

        public int GetRequestsCount() =>
            this.partnersRepository.AllAsNoTracking()
            .Where(x => !x.IsApproved)
            .ToList().Count;

        public async Task<bool> ApproveAsync(int id)
        {
            var partner = this.GetById(id);
            if (partner == null)
            {
                return false;
            }

            var manager = await this.userManager.FindByIdAsync(partner.ManagerId);
            await this.userManager.AddToRoleAsync(manager, GlobalConstants.PartnerRoleName);
            partner.IsApproved = true;
            partner.ApprovedOn = DateTime.UtcNow;
            this.partnersRepository.Update(partner);
            await this.partnersRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UnapproveAsync(int id)
        {
            var partner = this.GetById(id);
            if (partner == null)
            {
                return false;
            }

            var manager = await this.userManager.FindByIdAsync(partner.ManagerId);
            await this.userManager.RemoveFromRoleAsync(manager, GlobalConstants.PartnerRoleName);

            partner.IsApproved = false;
            partner.ApprovedOn = null;
            this.partnersRepository.Update(partner);
            await this.partnersRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditAsync<T>(T model, IFormFile logo)
        {
            var newPartner = AutoMapperConfig.MapperInstance.Map<Partner>(model);

            var foundPartner = this.GetById(newPartner.Id);
            if (foundPartner == null)
            {
                return false;
            }

            foundPartner.CompanyName = newPartner.CompanyName;
            foundPartner.CompanyUrl = newPartner.CompanyUrl;

            if (logo != null)
            {
                foundPartner.LogoUrl = await this.imagesService.UploadCloudinaryImageAsync(logo, LogoDirectoryPath);
            }

            this.partnersRepository.Update(foundPartner);
            await this.partnersRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var partner = this.GetById(id);
            if (partner == null)
            {
                return false;
            }

            this.partnersRepository.Delete(partner);
            await this.partnersRepository.SaveChangesAsync();

            return true;
        }

        public T GetById<T>(int id) =>
            this.partnersRepository.AllAsNoTracking()
            .Where(x => x.Id == id)
            .To<T>()
            .FirstOrDefault();

        public T GetByManagerId<T>(string managerId) =>
            this.partnersRepository.AllAsNoTracking()
            .Where(x => x.ManagerId == managerId)
            .To<T>()
            .FirstOrDefault();

        private Partner GetById(int id) =>
            this.partnersRepository.AllAsNoTracking()
            .FirstOrDefault(x => x.Id == id);
    }
}
