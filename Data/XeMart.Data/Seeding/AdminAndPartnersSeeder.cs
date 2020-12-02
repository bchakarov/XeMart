namespace XeMart.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    using XeMart.Common;
    using XeMart.Data.Models;

    public class AdminAndPartnersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.Users.Any())
            {
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                await SeedUserAsync(dbContext, userManager, "test@example.com", "test@example.com", GlobalConstants.AdministratorRoleName);

                var logos = new string[]
                {
                    "https://res.cloudinary.com/dkhn1ww1h/image/upload/v1606080469/partners/agdxh61limopbbh3paur.png",
                    "https://res.cloudinary.com/dkhn1ww1h/image/upload/v1606080426/partners/korogi1rxtalgzxbktag.png",
                    "https://res.cloudinary.com/dkhn1ww1h/image/upload/v1606080382/partners/xwkuko9fadssl1ouanmw.png",
                    "https://res.cloudinary.com/dkhn1ww1h/image/upload/v1606080340/partners/zs4kcxg8glixjwdhmsxw.png",
                    "https://res.cloudinary.com/dkhn1ww1h/image/upload/v1606080269/partners/yigqwhip3gwviakitz9p.png",
                    "https://res.cloudinary.com/dkhn1ww1h/image/upload/v1606080181/partners/hxo541gpgy0m35xrszhw.png",
                    "https://res.cloudinary.com/dkhn1ww1h/image/upload/v1606079973/partners/xwn8khmmrcif0etfpayj.png",
                };

                for (int i = 1; i <= 7; i++)
                {
                    var email = $"partner{i}@example.com";
                    var companyName = $"Company {i}";
                    var companyUrl = "https://google.com";

                    await SeedUserAsync(dbContext, userManager, email, email, GlobalConstants.PartnerRoleName);
                    await AddApprovedPartnerAsync(userManager, dbContext, email, companyName, companyUrl, logos[i - 1]);
                }
            }
        }

        private static async Task SeedUserAsync(ApplicationDbContext db, UserManager<ApplicationUser> userManager, string email, string password, string roleName)
        {
            var shoppingCart = new ShoppingCart();

            var user = new ApplicationUser()
            {
                Email = email,
                UserName = email,
                ShoppingCart = shoppingCart,
            };

            await userManager.CreateAsync(user, password);
            shoppingCart.User = user;
            await db.SaveChangesAsync();

            if (!string.IsNullOrEmpty(roleName))
            {
                var createdUser = await userManager.FindByNameAsync(email);
                await userManager.AddToRoleAsync(createdUser, roleName);
            }
        }

        private static async Task AddApprovedPartnerAsync(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext, string email, string companyName, string companyUrl, string logoUrl)
        {
            var manager = await userManager.FindByNameAsync(email);
            var partner = new Partner
            {
                CompanyName = companyName,
                CompanyUrl = companyUrl,
                IsApproved = true,
                ApprovedOn = DateTime.UtcNow,
                Manager = manager,
                LogoUrl = logoUrl,
            };

            dbContext.Partners.Add(partner);
            await dbContext.SaveChangesAsync();

            manager.PartnerId = partner.Id;
            await userManager.UpdateAsync(manager);
        }
    }
}
