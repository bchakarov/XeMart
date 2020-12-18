namespace XeMart.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using XeMart.Data.Models;

    public class CategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.MainCategories.Any())
            {
                var mainCategories = new List<(string, string)>
                {
                    ("TV & Video", "fas fa-tv"),
                    ("PCs, Laptops & Gaming", "fas fa-laptop"),
                };

                foreach (var (name, icon) in mainCategories)
                {
                    await SeedMainCategoryAsync(dbContext, name, icon);
                }

                var subcategories = new List<(string, string, string)>
                {
                    ("TVs", "/images/subcategories/tvs.jpg", mainCategories.ElementAt(0).Item1),
                    ("Soundbars", "/images/subcategories/soundbars.jpg", mainCategories.ElementAt(0).Item1),
                    ("Home Theaters", "/images/subcategories/home-theaters.jpg", mainCategories.ElementAt(0).Item1),
                    ("Blu-Ray & DVD Players", "/images/subcategories/blu-rays.jpg", mainCategories.ElementAt(0).Item1),
                    ("TV Accessories", "/images/subcategories/tv-accessories.jpg", mainCategories.ElementAt(0).Item1),
                    ("PCs", "/images/subcategories/pcs.jpg", mainCategories.ElementAt(1).Item1),
                    ("Laptops", "/images/subcategories/laptops.jpg", mainCategories.ElementAt(1).Item1),
                    ("Monitors", "/images/subcategories/monitors.jpg", mainCategories.ElementAt(1).Item1),
                    ("Peripherals", "/images/subcategories/peripherals.jpg", mainCategories.ElementAt(1).Item1),
                };

                foreach (var (name, imageUrl, mainCategory) in subcategories)
                {
                    await SeedSubcategoryAsync(dbContext, name, imageUrl, mainCategory);
                }
            }
        }

        private static async Task SeedMainCategoryAsync(ApplicationDbContext dbContext, string categoryName, string icon)
        {
            dbContext.MainCategories.Add(new MainCategory
            {
                Name = categoryName,
                FontAwesomeIcon = icon,
            });

            await dbContext.SaveChangesAsync();
        }

        private static async Task SeedSubcategoryAsync(ApplicationDbContext dbContext, string categoryName, string imageUrl, string mainCategoryName)
        {
            var mainCategoryId = dbContext.MainCategories.FirstOrDefault(x => x.Name == mainCategoryName).Id;
            dbContext.Subcategories.Add(new Subcategory
            {
                Name = categoryName,
                ImageUrl = imageUrl,
                MainCategoryId = mainCategoryId,
            });

            await dbContext.SaveChangesAsync();
        }
    }
}
