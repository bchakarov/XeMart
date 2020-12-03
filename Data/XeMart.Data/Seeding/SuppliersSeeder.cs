namespace XeMart.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using XeMart.Data.Models;

    public class SuppliersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.Suppliers.Any())
            {
                // (Name, Price to home, Price to office, Is default)
                var suppliers = new List<(string, decimal, decimal, bool)>
                {
                    ("Econt", 25, 20, true),
                    ("Speedy", 30, 25, false),
                };

                foreach (var (name, priceToHome, priceToOffice, isDefault) in suppliers)
                {
                    dbContext.Add(new Supplier
                    {
                        Name = name,
                        PriceToHome = priceToHome,
                        PriceToOffice = priceToOffice,
                        IsDefault = isDefault,
                    });
                }

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
