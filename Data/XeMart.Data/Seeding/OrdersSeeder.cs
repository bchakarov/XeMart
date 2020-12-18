namespace XeMart.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    using XeMart.Common;
    using XeMart.Data.Models;
    using XeMart.Data.Models.Enums;

    public class OrdersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.Orders.Any())
            {
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var adminList = await userManager.GetUsersInRoleAsync(GlobalConstants.AdministratorRoleName);
                var adminUser = adminList.FirstOrDefault();

                var defaultSupplier = dbContext.Suppliers.FirstOrDefault(x => x.IsDefault);

                var rnd = new Random();

                // Seed address to adminUser
                var country = dbContext.Countries.FirstOrDefault(x => x.Name == "Bulgaria");
                var city = new City { Name = "Sofia", ZIPCode = "1000", Country = country };
                var address = new Address { Street = "Test Street", City = city, User = adminUser };
                await dbContext.Cities.AddAsync(city);
                await dbContext.Addresses.AddAsync(address);
                await dbContext.SaveChangesAsync();

                // TV orders
                var tvsOrders = new List<Order>();
                var tvs = dbContext.Products.Where(x => x.Subcategory.Name == "TVS");
                foreach (var tv in tvs)
                {
                    tvsOrders.Add(new Order
                    {
                        User = adminUser,
                        Address = address,
                        PaymentStatus = PaymentStatus.Paid,
                        PaymentType = PaymentType.CashOnDelivery,
                        IsDelivered = true,
                        DeliveredOn = DateTime.UtcNow,
                        DeliveryPrice = defaultSupplier.PriceToHome,
                        DeliveryType = DeliveryType.Home,
                        UserFullName = "AdminUser",
                        Phone = "123456789",
                        Email = "test@example.com",
                        Status = OrderStatus.Delivered,
                        Supplier = defaultSupplier,
                        Products = new List<OrderProduct> { new OrderProduct { Product = tv, Price = tv.Price, Quantity = 1 } },
                    });
                }

                // Seed products to tv orders
                for (int i = 0; i <= 1; i++)
                {
                    foreach (var order in tvsOrders)
                    {
                        var randomValue = rnd.Next(1, 11);

                        if (randomValue <= 1)
                        {
                            // Copurchase home theater with 10% probability
                            var homeTheater = dbContext.Products.Where(x => x.Subcategory.Name == "Home Theaters").OrderBy(x => Guid.NewGuid()).FirstOrDefault();
                            order.Products.Add(new OrderProduct { Product = homeTheater, Price = homeTheater.Price, Quantity = 1 });
                        }
                        else if (randomValue <= 2)
                        {
                            // Copurchase soundbar with 20% probability
                            var soundbar = dbContext.Products.Where(x => x.Subcategory.Name == "Soundbars").OrderBy(x => Guid.NewGuid()).FirstOrDefault();
                            order.Products.Add(new OrderProduct { Product = soundbar, Price = soundbar.Price, Quantity = 1 });
                        }
                        else if (randomValue <= 4)
                        {
                            // Copurchase blu-ray with 40% probability
                            var bluRay = dbContext.Products.Where(x => x.Subcategory.Name == "Blu-Ray & DVD Players").OrderBy(x => Guid.NewGuid()).FirstOrDefault();
                            order.Products.Add(new OrderProduct { Product = bluRay, Price = bluRay.Price, Quantity = 1 });
                        }

                        // Always copurchase tv accessory
                        var accessory = dbContext.Products.Where(x => x.Subcategory.Name == "TV Accessories").OrderBy(x => Guid.NewGuid()).FirstOrDefault();
                        order.Products.Add(new OrderProduct { Product = accessory, Price = accessory.Price, Quantity = 1 });

                        // Calculate total order price
                        order.TotalPrice = order.Products.Sum(x => x.Price * x.Quantity) + order.DeliveryPrice;
                    }
                }

                // PCs orders
                var pcsOrders = new List<Order>();
                var pcs = dbContext.Products.Where(x => x.Subcategory.Name == "PCs");
                foreach (var pc in pcs)
                {
                    pcsOrders.Add(new Order
                    {
                        User = adminUser,
                        Address = address,
                        PaymentStatus = PaymentStatus.Paid,
                        PaymentType = PaymentType.CashOnDelivery,
                        IsDelivered = true,
                        DeliveredOn = DateTime.UtcNow,
                        DeliveryPrice = defaultSupplier.PriceToHome,
                        DeliveryType = DeliveryType.Home,
                        UserFullName = "AdminUser",
                        Phone = "123456789",
                        Email = "test@example.com",
                        Status = OrderStatus.Delivered,
                        Supplier = defaultSupplier,
                        Products = new List<OrderProduct> { new OrderProduct { Product = pc, Price = pc.Price, Quantity = 1 } },
                    });
                }

                // Seed products to pc orders
                for (int i = 0; i <= 1; i++)
                {
                    foreach (var order in pcsOrders)
                    {
                        var randomValue = rnd.Next(1, 11);

                        if (randomValue <= 1)
                        {
                            // Copurchase laptop with 10% probability
                            var laptop = dbContext.Products.Where(x => x.Subcategory.Name == "Laptops").OrderBy(x => Guid.NewGuid()).FirstOrDefault();
                            order.Products.Add(new OrderProduct { Product = laptop, Price = laptop.Price, Quantity = 1 });
                        }
                        else if (randomValue <= 8)
                        {
                            // Copurchase monitor with 80% probability
                            var monitor = dbContext.Products.Where(x => x.Subcategory.Name == "Monitors").OrderBy(x => Guid.NewGuid()).FirstOrDefault();
                            order.Products.Add(new OrderProduct { Product = monitor, Price = monitor.Price, Quantity = 1 });
                        }

                        // Always copurchase peripheral
                        var peripheral = dbContext.Products.Where(x => x.Subcategory.Name == "Peripherals").OrderBy(x => Guid.NewGuid()).FirstOrDefault();
                        order.Products.Add(new OrderProduct { Product = peripheral, Price = peripheral.Price, Quantity = 1 });

                        // Calculate total order price
                        order.TotalPrice = order.Products.Sum(x => x.Price * x.Quantity) + order.DeliveryPrice;
                    }
                }

                await dbContext.Orders.AddRangeAsync(tvsOrders);
                await dbContext.Orders.AddRangeAsync(pcsOrders);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
