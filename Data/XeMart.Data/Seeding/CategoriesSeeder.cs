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
                    ("Phones & Tables", "fas fa-mobile-alt"),
                };

                foreach (var (name, icon) in mainCategories)
                {
                    await SeedMainCategoryAsync(dbContext, name, icon);
                }

                var subcategories = new List<(string, string, string)>
                {
                    ("TVs", "/images/subcategories/8854895951902.jpg", mainCategories.ElementAt(0).Item1),
                    ("Soundbars", "/images/subcategories/8854896214046.jpg", mainCategories.ElementAt(0).Item1),
                    ("Home Theaters", "/images/subcategories/8854896082974.jpg", mainCategories.ElementAt(0).Item1),
                    ("Blu-Ray & DVD Players", "/images/subcategories/8854896279582.jpg", mainCategories.ElementAt(0).Item1),
                    ("TV Accessories", "/images/subcategories/8854896672798.jpg", mainCategories.ElementAt(0).Item1),
                    ("PCs", "/images/subcategories/10649289621534.jpg", mainCategories.ElementAt(1).Item1),
                    ("Laptops", "/images/subcategories/10649182306334.jpg", mainCategories.ElementAt(1).Item1),
                    ("Monitors", "/images/subcategories/10649184895006.jpg", mainCategories.ElementAt(1).Item1),
                    ("Game Consoles", "/images/subcategories/10629502337054.jpg", mainCategories.ElementAt(1).Item1),
                    ("Game Console Accessories", "/images/subcategories/8854902898718.jpg", mainCategories.ElementAt(1).Item1),
                    ("Mobile Phones", "/images/subcategories/11501714079774.jpg", mainCategories.ElementAt(2).Item1),
                    ("Tablets", "/images/subcategories/11501719060510.jpg", mainCategories.ElementAt(2).Item1),
                    ("Headphones", "/images/subcategories/12936450080798.jpg", mainCategories.ElementAt(2).Item1),
                    ("Accessories", "/images/subcategories/8901743247390.jpg", mainCategories.ElementAt(2).Item1),
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
