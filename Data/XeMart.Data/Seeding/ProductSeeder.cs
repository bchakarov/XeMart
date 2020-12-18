namespace XeMart.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using XeMart.Data.Models;

    public class ProductSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.Products.Any())
            {
                var tvsSubcategoryId = dbContext.Subcategories.FirstOrDefault(x => x.Name == "TVS").Id;
                var tvsToAdd = new List<Product>
                {
                    new Product
                    {
                        Name = "TV LG 65NANO813NA 4K Ultra HD LED SMART TV, WEBOS, 65.0 \"",
                        Price = 1200M,
                        Description = "TV LG 65NANO813NA 4K Ultra HD LED SMART TV, WEBOS, 65.0 \"",
                        SubcategoryId = tvsSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/13266772099102.jpg" } },
                    },
                    new Product
                    {
                        Name = "TV LG OLED55CX3LA 4K Ultra HD OLED SMART TV, WEBOS, 55.0 \"",
                        Price = 1699.99M,
                        Description = "TV LG OLED55CX3LA 4K Ultra HD OLED SMART TV, WEBOS, 55.0 \"",
                        SubcategoryId = tvsSubcategoryId,
                        Images = new List<ProductImage>
                        {
                            new ProductImage { ImageUrl = "/images/products/12923803598878.jpg" },
                            new ProductImage { ImageUrl = "/images/products/12923803795486.jpg" },
                            new ProductImage { ImageUrl = "/images/products/12923802943518.jpg" },
                        },
                    },
                    new Product
                    {
                        Name = "TV SONY KD-65XH8096 4K Ultra HD LED SMART TV, ANDROID TV, 65.0 \"",
                        Price = 1100M,
                        Description = "TV SONY KD-65XH8096 4K Ultra HD LED SMART TV, ANDROID TV, 65.0 \"",
                        SubcategoryId = tvsSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/13709491240990.jpg" } },
                    },
                    new Product
                    {
                        Name = "TV SAMSUNG QE-55Q70R 4K Ultra HD QLED SMART TV, TIZEN, 55.0 \"",
                        Price = 1000M,
                        Description = "TV SAMSUNG QE-55Q70R 4K Ultra HD QLED SMART TV, TIZEN, 55.0 \"",
                        SubcategoryId = tvsSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/11563870126110.jpg" } },
                    },
                    new Product
                    {
                        Name = "TV PANASONIC TX-55GZ960E 4K Ultra HD OLED SMART TV, 55.0 \"",
                        Price = 1300M,
                        Description = "TV PANASONIC TX-55GZ960E 4K Ultra HD OLED SMART TV, 55.0 \"",
                        SubcategoryId = tvsSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/11704001953822.jpg" } },
                    },
                    new Product
                    {
                        Name = "TV PHILIPS 58PUS7855 4K Ultra HD LED SMART TV, SAPHI, 58.0 \"",
                        Price = 575M,
                        Description = "TV PHILIPS 58PUS7855 4K Ultra HD LED SMART TV, SAPHI, 58.0 \"",
                        SubcategoryId = tvsSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/13216680443934.jpg" } },
                    },
                    new Product
                    {
                        Name = "TV SHARP 55BL3EA 4K Ultra HD LED SMART TV, ANDROID, 55.0 \"",
                        Price = 500M,
                        Description = "TV SHARP 55BL3EA 4K Ultra HD LED SMART TV, ANDROID, 55.0 \"",
                        SubcategoryId = tvsSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/12104399814686.jpg" } },
                    },
                    new Product
                    {
                        Name = "TV PANASONIC TX-50GX700E 4K Ultra HD LED SMART TV, 50.0 \"",
                        Price = 540M,
                        Description = "TV PANASONIC TX-50GX700E 4K Ultra HD LED SMART TV, 50.0 \"",
                        SubcategoryId = tvsSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/11703997890590.jpg" } },
                    },
                    new Product
                    {
                        Name = "TV SAMSUNG QE-75Q70T 4K Ultra HD QLED SMART TV, TIZEN, 75.0 \"",
                        Price = 2060.99M,
                        Description = "TV SAMSUNG QE-75Q70T 4K Ultra HD QLED SMART TV, TIZEN, 75.0 \"",
                        SubcategoryId = tvsSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/13413719179294.jpg" } },
                    },
                    new Product
                    {
                        Name = "TV PHILIPS 58PUS7555 4K Ultra HD LED SMART TV, SAPHI, 58.0 \"",
                        Price = 600M,
                        Description = "TV PHILIPS 58PUS7555 4K Ultra HD LED SMART TV, SAPHI, 58.0 \"",
                        SubcategoryId = tvsSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/12990378803230.jpg" } },
                    },
                    new Product
                    {
                        Name = "TV SONY KD-65AG9 4K Ultra HD OLED SMART TV, ANDROID TV, 65 \"",
                        Price = 3459.99M,
                        Description = "TV SONY KD-65AG9 4K Ultra HD OLED SMART TV, ANDROID TV, 65 \"",
                        SubcategoryId = tvsSubcategoryId,
                        Images = new List<ProductImage>
                        {
                            new ProductImage { ImageUrl = "/images/products/954519.sony-bravia-kd-65ag9b.jpg" },
                            new ProductImage { ImageUrl = "/images/products/954504.sony-bravia-kd-65ag9b.jpg" },
                            new ProductImage { ImageUrl = "/images/products/599073240.sony-bravia-kd-65ag9b.jpg" },
                            new ProductImage { ImageUrl = "/images/products/954507.sony-bravia-kd-65ag9b.jpg" },
                            new ProductImage { ImageUrl = "/images/products/954522.sony-bravia-kd-65ag9b.jpg" },
                        },
                    },
                    new Product
                    {
                        Name = "TV SONY KD-49XH8596 4K Ultra HD LED SMART TV, ANDROID TV, 49.0 \"",
                        Price = 800M,
                        Description = "TV SONY KD-49XH8596 4K Ultra HD LED SMART TV, ANDROID TV, 49.0 \"",
                        SubcategoryId = tvsSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/13457949786142.jpg" } },
                    },
                    new Product
                    {
                        Name = "TV SAMSUNG UE-55TU7092 4K Ultra HD LED SMART TV, TIZEN, 55.0 \"",
                        Price = 549.99M,
                        Description = "TV SAMSUNG UE-55TU7092 4K Ultra HD LED SMART TV, TIZEN, 55.0 \"",
                        SubcategoryId = tvsSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/13640711176222.jpg" } },
                    },
                    new Product
                    {
                        Name = "TV PANASONIC TX-58GX830E 4K Ultra HD LED SMART TV, 58.0 \"",
                        Price = 649.99M,
                        Description = "TV PANASONIC TX-58GX830E 4K Ultra HD LED SMART TV, 58.0 \"",
                        SubcategoryId = tvsSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/11943148453918.jpg" } },
                    },
                };

                var soundbarsSubcategoryId = dbContext.Subcategories.FirstOrDefault(x => x.Name == "Soundbars").Id;
                var soundbarsToAdd = new List<Product>
                {
                    new Product
                    {
                        Name = "PHILIPS HTL1510B/12 70 W, BLUETOOTH",
                        Price = 120M,
                        Description = "PHILIPS HTL1510B/12 70 W, BLUETOOTH",
                        SubcategoryId = soundbarsSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/soundbar-philips.jpg" } },
                    },
                    new Product
                    {
                        Name = "SAMSUNG HW-T400 40 W, BLUETOOTH",
                        Price = 125M,
                        Description = "SAMSUNG HW-T400 40 W, BLUETOOTH",
                        SubcategoryId = soundbarsSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/soundbar-samsung.jpg" } },
                    },
                    new Product
                    {
                        Name = "SONY HT-S20R 400 W, BLUETOOTH",
                        Price = 199.99M,
                        Description = "SONY HT-S20R 400 W, BLUETOOTH",
                        SubcategoryId = soundbarsSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/soundbar-sony.jpg" } },
                    },
                    new Product
                    {
                        Name = "LG SN5Y 400 W, BLUETOOTH, WIRELESS SUBWOOFER",
                        Price = 249.99M,
                        Description = "LG SN5Y 400 W, BLUETOOTH, WIRELESS SUBWOOFER",
                        SubcategoryId = soundbarsSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/soundbar-lg.jpg" } },
                    },
                };

                var homeTheatersSubcategoryId = dbContext.Subcategories.FirstOrDefault(x => x.Name == "Home Theaters").Id;
                var homeTheatersToAdd = new List<Product>
                {
                    new Product
                    {
                        Name = "SONY BDV-E4100 1000 W, WI-FI",
                        Price = 400M,
                        Description = "SONY BDV-E4100 1000 W, WI-FI",
                        SubcategoryId = homeTheatersSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/hometheater-sony.jpg" } },
                    },
                    new Product
                    {
                        Name = "HARMAN KARDON BDS-380 330 W, WI-FI",
                        Price = 499.99M,
                        Description = "HARMAN KARDON BDS-380 330 W, WI-FI",
                        SubcategoryId = homeTheatersSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/hometheater-harman.jpg" } },
                    },
                };

                var bluRaysSubcategoryId = dbContext.Subcategories.FirstOrDefault(x => x.Name == "Blu-Ray & DVD Players").Id;
                var bluRaysToAdd = new List<Product>
                {
                    new Product
                    {
                        Name = "BLU-RAY PLAYER SONY BDP-S6700",
                        Price = 99.99M,
                        Description = "BLU-RAY PLAYER SONY BDP-S6700",
                        SubcategoryId = bluRaysSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/bluray-sony.jpg" } },
                    },
                };

                var tvsAccessoriesSubcategoryId = dbContext.Subcategories.FirstOrDefault(x => x.Name == "TV Accessories").Id;
                var tvsAccessoriesToAdd = new List<Product>
                {
                    new Product
                    {
                        Name = "TV WALLMOUNT THUNDER STTV TTS-P113",
                        Price = 30.99M,
                        Description = "TV WALLMOUNT THUNDER STTV TTS-P113",
                        SubcategoryId = tvsAccessoriesSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/wallmount-thunder.jpg" } },
                    },
                    new Product
                    {
                        Name = "TV WALLMOUNT VOGELS THIN 245",
                        Price = 125.99M,
                        Description = "TV WALLMOUNT VOGELS THIN 245",
                        SubcategoryId = tvsAccessoriesSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/wallmount-vogels.jpg" } },
                    },
                    new Product
                    {
                        Name = "HDMI CABLE HAMA 83081/122105 3.0 M",
                        Price = 12.99M,
                        Description = "HDMI CABLE HAMA 83081/122105 3.0 M",
                        SubcategoryId = tvsAccessoriesSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/hdmicable-hama.jpg" } },
                    },
                    new Product
                    {
                        Name = "HDMI CABLE PHILIPS SWV2434W/10 5M",
                        Price = 20.99M,
                        Description = "HDMI CABLE HAMA 83081/122105 3.0 M",
                        SubcategoryId = tvsAccessoriesSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/hdmicable-philips.jpg" } },
                    },
                    new Product
                    {
                        Name = "HAMA 42553 HDMI SWITCHER 2/1",
                        Price = 15.99M,
                        Description = "HAMA 42553 HDMI SWITCHER 2/1",
                        SubcategoryId = tvsAccessoriesSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/hdmiswitcher-hama.jpg" } },
                    },
                    new Product
                    {
                        Name = "CLEANING KIT DIVA HN-4135N",
                        Price = 5.99M,
                        Description = "CLEANING KIT DIVA HN-4135N",
                        SubcategoryId = tvsAccessoriesSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/cleaningkit-diva.jpg" } },
                    },
                    new Product
                    {
                        Name = "FOAM CLEANER VIVANCO 39755",
                        Price = 5.99M,
                        Description = "FOAM CLEANER VIVANCO 39755",
                        SubcategoryId = tvsAccessoriesSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/foamcleaner-vivanco.jpg" } },
                    },
                };

                var pcsSubcategoryId = dbContext.Subcategories.FirstOrDefault(x => x.Name == "PCs").Id;
                var pcsToAdd = new List<Product>
                {
                    new Product
                    {
                        Name = "PC ACER NITRO N50-600",
                        Price = 1000M,
                        Description = "PC ACER NITRO N50-600",
                        SubcategoryId = pcsSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/pc-acer.jpg" } },
                    },
                    new Product
                    {
                        Name = "PC GPLAY PURITAS",
                        Price = 1200M,
                        Description = "PC GPLAY PURITAS",
                        SubcategoryId = pcsSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/pc-gplay.jpg" } },
                    },
                    new Product
                    {
                        Name = "PC LENOVO T540-15ICK 90LW004SRM",
                        Price = 1350M,
                        Description = "PC LENOVO T540-15ICK 90LW004SRM",
                        SubcategoryId = pcsSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/pc-lenovo.jpg" } },
                    },
                };

                var laptopsSubcategoryId = dbContext.Subcategories.FirstOrDefault(x => x.Name == "Laptops").Id;
                var laptopsToAdd = new List<Product>
                {
                    new Product
                    {
                        Name = "Laptop Aspire 5 ACER A515-56-38ZC NX.A18EX.00E 15.6 \"",
                        Price = 999.99M,
                        Description = "Laptop Aspire 5 ACER A515-56-38ZC NX.A18EX.00E 15.6 \"",
                        SubcategoryId = laptopsSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/laptop-acer.jpg" } },
                    },
                    new Product
                    {
                        Name = "Laptop LENOVO Ultraslim IdeaPad 5 14IIL05 81YH00CLBM 14.0 \"",
                        Price = 1199.99M,
                        Description = "Laptop LENOVO Ultraslim IdeaPad 5 14IIL05 81YH00CLBM 14.0 \"",
                        SubcategoryId = laptopsSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/laptop-lenovo.jpg" } },
                    },
                };

                var monitorsSubcategoryId = dbContext.Subcategories.FirstOrDefault(x => x.Name == "Monitors").Id;
                var monitorsToAdd = new List<Product>
                {
                    new Product
                    {
                        Name = "Monitor ASUS VG248QE 24.0 \"",
                        Price = 200.99M,
                        Description = "Monitor ASUS VG248QE 24.0 \"",
                        SubcategoryId = monitorsSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/monitor-asus.jpg" } },
                    },
                    new Product
                    {
                        Name = "Monitor LG 27MK600M-B 0.311 mm, 27.0 \"",
                        Price = 259.99M,
                        Description = "Monitor LG 27MK600M-B 0.311 mm, 27.0 \"",
                        SubcategoryId = monitorsSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/monitor-lg.jpg" } },
                    },
                    new Product
                    {
                        Name = "Monitor ACER KG1 KG271CBMIDPX 0.311 mm, 27.0 \"",
                        Price = 300M,
                        Description = "Monitor ACER KG1 KG271CBMIDPX 0.311 mm, 27.0 \"",
                        SubcategoryId = monitorsSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/monitor-acer.jpg" } },
                    },
                };

                var peripheralsSubcategoryId = dbContext.Subcategories.FirstOrDefault(x => x.Name == "Peripherals").Id;
                var peripheralsToAdd = new List<Product>
                {
                    new Product
                    {
                        Name = "Mouse RAZER Abyssus Essential",
                        Price = 50.99M,
                        Description = "Mouse RAZER Abyssus Essential",
                        SubcategoryId = peripheralsSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/mouse-razor.jpg" } },
                    },
                    new Product
                    {
                        Name = "Keyboard LOGITECH G413 MECHANICAL CARBON",
                        Price = 100M,
                        Description = "Keyboard LOGITECH G413 MECHANICAL CARBON",
                        SubcategoryId = peripheralsSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/keyboard-logitech.jpg" } },
                    },
                    new Product
                    {
                        Name = "Headphones TRACER SECTOR 7.1",
                        Price = 45.99M,
                        Description = "Headphones TRACER SECTOR 7.1",
                        SubcategoryId = peripheralsSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/headphones-tracer.jpg" } },
                    },
                    new Product
                    {
                        Name = "Gaming Chair COUGAR ARMOR S",
                        Price = 250M,
                        Description = "Gaming Chair COUGAR ARMOR S",
                        SubcategoryId = peripheralsSubcategoryId,
                        Images = new List<ProductImage> { new ProductImage { ImageUrl = "/images/products/chair-cougar.jpg" } },
                    },
                };

                var allProducts = new List<Product>(tvsToAdd.Count +
                                    soundbarsToAdd.Count +
                                    homeTheatersToAdd.Count +
                                    bluRaysToAdd.Count +
                                    tvsAccessoriesToAdd.Count +
                                    pcsToAdd.Count +
                                    laptopsToAdd.Count +
                                    monitorsToAdd.Count +
                                    peripheralsToAdd.Count);
                allProducts.AddRange(tvsToAdd);
                allProducts.AddRange(soundbarsToAdd);
                allProducts.AddRange(homeTheatersToAdd);
                allProducts.AddRange(bluRaysToAdd);
                allProducts.AddRange(tvsAccessoriesToAdd);
                allProducts.AddRange(pcsToAdd);
                allProducts.AddRange(laptopsToAdd);
                allProducts.AddRange(monitorsToAdd);
                allProducts.AddRange(peripheralsToAdd);

                await dbContext.Products.AddRangeAsync(allProducts);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
