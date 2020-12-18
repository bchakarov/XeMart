namespace XeMart.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    using Moq;

    using XeMart.Common;
    using XeMart.Data.Common.Repositories;
    using XeMart.Data.Models;
    using XeMart.Data.Models.Enums;
    using XeMart.Services.Mapping;
    using XeMart.Services.Messaging;
    using XeMart.Web.ViewModels;
    using XeMart.Web.ViewModels.Orders;
    using XeMart.Web.ViewModels.Products;
    using XeMart.Web.ViewModels.ShoppingCart;

    using Xunit;

    [Collection("Sequential")]
    public class OrdersServiceTests
    {
        [Fact]
        public void GetOrdersCountByUserIdShouldReturnCorrectCountUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var orders = new List<Order>
            {
                new Order { UserId = "TestUserId1" },
                new Order { UserId = "TestUserId1" },
                new Order { UserId = "TestUserId2" },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(orders.AsQueryable());

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            Assert.Equal(2, service.GetOrdersCountByUserId("TestUserId1"));

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetOrdersCountByStatusShouldReturnCorrectCountUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var orders = new List<Order>
            {
                new Order { Status = OrderStatus.Cancelled },
                new Order { Status = OrderStatus.Delivered },
                new Order { Status = OrderStatus.Delivered },
                new Order { Status = OrderStatus.Processed },
                new Order { Status = OrderStatus.Processed },
                new Order { Status = OrderStatus.Processed },
                new Order { Status = OrderStatus.Processing },
                new Order { Status = OrderStatus.Unprocessed },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(orders.AsQueryable());

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            Assert.Equal(1, service.GetOrdersCountByStatus(OrderStatus.Cancelled));
            Assert.Equal(2, service.GetOrdersCountByStatus(OrderStatus.Delivered));
            Assert.Equal(3, service.GetOrdersCountByStatus(OrderStatus.Processed));
            Assert.Equal(1, service.GetOrdersCountByStatus(OrderStatus.Processing));
            Assert.Equal(1, service.GetOrdersCountByStatus(OrderStatus.Unprocessed));

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(5));
        }

        [Fact]
        public void GetDeletedOrdersCountShouldReturnCorrectCountUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var orders = new List<Order>
            {
                new Order { IsDeleted = true },
                new Order { IsDeleted = true },
                new Order { IsDeleted = true },
                new Order { IsDeleted = true },
                new Order { IsDeleted = false },
                new Order { IsDeleted = false },
            };

            repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(orders.AsQueryable());

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            Assert.Equal(4, service.GetDeletedOrdersCount());

            repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Once);
        }

        [Fact]
        public void GetPaymentTypeByIdShouldReturnCorrectPaymentTypeUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var orders = new List<Order>
            {
                new Order { Id = "TestOrderId1", PaymentType = PaymentType.CashOnDelivery },
                new Order { Id = "TestOrderId2", PaymentType = PaymentType.Stripe },
                new Order { Id = "TestOrderId3" },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(orders.AsQueryable());

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            Assert.Equal(PaymentType.CashOnDelivery, service.GetPaymentTypeById("TestOrderId1"));
            Assert.Equal(PaymentType.Stripe, service.GetPaymentTypeById("TestOrderId2"));
            Assert.Equal(PaymentType.Unknown, service.GetPaymentTypeById("TestOrderId3"));

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(3));
        }

        [Fact]
        public void UserHasOrderShouldWorkCorrectlyUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var orders = new List<Order>
            {
                new Order { Id = "TestOrderId1", UserId = "TestUserId1" },
                new Order { Id = "TestOrderId1", UserId = "TestUserId2" },
                new Order { Id = "TestOrderId2", UserId = "TestUserId3" },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(orders.AsQueryable());

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            Assert.True(service.UserHasOrder("TestUserId1", "TestOrderId1"));
            Assert.True(service.UserHasOrder("TestUserId2", "TestOrderId1"));
            Assert.False(service.UserHasOrder("TestUserId3", "TestOrderId1"));

            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(3));
        }

        [Fact]
        public void GetProcessingOrderByUserIdShouldWorkCorrectlyUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var products = new List<OrderProduct>
            {
                new OrderProduct { Id = 1 },
                new OrderProduct { Id = 2 },
                new OrderProduct { Id = 3 },
            };

            var orders = new List<Order>
            {
                new Order { Id = "TestOrderId1", UserId = "TestUserId1", Status = OrderStatus.Processing },
                new Order { Id = "TestOrderId2", UserId = "TestUserId2", Status = OrderStatus.Processing, Products = products },
                new Order { Id = "TestOrderId3", UserId = "TestUserId3" },
            };

            repository.Setup(r => r.All()).Returns(orders.AsQueryable());

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            Assert.Equal("TestOrderId2", service.GetProcessingOrderByUserId("TestUserId2").Id);
            Assert.Equal(3, service.GetProcessingOrderByUserId("TestUserId2").Products.Count);

            repository.Verify(x => x.All(), Times.Exactly(2));
        }

        [Fact]
        public void GetByIdGenericShouldWorkCorrectlyWithUndeliveredOrderUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var products = new List<OrderProduct>
            {
                new OrderProduct
                {
                    Price = 120,
                    Quantity = 5,
                    ProductId = "TestProductId",
                    Product = new Product
                    {
                        Name = "TestProductName",
                        Images = new List<ProductImage>
                        {
                            new ProductImage { ImageUrl = "TestImageUrl1" },
                            new ProductImage { ImageUrl = "TestImageUrl2" },
                        },
                    },
                },
            };

            var orders = new List<Order>
            {
                new Order
                {
                    Id = "TestOrderId",
                    UserId = "TestUserId",
                    UserFullName = "TestFullName",
                    Email = "TestEmail",
                    Phone = "TestPhone",
                    Address = new Address { Street = "TestStreet", City = new City { Name = "TestCity", ZIPCode = "TestZIPCode", Country = new Country { Name = "TestCountry" } } },
                    DeliveryPrice = 42,
                    TotalPrice = 84,
                    CreatedOn = new DateTime(2020, 12, 31, 12, 12, 12),
                    PaymentType = PaymentType.Stripe,
                    PaymentStatus = PaymentStatus.Paid,
                    IsDelivered = false,
                    DeliveredOn = null,
                    Status = OrderStatus.Processed,
                    Supplier = new Supplier { Name = "TestSupplierName" },
                    DeliveryType = DeliveryType.Home,
                    Products = products,
                },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(orders.AsQueryable());

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            var orderModel = service.GetById<OrderViewModel>("TestOrderId");
            Assert.Equal("TestOrderId", orderModel.Id);
            Assert.Equal("TestFullName", orderModel.UserFullName);
            Assert.Equal("TestEmail", orderModel.Email);
            Assert.Equal("TestPhone", orderModel.Phone);
            Assert.Equal("TestStreet TestCity, TestZIPCode, TestCountry", orderModel.Address);
            Assert.Equal(42, orderModel.DeliveryPrice);
            Assert.Equal(84, orderModel.TotalPrice);
            Assert.Equal("31-Dec-2020 12:12", orderModel.CreatedOn);
            Assert.Equal(PaymentType.Stripe, orderModel.PaymentType);
            Assert.Equal(PaymentStatus.Paid, orderModel.PaymentStatus);
            Assert.False(orderModel.IsDelivered);
            Assert.Null(orderModel.DeliveredOn);
            Assert.Equal(OrderStatus.Processed, orderModel.Status);
            Assert.Equal("TestSupplierName", orderModel.SupplierName);
            Assert.Equal(DeliveryType.Home, orderModel.DeliveryType);
            Assert.Single(orderModel.Products);
            Assert.Equal("TestProductId", orderModel.Products.FirstOrDefault().ProductId);
            Assert.Equal(120, orderModel.Products.FirstOrDefault().Price);
            Assert.Equal(5, orderModel.Products.FirstOrDefault().Quantity);
            Assert.Equal(600, orderModel.Products.FirstOrDefault().TotalPrice);
            Assert.Equal("TestProductName", orderModel.Products.FirstOrDefault().ProductName);
            Assert.Equal("TestImageUrl1", orderModel.Products.FirstOrDefault().ImageUrl);

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetByIdGenericShouldWorkCorrectlyWithDeliveredOrderUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var products = new List<OrderProduct>
            {
                new OrderProduct
                {
                    Price = 120,
                    Quantity = 5,
                    ProductId = "TestProductId",
                    Product = new Product
                    {
                        Name = "TestProductName",
                        Images = new List<ProductImage>
                        {
                            new ProductImage { ImageUrl = "TestImageUrl1" },
                            new ProductImage { ImageUrl = "TestImageUrl2" },
                        },
                    },
                },
            };

            var orders = new List<Order>
            {
                new Order
                {
                    Id = "TestOrderId",
                    UserId = "TestUserId",
                    UserFullName = "TestFullName",
                    Email = "TestEmail",
                    Phone = "TestPhone",
                    Address = new Address { Street = "TestStreet", City = new City { Name = "TestCity", ZIPCode = "TestZIPCode", Country = new Country { Name = "TestCountry" } } },
                    DeliveryPrice = 42,
                    TotalPrice = 84,
                    CreatedOn = new DateTime(2020, 12, 31, 12, 12, 12),
                    PaymentType = PaymentType.Stripe,
                    PaymentStatus = PaymentStatus.Paid,
                    IsDelivered = true,
                    DeliveredOn = new DateTime(2021, 12, 31, 12, 12, 12),
                    Status = OrderStatus.Processed,
                    Supplier = new Supplier { Name = "TestSupplierName" },
                    DeliveryType = DeliveryType.Home,
                    Products = products,
                },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(orders.AsQueryable());

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            var orderModel = service.GetById<OrderViewModel>("TestOrderId");
            Assert.Equal("TestOrderId", orderModel.Id);
            Assert.Equal("TestFullName", orderModel.UserFullName);
            Assert.Equal("TestEmail", orderModel.Email);
            Assert.Equal("TestPhone", orderModel.Phone);
            Assert.Equal("TestStreet TestCity, TestZIPCode, TestCountry", orderModel.Address);
            Assert.Equal(42, orderModel.DeliveryPrice);
            Assert.Equal(84, orderModel.TotalPrice);
            Assert.Equal("31-Dec-2020 12:12", orderModel.CreatedOn);
            Assert.Equal(PaymentType.Stripe, orderModel.PaymentType);
            Assert.Equal(PaymentStatus.Paid, orderModel.PaymentStatus);
            Assert.True(orderModel.IsDelivered);
            Assert.Equal("31-Dec-2021 12:12", orderModel.DeliveredOn);
            Assert.Equal(OrderStatus.Processed, orderModel.Status);
            Assert.Equal("TestSupplierName", orderModel.SupplierName);
            Assert.Equal(DeliveryType.Home, orderModel.DeliveryType);
            Assert.Single(orderModel.Products);
            Assert.Equal("TestProductId", orderModel.Products.FirstOrDefault().ProductId);
            Assert.Equal(120, orderModel.Products.FirstOrDefault().Price);
            Assert.Equal(5, orderModel.Products.FirstOrDefault().Quantity);
            Assert.Equal(600, orderModel.Products.FirstOrDefault().TotalPrice);
            Assert.Equal("TestProductName", orderModel.Products.FirstOrDefault().ProductName);
            Assert.Equal("TestImageUrl1", orderModel.Products.FirstOrDefault().ImageUrl);

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetByIdGenericShouldWorkCorrectlyWithNoProductImagesUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var products = new List<OrderProduct>
            {
                new OrderProduct
                {
                    Price = 120,
                    Quantity = 5,
                    ProductId = "TestProductId",
                    Product = new Product
                    {
                        Name = "TestProductName",
                        Images = new List<ProductImage>(),
                    },
                },
            };

            var orders = new List<Order>
            {
                new Order
                {
                    Id = "TestOrderId",
                    UserId = "TestUserId",
                    UserFullName = "TestFullName",
                    Email = "TestEmail",
                    Phone = "TestPhone",
                    Address = new Address { Street = "TestStreet", City = new City { Name = "TestCity", ZIPCode = "TestZIPCode", Country = new Country { Name = "TestCountry" } } },
                    DeliveryPrice = 42,
                    TotalPrice = 84,
                    CreatedOn = new DateTime(2020, 12, 31, 12, 12, 12),
                    PaymentType = PaymentType.Stripe,
                    PaymentStatus = PaymentStatus.Paid,
                    IsDelivered = true,
                    DeliveredOn = new DateTime(2021, 12, 31, 12, 12, 12),
                    Status = OrderStatus.Processed,
                    Supplier = new Supplier { Name = "TestSupplierName" },
                    DeliveryType = DeliveryType.Home,
                    Products = products,
                },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(orders.AsQueryable());

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            var orderModel = service.GetById<OrderViewModel>("TestOrderId");
            Assert.Equal("TestOrderId", orderModel.Id);
            Assert.Equal("TestFullName", orderModel.UserFullName);
            Assert.Equal("TestEmail", orderModel.Email);
            Assert.Equal("TestPhone", orderModel.Phone);
            Assert.Equal("TestStreet TestCity, TestZIPCode, TestCountry", orderModel.Address);
            Assert.Equal(42, orderModel.DeliveryPrice);
            Assert.Equal(84, orderModel.TotalPrice);
            Assert.Equal("31-Dec-2020 12:12", orderModel.CreatedOn);
            Assert.Equal(PaymentType.Stripe, orderModel.PaymentType);
            Assert.Equal(PaymentStatus.Paid, orderModel.PaymentStatus);
            Assert.True(orderModel.IsDelivered);
            Assert.Equal("31-Dec-2021 12:12", orderModel.DeliveredOn);
            Assert.Equal(OrderStatus.Processed, orderModel.Status);
            Assert.Equal("TestSupplierName", orderModel.SupplierName);
            Assert.Equal(DeliveryType.Home, orderModel.DeliveryType);
            Assert.Single(orderModel.Products);
            Assert.Equal("TestProductId", orderModel.Products.FirstOrDefault().ProductId);
            Assert.Equal(120, orderModel.Products.FirstOrDefault().Price);
            Assert.Equal(5, orderModel.Products.FirstOrDefault().Quantity);
            Assert.Equal(600, orderModel.Products.FirstOrDefault().TotalPrice);
            Assert.Equal("TestProductName", orderModel.Products.FirstOrDefault().ProductName);
            Assert.Equal(GlobalConstants.ImageNotFoundPath, orderModel.Products.FirstOrDefault().ImageUrl);

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task FulfillOrderByIdShouldWorkCorrectlyUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var orders = new List<Order>
            {
                new Order { Id = "TestOrderId1", UserId = "TestUserId1" },
                new Order { Id = "TestOrderId2", UserId = "TestUserId2", PaymentStatus = PaymentStatus.Unpaid },
            };

            repository.Setup(r => r.All()).Returns(orders.AsQueryable());
            repository.Setup(r => r.Update(It.IsAny<Order>())).Callback((Order item) =>
            {
                var foundOrder = orders.FirstOrDefault(x => x.Id == item.Id);
                foundOrder.PaymentStatus = item.PaymentStatus;
                foundOrder.StripeId = item.StripeId;
            });
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            await service.FulfillOrderById("TestOrderId2", "TestStripeId");
            Assert.Equal(PaymentStatus.Paid, orders.FirstOrDefault(x => x.Id == "TestOrderId2").PaymentStatus);
            Assert.Equal("TestStripeId", orders.FirstOrDefault(x => x.Id == "TestOrderId2").StripeId);

            repository.Verify(x => x.All(), Times.Once);
            repository.Verify(x => x.Update(It.IsAny<Order>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task CancelAnyProcessingOrdersShouldWorkCorrectlyWithNoProcessingOrdersUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var orders = new List<Order>
            {
                new Order { Id = "TestOrderId1", UserId = "TestUserId1", Status = OrderStatus.Cancelled },
                new Order { Id = "TestOrderId2", UserId = "TestUserId2", Status = OrderStatus.Processed },
            };

            repository.Setup(r => r.All()).Returns(orders.AsQueryable());
            repository.Setup(r => r.Update(It.IsAny<Order>())).Callback((Order item) =>
            {
                var foundOrder = orders.FirstOrDefault(x => x.Id == item.Id);
                foundOrder.Status = item.Status;
            });
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            await service.CancelAnyProcessingOrders("TestUserId1");
            Assert.Equal(OrderStatus.Cancelled, orders.FirstOrDefault(x => x.Id == "TestOrderId1").Status);

            repository.Verify(x => x.All(), Times.Once);
            repository.Verify(x => x.Update(It.IsAny<Order>()), Times.Never);
            repository.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task CancelAnyProcessingOrdersShouldWorkCorrectlyUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var orders = new List<Order>
            {
                new Order { Id = "TestOrderId1", UserId = "TestUserId1", Status = OrderStatus.Processing },
                new Order { Id = "TestOrderId2", UserId = "TestUserId2", Status = OrderStatus.Processed },
            };

            repository.Setup(r => r.All()).Returns(orders.AsQueryable());
            repository.Setup(r => r.Update(It.IsAny<Order>())).Callback((Order item) =>
            {
                var foundOrder = orders.FirstOrDefault(x => x.Id == item.Id);
                foundOrder.Status = item.Status;
            });
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            await service.CancelAnyProcessingOrders("TestUserId1");
            Assert.Equal(OrderStatus.Cancelled, orders.FirstOrDefault(x => x.Id == "TestOrderId1").Status);

            repository.Verify(x => x.All(), Times.Once);
            repository.Verify(x => x.Update(It.IsAny<Order>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task DeleteAsyncShouldReturnFalseWithInvalidOrderIdUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var orders = new List<Order>
            {
                new Order { Id = "TestOrderId1", UserId = "TestUserId1" },
                new Order { Id = "TestOrderId2", UserId = "TestUserId2" },
            };

            repository.Setup(r => r.All()).Returns(orders.AsQueryable());
            repository.Setup(r => r.Delete(It.IsAny<Order>())).Callback((Order item) => orders.Remove(item));
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            Assert.False(await service.DeleteAsync("TestOrderId3"));
            Assert.Equal(2, orders.Count);

            repository.Verify(x => x.All(), Times.Once);
            repository.Verify(x => x.Delete(It.IsAny<Order>()), Times.Never);
            repository.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteAsyncShouldWorkCorrectlyUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var orders = new List<Order>
            {
                new Order { Id = "TestOrderId1", UserId = "TestUserId1" },
                new Order { Id = "TestOrderId2", UserId = "TestUserId2" },
            };

            repository.Setup(r => r.All()).Returns(orders.AsQueryable());
            repository.Setup(r => r.Delete(It.IsAny<Order>())).Callback((Order item) => orders.Remove(item));
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            Assert.True(await service.DeleteAsync("TestOrderId2"));
            Assert.Single(orders);

            repository.Verify(x => x.All(), Times.Once);
            repository.Verify(x => x.Delete(It.IsAny<Order>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task UndeleteAsyncShouldReturnFalseWithInvalidOrderIdUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var orders = new List<Order>
            {
                new Order { Id = "TestOrderId1", UserId = "TestUserId1" },
                new Order { Id = "TestOrderId2", UserId = "TestUserId2", IsDeleted = true, DeletedOn = DateTime.UtcNow },
            };

            repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(orders.AsQueryable());
            repository.Setup(r => r.Undelete(It.IsAny<Order>())).Callback((Order item) =>
            {
                var foundOrder = orders.FirstOrDefault(x => x.Id == item.Id);
                foundOrder.IsDeleted = false;
                foundOrder.DeletedOn = null;
            });
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            Assert.False(await service.UndeleteAsync("TestOrderId3"));
            Assert.True(orders.LastOrDefault().IsDeleted);

            repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Once);
            repository.Verify(x => x.Undelete(It.IsAny<Order>()), Times.Never);
            repository.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task UndeleteAsyncShouldWorkCorrectlyUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var orders = new List<Order>
            {
                new Order { Id = "TestOrderId1", UserId = "TestUserId1" },
                new Order { Id = "TestOrderId2", UserId = "TestUserId2", IsDeleted = true, DeletedOn = DateTime.UtcNow },
            };

            repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(orders.AsQueryable());
            repository.Setup(r => r.Undelete(It.IsAny<Order>())).Callback((Order item) =>
            {
                var foundOrder = orders.FirstOrDefault(x => x.Id == item.Id);
                foundOrder.IsDeleted = false;
                foundOrder.DeletedOn = null;
            });
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            Assert.True(await service.UndeleteAsync("TestOrderId2"));
            Assert.False(orders.LastOrDefault().IsDeleted);
            Assert.Null(orders.LastOrDefault().DeletedOn);

            repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Once);
            repository.Verify(x => x.Undelete(It.IsAny<Order>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task CreateAsyncGenericShouldIncreaseCountAndCancelProcessingOrderUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Order>>();
            var suppliersService = new Mock<ISuppliersService>();

            var suppliers = new List<Supplier>
            {
                new Supplier { Id = 1, Name = "TestSupplierName1", PriceToHome = 25, PriceToOffice = 42 },
            };

            var orders = new List<Order>
            {
                new Order { Id = "TestId1", Status = OrderStatus.Processing, UserId = "TestUserId1" },
            };

            repository.Setup(r => r.All()).Returns(orders.AsQueryable());
            repository.Setup(r => r.Update(It.IsAny<Order>())).Callback((Order item) =>
            {
                var foundOrder = orders.FirstOrDefault(x => x.Id == item.Id);
                foundOrder.Status = item.Status;
            });
            repository.Setup(r => r.AddAsync(It.IsAny<Order>())).Callback((Order item) => orders.Add(item));
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            suppliersService.Setup(r => r.GetDeliveryPrice(It.IsAny<int>(), It.IsAny<DeliveryType>())).Returns((int id, DeliveryType type) =>
            {
                var foundSupplier = suppliers.FirstOrDefault(x => x.Id == id);
                if (type == DeliveryType.Home)
                {
                    return foundSupplier.PriceToHome;
                }

                return foundSupplier.PriceToOffice;
            });

            var service = new OrdersService(repository.Object, null, null, suppliersService.Object, null, null, null);
            var model = new CreateOrderInputViewModel { SupplierId = 1, DeliveryType = DeliveryType.Home };
            await service.CreateAsync(model, "TestUserId1");
            Assert.Equal(2, orders.Count);
            Assert.Equal(OrderStatus.Cancelled, orders.FirstOrDefault().Status);

            repository.Verify(x => x.All(), Times.Once);
            repository.Verify(x => x.Update(It.IsAny<Order>()), Times.Once);
            repository.Verify(x => x.AddAsync(It.IsAny<Order>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());

            suppliersService.Verify(x => x.GetDeliveryPrice(It.IsAny<int>(), It.IsAny<DeliveryType>()), Times.Once);
        }

        [Theory]
        [InlineData(DeliveryType.Home, PaymentType.CashOnDelivery)]
        [InlineData(DeliveryType.Home, PaymentType.Stripe)]
        [InlineData(DeliveryType.Office, PaymentType.CashOnDelivery)]
        public async Task CreateAsyncGenericShouldMapCorrectlyAndCancelProcessingOrderUsingMoq(DeliveryType deliveryType, PaymentType paymentType)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Order>>();
            var suppliersService = new Mock<ISuppliersService>();

            var suppliers = new List<Supplier>
            {
                new Supplier { Id = 1, Name = "TestSupplierName1", PriceToHome = 25, PriceToOffice = 42 },
            };

            var orders = new List<Order>
            {
                new Order { Id = "TestId1", Status = OrderStatus.Processing, UserId = "TestUserId1" },
            };

            repository.Setup(r => r.All()).Returns(orders.AsQueryable());
            repository.Setup(r => r.Update(It.IsAny<Order>())).Callback((Order item) =>
            {
                var foundOrder = orders.FirstOrDefault(x => x.Id == item.Id);
                foundOrder.Status = item.Status;
            });
            repository.Setup(r => r.AddAsync(It.IsAny<Order>())).Callback((Order item) => orders.Add(item));
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            suppliersService.Setup(r => r.GetDeliveryPrice(It.IsAny<int>(), It.IsAny<DeliveryType>())).Returns((int id, DeliveryType type) =>
            {
                var foundSupplier = suppliers.FirstOrDefault(x => x.Id == id);
                if (type == DeliveryType.Home)
                {
                    return foundSupplier.PriceToHome;
                }

                return foundSupplier.PriceToOffice;
            });

            var service = new OrdersService(repository.Object, null, null, suppliersService.Object, null, null, null);
            var model = new CreateOrderInputViewModel
            {
                UserFullName = "TestFullName",
                Email = "TestEmail",
                Phone = "TestPhone",
                SupplierId = 1,
                DeliveryType = deliveryType,
                AddressId = "TestAddressId",
                PaymentType = paymentType,
            };
            await service.CreateAsync(model, "TestUserId1");

            Assert.Equal(OrderStatus.Cancelled, orders.FirstOrDefault().Status);
            Assert.Equal("TestFullName", orders.LastOrDefault().UserFullName);
            Assert.Equal("TestEmail", orders.LastOrDefault().Email);
            Assert.Equal("TestPhone", orders.LastOrDefault().Phone);
            Assert.Equal(1, orders.LastOrDefault().SupplierId);
            Assert.Equal(deliveryType, orders.LastOrDefault().DeliveryType);
            Assert.Equal("TestAddressId", orders.LastOrDefault().AddressId);
            Assert.Equal(paymentType, orders.LastOrDefault().PaymentType);

            repository.Verify(x => x.All(), Times.Once);
            repository.Verify(x => x.Update(It.IsAny<Order>()), Times.Once);
            repository.Verify(x => x.AddAsync(It.IsAny<Order>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());

            suppliersService.Verify(x => x.GetDeliveryPrice(It.IsAny<int>(), It.IsAny<DeliveryType>()), Times.Once);
        }

        [Fact]
        public void TakeOrdersByUserIdGenericShouldWorkCorrectlyWithUserIdUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var orders = new List<Order>
            {
                new Order { Id = "TestOrderId1", UserId = "TestUserId1" },
                new Order { Id = "TestOrderId2", UserId = "TestUserId1" },
                new Order { Id = "TestOrderId3", UserId = "TestUserId2" },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(orders.AsQueryable());

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            Assert.Equal(2, service.TakeOrdersByUserId<OrderSummaryViewModel>("TestUserId1", 1, 3).Count());

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Theory]
        [InlineData(1, 2, 2)]
        [InlineData(1, 7, 6)]
        [InlineData(2, 3, 3)]
        [InlineData(6, 1, 1)]
        [InlineData(7, 1, 0)]
        public void TakeOrdersByUserIdGenericShouldWorkCorrectlyWithPagingUsingMoq(int page, int ordersToTake, int count)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var orders = new List<Order>
            {
                new Order { Id = "TestOrderId1", UserId = "TestUserId1" },
                new Order { Id = "TestOrderId2", UserId = "TestUserId1" },
                new Order { Id = "TestOrderId3", UserId = "TestUserId1" },
                new Order { Id = "TestOrderId4", UserId = "TestUserId1" },
                new Order { Id = "TestOrderId5", UserId = "TestUserId1" },
                new Order { Id = "TestOrderId6", UserId = "TestUserId1" },
                new Order { Id = "TestOrderId7", UserId = "TestUserId2" },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(orders.AsQueryable());

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            Assert.Equal(count, service.TakeOrdersByUserId<OrderSummaryViewModel>("TestUserId1", page, ordersToTake).Count());

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void TakeOrdersByUserIdGenericShouldMapCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var orders = new List<Order>
            {
                new Order
                {
                    Id = "TestOrderId1",
                    UserId = "TestUserId1",
                    CreatedOn = new DateTime(2020, 12, 31, 12, 12, 12),
                    PaymentType = PaymentType.CashOnDelivery,
                    PaymentStatus = PaymentStatus.Paid,
                    IsDelivered = true,
                    DeliveredOn = new DateTime(2020, 12, 30, 12, 12, 12),
                    DeletedOn = new DateTime(2021, 12, 31, 12, 12, 12),
                },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(orders.AsQueryable());

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            var order = service.TakeOrdersByUserId<OrderSummaryViewModel>("TestUserId1", 1, 1).FirstOrDefault();
            Assert.Equal("TestOrderId1", order.Id);
            Assert.Equal("31-Dec-2020 12:12", order.CreatedOn);
            Assert.Equal(PaymentType.CashOnDelivery, order.PaymentType);
            Assert.Equal(PaymentStatus.Paid, order.PaymentStatus);
            Assert.True(order.IsDelivered);
            Assert.Equal("30-Dec-2020 12:12", order.DeliveredOn);
            Assert.Equal("31-Dec-2021 12:12", order.DeletedOn);

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void TakeOrdersByUserIdGenericShouldMapCorrectlyWithNotDeliveredAndNotDeletedUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var orders = new List<Order>
            {
                new Order
                {
                    Id = "TestOrderId1",
                    UserId = "TestUserId1",
                    CreatedOn = new DateTime(2020, 12, 31, 12, 12, 12),
                    PaymentType = PaymentType.CashOnDelivery,
                    PaymentStatus = PaymentStatus.Paid,
                    IsDelivered = false,
                    DeliveredOn = null,
                    DeletedOn = null,
                },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(orders.AsQueryable());

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            var order = service.TakeOrdersByUserId<OrderSummaryViewModel>("TestUserId1", 1, 1).FirstOrDefault();
            Assert.Equal("TestOrderId1", order.Id);
            Assert.Equal("31-Dec-2020 12:12", order.CreatedOn);
            Assert.Equal(PaymentType.CashOnDelivery, order.PaymentType);
            Assert.Equal(PaymentStatus.Paid, order.PaymentStatus);
            Assert.False(order.IsDelivered);
            Assert.Null(order.DeliveredOn);
            Assert.Null(order.DeletedOn);

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void TakeOrdersByStatusGenericShouldWorkCorrectlyWithUserIdUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var orders = new List<Order>
            {
                new Order { Id = "TestOrderId1", Status = OrderStatus.Unprocessed },
                new Order { Id = "TestOrderId2", Status = OrderStatus.Unprocessed },
                new Order { Id = "TestOrderId3", Status = OrderStatus.Processed },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(orders.AsQueryable());

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            Assert.Equal(2, service.TakeOrdersByStatus<OrderSummaryViewModel>(OrderStatus.Unprocessed, 1, 3).Count());

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Theory]
        [InlineData(1, 2, 2)]
        [InlineData(1, 7, 6)]
        [InlineData(2, 3, 3)]
        [InlineData(6, 1, 1)]
        [InlineData(7, 1, 0)]
        public void TakeOrdersByStatusGenericShouldWorkCorrectlyWithPagingUsingMoq(int page, int ordersToTake, int count)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var orders = new List<Order>
            {
                new Order { Id = "TestOrderId1", Status = OrderStatus.Unprocessed },
                new Order { Id = "TestOrderId2", Status = OrderStatus.Unprocessed },
                new Order { Id = "TestOrderId3", Status = OrderStatus.Unprocessed },
                new Order { Id = "TestOrderId4", Status = OrderStatus.Unprocessed },
                new Order { Id = "TestOrderId5", Status = OrderStatus.Unprocessed },
                new Order { Id = "TestOrderId6", Status = OrderStatus.Unprocessed },
                new Order { Id = "TestOrderId7", Status = OrderStatus.Processed },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(orders.AsQueryable());

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            Assert.Equal(count, service.TakeOrdersByStatus<OrderSummaryViewModel>(OrderStatus.Unprocessed, page, ordersToTake).Count());

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void TakeOrdersByStatusGenericShouldMapCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var orders = new List<Order>
            {
                new Order
                {
                    Id = "TestOrderId1",
                    UserId = "TestUserId1",
                    CreatedOn = new DateTime(2020, 12, 31, 12, 12, 12),
                    PaymentType = PaymentType.CashOnDelivery,
                    PaymentStatus = PaymentStatus.Paid,
                    IsDelivered = true,
                    DeliveredOn = new DateTime(2020, 12, 30, 12, 12, 12),
                    DeletedOn = new DateTime(2021, 12, 31, 12, 12, 12),
                    Status = OrderStatus.Unprocessed,
                },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(orders.AsQueryable());

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            var order = service.TakeOrdersByStatus<OrderSummaryViewModel>(OrderStatus.Unprocessed, 1, 1).FirstOrDefault();
            Assert.Equal("TestOrderId1", order.Id);
            Assert.Equal("31-Dec-2020 12:12", order.CreatedOn);
            Assert.Equal(PaymentType.CashOnDelivery, order.PaymentType);
            Assert.Equal(PaymentStatus.Paid, order.PaymentStatus);
            Assert.True(order.IsDelivered);
            Assert.Equal("30-Dec-2020 12:12", order.DeliveredOn);
            Assert.Equal("31-Dec-2021 12:12", order.DeletedOn);

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void TakeOrdersByStatusGenericShouldMapCorrectlyWithNotDeliveredAndNotDeletedUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var orders = new List<Order>
            {
                new Order
                {
                    Id = "TestOrderId1",
                    UserId = "TestUserId1",
                    CreatedOn = new DateTime(2020, 12, 31, 12, 12, 12),
                    PaymentType = PaymentType.CashOnDelivery,
                    PaymentStatus = PaymentStatus.Paid,
                    IsDelivered = false,
                    DeliveredOn = null,
                    DeletedOn = null,
                    Status = OrderStatus.Unprocessed,
                },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(orders.AsQueryable());

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            var order = service.TakeOrdersByStatus<OrderSummaryViewModel>(OrderStatus.Unprocessed, 1, 1).FirstOrDefault();
            Assert.Equal("TestOrderId1", order.Id);
            Assert.Equal("31-Dec-2020 12:12", order.CreatedOn);
            Assert.Equal(PaymentType.CashOnDelivery, order.PaymentType);
            Assert.Equal(PaymentStatus.Paid, order.PaymentStatus);
            Assert.False(order.IsDelivered);
            Assert.Null(order.DeliveredOn);
            Assert.Null(order.DeletedOn);

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void TakeProcessingAndUnprocessedOrdersGenericShouldWorkCorrectlyWithUserIdUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var orders = new List<Order>
            {
                new Order { Id = "TestOrderId1", Status = OrderStatus.Processing },
                new Order { Id = "TestOrderId2", Status = OrderStatus.Unprocessed },
                new Order { Id = "TestOrderId3", Status = OrderStatus.Processed },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(orders.AsQueryable());

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            Assert.Equal(2, service.TakeProcessingAndUnprocessedOrders<OrderSummaryViewModel>(1, 3).Count());

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Theory]
        [InlineData(1, 2, 2)]
        [InlineData(1, 7, 6)]
        [InlineData(2, 3, 3)]
        [InlineData(6, 1, 1)]
        [InlineData(7, 1, 0)]
        public void TakeProcessingAndUnprocessedOrdersGenericShouldWorkCorrectlyWithPagingUsingMoq(int page, int ordersToTake, int count)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var orders = new List<Order>
            {
                new Order { Id = "TestOrderId1", Status = OrderStatus.Unprocessed },
                new Order { Id = "TestOrderId2", Status = OrderStatus.Processing },
                new Order { Id = "TestOrderId3", Status = OrderStatus.Unprocessed },
                new Order { Id = "TestOrderId4", Status = OrderStatus.Processing },
                new Order { Id = "TestOrderId5", Status = OrderStatus.Unprocessed },
                new Order { Id = "TestOrderId6", Status = OrderStatus.Processing },
                new Order { Id = "TestOrderId7", Status = OrderStatus.Processed },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(orders.AsQueryable());

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            Assert.Equal(count, service.TakeProcessingAndUnprocessedOrders<OrderSummaryViewModel>(page, ordersToTake).Count());

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void TakeProcessingAndUnprocessedOrdersGenericShouldMapCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var orders = new List<Order>
            {
                new Order
                {
                    Id = "TestOrderId1",
                    UserId = "TestUserId1",
                    CreatedOn = new DateTime(2020, 12, 31, 12, 12, 12),
                    PaymentType = PaymentType.CashOnDelivery,
                    PaymentStatus = PaymentStatus.Paid,
                    IsDelivered = true,
                    DeliveredOn = new DateTime(2020, 12, 30, 12, 12, 12),
                    DeletedOn = new DateTime(2021, 12, 31, 12, 12, 12),
                    Status = OrderStatus.Processing,
                },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(orders.AsQueryable());

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            var order = service.TakeProcessingAndUnprocessedOrders<OrderSummaryViewModel>(1, 1).FirstOrDefault();
            Assert.Equal("TestOrderId1", order.Id);
            Assert.Equal("31-Dec-2020 12:12", order.CreatedOn);
            Assert.Equal(PaymentType.CashOnDelivery, order.PaymentType);
            Assert.Equal(PaymentStatus.Paid, order.PaymentStatus);
            Assert.True(order.IsDelivered);
            Assert.Equal("30-Dec-2020 12:12", order.DeliveredOn);
            Assert.Equal("31-Dec-2021 12:12", order.DeletedOn);

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void TakeProcessingAndUnprocessedOrdersGenericShouldMapCorrectlyWithNotDeliveredAndNotDeletedUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var orders = new List<Order>
            {
                new Order
                {
                    Id = "TestOrderId1",
                    UserId = "TestUserId1",
                    CreatedOn = new DateTime(2020, 12, 31, 12, 12, 12),
                    PaymentType = PaymentType.CashOnDelivery,
                    PaymentStatus = PaymentStatus.Paid,
                    IsDelivered = false,
                    DeliveredOn = null,
                    DeletedOn = null,
                    Status = OrderStatus.Unprocessed,
                },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(orders.AsQueryable());

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            var order = service.TakeProcessingAndUnprocessedOrders<OrderSummaryViewModel>(1, 1).FirstOrDefault();
            Assert.Equal("TestOrderId1", order.Id);
            Assert.Equal("31-Dec-2020 12:12", order.CreatedOn);
            Assert.Equal(PaymentType.CashOnDelivery, order.PaymentType);
            Assert.Equal(PaymentStatus.Paid, order.PaymentStatus);
            Assert.False(order.IsDelivered);
            Assert.Null(order.DeliveredOn);
            Assert.Null(order.DeletedOn);

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void TakeDeletedOrdersGenericShouldWorkCorrectlyWithUserIdUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var orders = new List<Order>
            {
                new Order { Id = "TestOrderId1", IsDeleted = true },
                new Order { Id = "TestOrderId2", IsDeleted = true },
                new Order { Id = "TestOrderId3", IsDeleted = false },
            };

            repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(orders.AsQueryable());

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            Assert.Equal(2, service.TakeDeletedOrders<OrderSummaryViewModel>(1, 3).Count());

            repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Once);
        }

        [Theory]
        [InlineData(1, 2, 2)]
        [InlineData(1, 7, 6)]
        [InlineData(2, 3, 3)]
        [InlineData(6, 1, 1)]
        [InlineData(7, 1, 0)]
        public void TakeDeletedOrdersGenericShouldWorkCorrectlyWithPagingUsingMoq(int page, int ordersToTake, int count)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var orders = new List<Order>
            {
                new Order { Id = "TestOrderId1", IsDeleted = true },
                new Order { Id = "TestOrderId2", IsDeleted = true },
                new Order { Id = "TestOrderId3", IsDeleted = true },
                new Order { Id = "TestOrderId4", IsDeleted = true },
                new Order { Id = "TestOrderId5", IsDeleted = true },
                new Order { Id = "TestOrderId6", IsDeleted = true },
                new Order { Id = "TestOrderId7", IsDeleted = false },
            };

            repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(orders.AsQueryable());

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            Assert.Equal(count, service.TakeDeletedOrders<OrderSummaryViewModel>(page, ordersToTake).Count());

            repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Once);
        }

        [Fact]
        public void TakeDeletedOrdersGenericShouldMapCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var orders = new List<Order>
            {
                new Order
                {
                    Id = "TestOrderId1",
                    UserId = "TestUserId1",
                    CreatedOn = new DateTime(2020, 12, 31, 12, 12, 12),
                    PaymentType = PaymentType.CashOnDelivery,
                    PaymentStatus = PaymentStatus.Paid,
                    IsDelivered = true,
                    DeliveredOn = new DateTime(2020, 12, 30, 12, 12, 12),
                    IsDeleted = true,
                    DeletedOn = new DateTime(2021, 12, 31, 12, 12, 12),
                    Status = OrderStatus.Processing,
                },
            };

            repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(orders.AsQueryable());

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            var order = service.TakeDeletedOrders<OrderSummaryViewModel>(1, 1).FirstOrDefault();
            Assert.Equal("TestOrderId1", order.Id);
            Assert.Equal("31-Dec-2020 12:12", order.CreatedOn);
            Assert.Equal(PaymentType.CashOnDelivery, order.PaymentType);
            Assert.Equal(PaymentStatus.Paid, order.PaymentStatus);
            Assert.True(order.IsDelivered);
            Assert.Equal("30-Dec-2020 12:12", order.DeliveredOn);
            Assert.Equal("31-Dec-2021 12:12", order.DeletedOn);

            repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Once);
        }

        [Fact]
        public void TakeProcessingAndUnprocessedOrdersGenericShouldMapCorrectlyWithNotDeliveredUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var orders = new List<Order>
            {
                new Order
                {
                    Id = "TestOrderId1",
                    UserId = "TestUserId1",
                    CreatedOn = new DateTime(2020, 12, 31, 12, 12, 12),
                    PaymentType = PaymentType.CashOnDelivery,
                    PaymentStatus = PaymentStatus.Paid,
                    IsDelivered = false,
                    DeliveredOn = null,
                    Status = OrderStatus.Unprocessed,
                    IsDeleted = true,
                    DeletedOn = new DateTime(2021, 12, 31, 12, 12, 12),
                },
            };

            repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(orders.AsQueryable());

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            var order = service.TakeDeletedOrders<OrderSummaryViewModel>(1, 1).FirstOrDefault();
            Assert.Equal("TestOrderId1", order.Id);
            Assert.Equal("31-Dec-2020 12:12", order.CreatedOn);
            Assert.Equal(PaymentType.CashOnDelivery, order.PaymentType);
            Assert.Equal(PaymentStatus.Paid, order.PaymentStatus);
            Assert.False(order.IsDelivered);
            Assert.Null(order.DeliveredOn);
            Assert.Equal("31-Dec-2021 12:12", order.DeletedOn);

            repository.Verify(x => x.AllAsNoTrackingWithDeleted(), Times.Once);
        }

        [Fact]
        public void GetMostBoughtProductsGenericShouldReturnCorrectCountUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IRepository<OrderProduct>>();
            var productService = new Mock<IProductsService>();

            var products = new List<Product>
            {
                new Product { Id = "TestProductID1" },
                new Product { Id = "TestProductID1" },
                new Product { Id = "TestProductID1" },
            };

            var orderProducts = new List<OrderProduct>
            {
                new OrderProduct { Id = 1, OrderId = "TestOrderId1", ProductId = "TestProductId1", Order = new Order { Status = OrderStatus.Delivered } },
                new OrderProduct { Id = 2, OrderId = "TestOrderId2", ProductId = "TestProductId1", Order = new Order { Status = OrderStatus.Delivered } },
                new OrderProduct { Id = 3, OrderId = "TestOrderId3", ProductId = "TestProductId1", Order = new Order { Status = OrderStatus.Delivered } },
                new OrderProduct { Id = 4, OrderId = "TestOrderId4", ProductId = "TestProductId2", Order = new Order { Status = OrderStatus.Delivered } },
                new OrderProduct { Id = 5, OrderId = "TestOrderId5", ProductId = "TestProductId2", Order = new Order { Status = OrderStatus.Delivered } },
                new OrderProduct { Id = 6, OrderId = "TestOrderId6", ProductId = "TestProductId3", Order = new Order { Status = OrderStatus.Delivered } },
                new OrderProduct { Id = 7, OrderId = "TestOrderId7", ProductId = "TestProductId3", Order = new Order { Status = OrderStatus.Delivered } },
                new OrderProduct { Id = 8, OrderId = "TestOrderId8", ProductId = "TestProductId3", Order = new Order { Status = OrderStatus.Delivered } },
                new OrderProduct { Id = 9, OrderId = "TestOrderId9", ProductId = "TestProductId3", Order = new Order { Status = OrderStatus.Delivered } },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(orderProducts.AsQueryable());
            productService.Setup(r => r.GetById<ProductSidebarViewModel>(It.IsAny<string>())).Returns((string id) =>
            {
                return products.AsQueryable().Where(x => x.Id == id).To<ProductSidebarViewModel>().FirstOrDefault();
            });

            var service = new OrdersService(null, repository.Object, productService.Object, null, null, null, null);
            Assert.Equal(2, service.GetMostBoughtProducts<ProductSidebarViewModel>(2).Count());

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
            productService.Verify(x => x.GetById<ProductSidebarViewModel>(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public void GetMostBoughtProductsGenericShouldMapCorrectlyWithNoImagesAndNoReviewsUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IRepository<OrderProduct>>();
            var productService = new Mock<IProductsService>();

            var products = new List<Product>
            {
                new Product { Id = "TestProductId1" },
                new Product { Id = "TestProductId2" },
                new Product { Id = "TestProductId3", Name = "TestProductName3", Price = 423 },
            };

            var orderProducts = new List<OrderProduct>
            {
                new OrderProduct { Id = 1, OrderId = "TestOrderId1", ProductId = "TestProductId1", Order = new Order { Status = OrderStatus.Delivered } },
                new OrderProduct { Id = 2, OrderId = "TestOrderId2", ProductId = "TestProductId1", Order = new Order { Status = OrderStatus.Delivered } },
                new OrderProduct { Id = 3, OrderId = "TestOrderId3", ProductId = "TestProductId1", Order = new Order { Status = OrderStatus.Delivered } },
                new OrderProduct { Id = 4, OrderId = "TestOrderId4", ProductId = "TestProductId2", Order = new Order { Status = OrderStatus.Delivered } },
                new OrderProduct { Id = 5, OrderId = "TestOrderId5", ProductId = "TestProductId2", Order = new Order { Status = OrderStatus.Delivered } },
                new OrderProduct { Id = 6, OrderId = "TestOrderId6", ProductId = "TestProductId3", Order = new Order { Status = OrderStatus.Delivered } },
                new OrderProduct { Id = 7, OrderId = "TestOrderId7", ProductId = "TestProductId3", Order = new Order { Status = OrderStatus.Delivered } },
                new OrderProduct { Id = 8, OrderId = "TestOrderId8", ProductId = "TestProductId3", Order = new Order { Status = OrderStatus.Delivered } },
                new OrderProduct { Id = 9, OrderId = "TestOrderId9", ProductId = "TestProductId3", Order = new Order { Status = OrderStatus.Delivered } },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(orderProducts.AsQueryable());
            productService.Setup(r => r.GetById<ProductSidebarViewModel>(It.IsAny<string>())).Returns((string id) =>
            {
                return products.AsQueryable().Where(x => x.Id == id).To<ProductSidebarViewModel>().FirstOrDefault();
            });

            var service = new OrdersService(null, repository.Object, productService.Object, null, null, null, null);
            var productsResult = service.GetMostBoughtProducts<ProductSidebarViewModel>(2);
            Assert.Equal("TestProductId3", productsResult.FirstOrDefault().Id);
            Assert.Equal("TestProductName3", productsResult.FirstOrDefault().Name);
            Assert.Equal(423, productsResult.FirstOrDefault().Price);
            Assert.Equal(GlobalConstants.ImageNotFoundPath, productsResult.FirstOrDefault().ImageUrl);
            Assert.Equal(0, productsResult.FirstOrDefault().AverageRating);

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
            productService.Verify(x => x.GetById<ProductSidebarViewModel>(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public void GetMostBoughtProductsGenericShouldMapCorrectlyUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IRepository<OrderProduct>>();
            var productService = new Mock<IProductsService>();

            var products = new List<Product>
            {
                new Product { Id = "TestProductId1" },
                new Product { Id = "TestProductId2" },
                new Product
                {
                    Id = "TestProductId3",
                    Name = "TestProductName3",
                    Price = 423,
                    Images = new List<ProductImage>
                    {
                        new ProductImage { ImageUrl = "TestUrl1" },
                        new ProductImage { ImageUrl = "TestUrl2" },
                    },
                    Reviews = new List<UserProductReview>
                    {
                        new UserProductReview { Rating = 5 },
                        new UserProductReview { Rating = 2 },
                        new UserProductReview { Rating = 1 },
                    },
                },
            };

            var orderProducts = new List<OrderProduct>
            {
                new OrderProduct { Id = 1, OrderId = "TestOrderId1", ProductId = "TestProductId1", Order = new Order { Status = OrderStatus.Delivered } },
                new OrderProduct { Id = 2, OrderId = "TestOrderId2", ProductId = "TestProductId1", Order = new Order { Status = OrderStatus.Delivered } },
                new OrderProduct { Id = 3, OrderId = "TestOrderId3", ProductId = "TestProductId1", Order = new Order { Status = OrderStatus.Delivered } },
                new OrderProduct { Id = 4, OrderId = "TestOrderId4", ProductId = "TestProductId2", Order = new Order { Status = OrderStatus.Delivered } },
                new OrderProduct { Id = 5, OrderId = "TestOrderId5", ProductId = "TestProductId2", Order = new Order { Status = OrderStatus.Delivered } },
                new OrderProduct { Id = 6, OrderId = "TestOrderId6", ProductId = "TestProductId3", Order = new Order { Status = OrderStatus.Delivered } },
                new OrderProduct { Id = 7, OrderId = "TestOrderId7", ProductId = "TestProductId3", Order = new Order { Status = OrderStatus.Delivered } },
                new OrderProduct { Id = 8, OrderId = "TestOrderId8", ProductId = "TestProductId3", Order = new Order { Status = OrderStatus.Delivered } },
                new OrderProduct { Id = 9, OrderId = "TestOrderId9", ProductId = "TestProductId3", Order = new Order { Status = OrderStatus.Delivered } },
            };

            repository.Setup(r => r.AllAsNoTracking()).Returns(orderProducts.AsQueryable());
            productService.Setup(r => r.GetById<ProductSidebarViewModel>(It.IsAny<string>())).Returns((string id) =>
            {
                return products.AsQueryable().Where(x => x.Id == id).To<ProductSidebarViewModel>().FirstOrDefault();
            });

            var service = new OrdersService(null, repository.Object, productService.Object, null, null, null, null);
            var productsResult = service.GetMostBoughtProducts<ProductSidebarViewModel>(2);
            Assert.Equal("TestProductId3", productsResult.FirstOrDefault().Id);
            Assert.Equal("TestProductName3", productsResult.FirstOrDefault().Name);
            Assert.Equal(423, productsResult.FirstOrDefault().Price);
            Assert.Equal("TestUrl1", productsResult.FirstOrDefault().ImageUrl);
            Assert.Equal(2.67, productsResult.FirstOrDefault().AverageRating);

            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
            productService.Verify(x => x.GetById<ProductSidebarViewModel>(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task SetOrderStatusAsyncShouldReturnFalseWithInvalidOrderIdUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var orders = new List<Order>
            {
                new Order { Id = "TestOrderId1" },
                new Order { Id = "TestOrderId2" },
                new Order { Id = "TestOrderId3" },
            };

            repository.Setup(r => r.All()).Returns(orders.AsQueryable());

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            Assert.False(await service.SetOrderStatusAsync("invalid order id", "Delivered"));

            repository.Verify(x => x.All(), Times.Once);
        }

        [Fact]
        public async Task SetOrderStatusAsyncShouldReturnFalseWithInvalidOrderStatusUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var orders = new List<Order>
            {
                new Order { Id = "TestOrderId1" },
                new Order { Id = "TestOrderId2" },
                new Order { Id = "TestOrderId3" },
            };

            repository.Setup(r => r.All()).Returns(orders.AsQueryable());

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            Assert.False(await service.SetOrderStatusAsync("TestOrderId3", "invalid status"));

            repository.Verify(x => x.All(), Times.Once);
        }

        [Fact]
        public async Task SetOrderStatusAsyncShouldShouldWorkCorrectlyWithDeliveredStatusUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var orders = new List<Order>
            {
                new Order { Id = "TestOrderId1" },
                new Order { Id = "TestOrderId2" },
                new Order { Id = "TestOrderId3", Status = OrderStatus.Processed },
            };

            repository.Setup(r => r.All()).Returns(orders.AsQueryable());
            repository.Setup(r => r.Update(It.IsAny<Order>())).Callback((Order item) =>
            {
                var foundOrder = orders.FirstOrDefault(x => x.Id == item.Id);
                foundOrder.Status = item.Status;
                foundOrder.IsDelivered = item.IsDelivered;
                foundOrder.DeliveredOn = item.DeliveredOn;
                foundOrder.PaymentStatus = item.PaymentStatus;
            });
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            Assert.True(await service.SetOrderStatusAsync("TestOrderId3", "Delivered"));
            Assert.Equal(OrderStatus.Delivered, orders.FirstOrDefault(x => x.Id == "TestOrderId3").Status);
            Assert.True(orders.FirstOrDefault(x => x.Id == "TestOrderId3").IsDelivered);
            Assert.NotNull(orders.FirstOrDefault(x => x.Id == "TestOrderId3").DeliveredOn);
            Assert.Equal(PaymentStatus.Paid, orders.FirstOrDefault(x => x.Id == "TestOrderId3").PaymentStatus);

            repository.Verify(x => x.All(), Times.Once);
            repository.Verify(x => x.Update(It.IsAny<Order>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task SetOrderStatusAsyncShouldShouldWorkCorrectlyWithNotDeliveredStatusUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var orders = new List<Order>
            {
                new Order { Id = "TestOrderId1" },
                new Order { Id = "TestOrderId2" },
                new Order { Id = "TestOrderId3", Status = OrderStatus.Unprocessed },
            };

            repository.Setup(r => r.All()).Returns(orders.AsQueryable());
            repository.Setup(r => r.Update(It.IsAny<Order>())).Callback((Order item) =>
            {
                var foundOrder = orders.FirstOrDefault(x => x.Id == item.Id);
                foundOrder.Status = item.Status;
                foundOrder.IsDelivered = item.IsDelivered;
                foundOrder.DeliveredOn = item.DeliveredOn;
            });
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            Assert.True(await service.SetOrderStatusAsync("TestOrderId3", "Processed"));
            Assert.Equal(OrderStatus.Processed, orders.FirstOrDefault(x => x.Id == "TestOrderId3").Status);
            Assert.False(orders.FirstOrDefault(x => x.Id == "TestOrderId3").IsDelivered);
            Assert.Null(orders.FirstOrDefault(x => x.Id == "TestOrderId3").DeliveredOn);

            repository.Verify(x => x.All(), Times.Once);
            repository.Verify(x => x.Update(It.IsAny<Order>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task CompleteOrderAsyncShouldReturnNullWithInvalidProcessingOrderUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<Order>>();

            var orders = new List<Order>
            {
                new Order { Id = "TestOrderId1", UserId = "TestUserId1", Status = OrderStatus.Unprocessed },
                new Order { Id = "TestOrderId2", UserId = "TestUserId1", Status = OrderStatus.Unprocessed },
                new Order { Id = "TestOrderId3", UserId = "TestUserId1", Status = OrderStatus.Unprocessed },
            };

            repository.Setup(r => r.All()).Returns(orders.AsQueryable());

            var service = new OrdersService(repository.Object, null, null, null, null, null, null);
            Assert.Null(await service.CompleteOrderAsync("TestUserId1"));

            repository.Verify(x => x.All(), Times.Once);
        }

        [Fact]
        public async Task CompleteOrderAsyncShouldReturnNullWithNullShoppingCartUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<Order>>();
            var shoppingCartService = new Mock<IShoppingCartService>();

            var orders = new List<Order>
            {
                new Order { Id = "TestOrderId1", UserId = "TestUserId1", Status = OrderStatus.Processing },
                new Order { Id = "TestOrderId2", UserId = "TestUserId1", Status = OrderStatus.Unprocessed },
                new Order { Id = "TestOrderId3", UserId = "TestUserId1", Status = OrderStatus.Unprocessed },
            };

            repository.Setup(r => r.All()).Returns(orders.AsQueryable());
            shoppingCartService.Setup(r => r.GetAllProductsAsync<ShoppingCartProductViewModel>(It.IsAny<bool>(), It.IsAny<ISession>(), It.IsAny<string>())).Returns((bool isAuthenticated, ISession session, string userId) =>
            {
                return Task.FromResult<IEnumerable<ShoppingCartProductViewModel>>(null);
            });

            var service = new OrdersService(repository.Object, null, null, null, shoppingCartService.Object, null, null);
            Assert.Null(await service.CompleteOrderAsync("TestUserId1"));

            repository.Verify(x => x.All(), Times.Once);
            shoppingCartService.Verify(x => x.GetAllProductsAsync<ShoppingCartProductViewModel>(It.IsAny<bool>(), It.IsAny<ISession>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task CompleteOrderAsyncShouldReturnNullWithEmptyShoppingCartUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<Order>>();
            var shoppingCartService = new Mock<IShoppingCartService>();

            var orders = new List<Order>
            {
                new Order { Id = "TestOrderId1", UserId = "TestUserId1", Status = OrderStatus.Processing },
                new Order { Id = "TestOrderId2", UserId = "TestUserId1", Status = OrderStatus.Unprocessed },
                new Order { Id = "TestOrderId3", UserId = "TestUserId1", Status = OrderStatus.Unprocessed },
            };

            repository.Setup(r => r.All()).Returns(orders.AsQueryable());
            shoppingCartService.Setup(r => r.GetAllProductsAsync<ShoppingCartProductViewModel>(It.IsAny<bool>(), It.IsAny<ISession>(), It.IsAny<string>())).Returns((bool isAuthenticated, ISession session, string userId) =>
            {
                return Task.FromResult<IEnumerable<ShoppingCartProductViewModel>>(new List<ShoppingCartProductViewModel>());
            });

            var service = new OrdersService(repository.Object, null, null, null, shoppingCartService.Object, null, null);
            Assert.Null(await service.CompleteOrderAsync("TestUserId1"));

            repository.Verify(x => x.All(), Times.Once);
            shoppingCartService.Verify(x => x.GetAllProductsAsync<ShoppingCartProductViewModel>(It.IsAny<bool>(), It.IsAny<ISession>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task CompleteOrderAsyncShouldWorkCorrectlyWithUnpaidOrderAndStripeOrderUsingMoq()
        {
            var repository = new Mock<IDeletableEntityRepository<Order>>();
            var shoppingCartService = new Mock<IShoppingCartService>();

            var orders = new List<Order>
            {
                new Order
                {
                    Id = "TestOrderId1",
                    UserId = "TestUserId1",
                    Status = OrderStatus.Processing,
                    PaymentType = PaymentType.Stripe,
                    Products = new List<OrderProduct>
                    {
                        new OrderProduct { ProductId = "TestProductId1" },
                    },
                },
            };

            repository.Setup(r => r.All()).Returns(orders.AsQueryable());
            repository.Setup(r => r.AllAsNoTracking()).Returns(orders.AsQueryable());
            repository.Setup(r => r.Update(It.IsAny<Order>())).Callback((Order item) =>
            {
                var foundOrder = orders.FirstOrDefault(x => x.Id == item.Id);
                foundOrder.TotalPrice = item.TotalPrice;
            });
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            shoppingCartService.Setup(r => r.GetAllProductsAsync<ShoppingCartProductViewModel>(It.IsAny<bool>(), It.IsAny<ISession>(), It.IsAny<string>())).Returns((bool isAuthenticated, ISession session, string userId) =>
            {
                return Task.FromResult<IEnumerable<ShoppingCartProductViewModel>>(new List<ShoppingCartProductViewModel>
                {
                    new ShoppingCartProductViewModel { ProductId = "TestProductId1" },
                    new ShoppingCartProductViewModel { ProductId = "TestProductId2", ProductPrice = 42, Quantity = 5 },
                    new ShoppingCartProductViewModel { ProductId = "TestProductId3", ProductPrice = 25, Quantity = 4 },
                });
            });

            var service = new OrdersService(repository.Object, null, null, null, shoppingCartService.Object, null, null);
            Assert.Equal("TestOrderId1", await service.CompleteOrderAsync("TestUserId1"));
            Assert.Equal(310, orders.FirstOrDefault(x => x.Id == "TestOrderId1").TotalPrice);
            Assert.Equal(3, orders.FirstOrDefault(x => x.Id == "TestOrderId1").Products.Count);

            repository.Verify(x => x.All(), Times.Once);
            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(3));
            repository.Verify(x => x.Update(It.IsAny<Order>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());

            shoppingCartService.Verify(x => x.GetAllProductsAsync<ShoppingCartProductViewModel>(It.IsAny<bool>(), It.IsAny<ISession>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task CompleteOrderAsyncShouldWorkCorrectlyWithCashOnDeliveryPaymentTypeUsingMoq()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var repository = new Mock<IDeletableEntityRepository<Order>>();
            var shoppingCartService = new Mock<IShoppingCartService>();
            var viewRenderService = new Mock<IViewRenderService>();
            var emailSender = new Mock<IEmailSender>();

            var orders = new List<Order>
            {
                new Order
                {
                    Id = "TestOrderId1",
                    UserId = "TestUserId1",
                    Status = OrderStatus.Processing,
                    PaymentType = PaymentType.CashOnDelivery,
                    Products = new List<OrderProduct>
                    {
                        new OrderProduct { ProductId = "TestProductId1" },
                    },
                    IsDeleted = false,
                    CreatedOn = DateTime.UtcNow,
                    Address = new Address { Street = "TestStreet", City = new City { Name = "TestCity", ZIPCode = "TestZIPCode", Country = new Country { Name = "TestCountry" } }, },
                    Supplier = new Supplier { Name = "TestSupplier", PriceToHome = 25, PriceToOffice = 30 },
                },
            };

            var shoppingCartProducts = new List<ShoppingCartProductViewModel>
            {
                new ShoppingCartProductViewModel { ProductId = "TestProductId1", },
                new ShoppingCartProductViewModel { ProductId = "TestProductId2", ProductPrice = 42, Quantity = 5 },
                new ShoppingCartProductViewModel { ProductId = "TestProductId3", ProductPrice = 25, Quantity = 4 },
            };

            repository.Setup(r => r.All()).Returns(orders.Where(x => !x.IsDeleted).AsQueryable());
            repository.Setup(r => r.AllAsNoTracking()).Returns(orders.Where(x => !x.IsDeleted).AsQueryable());
            repository.Setup(r => r.Update(It.IsAny<Order>())).Callback((Order item) =>
            {
                var foundOrder = orders.FirstOrDefault(x => x.Id == item.Id);
                foundOrder.TotalPrice = item.TotalPrice;
                foundOrder.Status = item.Status;
            });
            repository.Setup(r => r.SaveChangesAsync()).Verifiable();

            shoppingCartService.Setup(r => r.GetAllProductsAsync<ShoppingCartProductViewModel>(It.IsAny<bool>(), It.IsAny<ISession>(), It.IsAny<string>())).Returns((bool isAuthenticated, ISession session, string userId) =>
            {
                return Task.FromResult<IEnumerable<ShoppingCartProductViewModel>>(shoppingCartProducts);
            });
            shoppingCartService.Setup(r => r.DeleteAllProductsAsync(It.IsAny<string>())).Callback((string userId) =>
            {
                shoppingCartProducts.Clear();
            });

            viewRenderService.Setup(r => r.RenderToStringAsync(It.IsAny<string>(), It.IsAny<object>())).Verifiable();

            emailSender.Setup(r => r.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), null)).Verifiable();

            var service = new OrdersService(repository.Object, null, null, null, shoppingCartService.Object, emailSender.Object, viewRenderService.Object);
            Assert.Equal("TestOrderId1", await service.CompleteOrderAsync("TestUserId1"));
            Assert.Equal(310, orders.FirstOrDefault(x => x.Id == "TestOrderId1").TotalPrice);
            Assert.Equal(OrderStatus.Unprocessed, orders.FirstOrDefault(x => x.Id == "TestOrderId1").Status);
            Assert.Equal(3, orders.FirstOrDefault(x => x.Id == "TestOrderId1").Products.Count);
            Assert.Empty(shoppingCartProducts);

            repository.Verify(x => x.All(), Times.Once);
            repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(4));
            repository.Verify(x => x.Update(It.IsAny<Order>()), Times.Once);
            repository.Verify(x => x.SaveChangesAsync());

            shoppingCartService.Verify(x => x.GetAllProductsAsync<ShoppingCartProductViewModel>(It.IsAny<bool>(), It.IsAny<ISession>(), It.IsAny<string>()), Times.Once);
            shoppingCartService.Verify(x => x.DeleteAllProductsAsync(It.IsAny<string>()), Times.Once);

            viewRenderService.Verify(x => x.RenderToStringAsync(It.IsAny<string>(), It.IsAny<object>()));

            emailSender.Verify(e => e.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), null));
        }
    }
}
