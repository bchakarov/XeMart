namespace XeMart.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using XeMart.Data.Models;

    public class HomePageSlidesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.HomePageSlides.Any())
            {
                var slides = new List<HomePageSlide>
                {
                    new HomePageSlide
                    {
                        ImageUrl = "https://xemart.blob.core.windows.net/images/sm-1.png",
                        Description = "@<p><span style=\"color :#e03e2d;\">Smartphone deal</span></p>" +
                            "<p><span style=\"font-size: 36pt;\"><strong> Huawei Honor 8x | 8x Max</strong></span></p>" +
                            "<p>The Smart Power In Your Hand </p>",
                        LinkUrl = "#",
                        Position = 1,
                    },
                    new HomePageSlide
                    {
                        ImageUrl = "https://xemart.blob.core.windows.net/images/sm-2.png",
                        Description = "@<p><span style=\"color :#e03e2d;\">Smartphone deal</span></p>" +
                            "<p><span style=\"font-size: 36pt;\"><strong> Samsung Galaxy S9+</strong></span></p>" +
                            "<p>The Smart Power In Your Hand </p>",
                        LinkUrl = "#",
                        Position = 2,
                    },
                };

                foreach (var slide in slides)
                {
                    await dbContext.HomePageSlides.AddAsync(slide);
                }

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
