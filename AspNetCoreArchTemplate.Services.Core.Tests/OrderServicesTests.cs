namespace PowerNutrition.Services.Core.Tests
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using PowerNutrition.Data;
    using PowerNutrition.Data.Models;
    using PowerNutrition.Services.Core;
    using PowerNutrition.Services.Core.Interfaces;
    using PowerNutrition.Web.ViewModels.Order;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class OrderServicesTests
    {
        private PowerNutritionDbContext dbContext;
        private OrderService service;

        private string userId;
        private Guid supplementId;

        [SetUp]
        public async Task SetUp()
        {
            DbContextOptions<PowerNutritionDbContext> options = new DbContextOptionsBuilder<PowerNutritionDbContext>()
                .UseInMemoryDatabase(databaseName: "OrderTestDb")
                .Options;

            dbContext = new PowerNutritionDbContext(options);

            userId = Guid.NewGuid().ToString();
            supplementId = Guid.NewGuid();

            ApplicationUser user = new ApplicationUser
            {
                Id = userId,
                UserName = "testuser",
                Email = "test@example.com"
            };

            await dbContext.Users.AddAsync(user);

            Supplement supplement = new Supplement
            {
                Id = supplementId,
                Name = "Whey Protein",
                Brand = "Optimum",
                Price = 50,
                Stock = 10,
                ImageUrl = "http://example.com/image.jpg",
                Description = "Protein"
            };

            CartItem cartItem = new CartItem
            {
                UserId = userId,
                SupplementId = supplementId,
                Quantity = 2
            };

            await dbContext.Supplements.AddAsync(supplement);
            await dbContext.CartsItems.AddAsync(cartItem);
            await dbContext.SaveChangesAsync();

            UserManager<IdentityUser> fakeManager = null!;

            service = new OrderService(dbContext, fakeManager);
        }
        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        [Test]
        public async Task PlaceOrderAsync_ShouldCreateOrderAndClearCart_WhenValid()
        {
            OrderInputModel input = new OrderInputModel
            {
                Address = "123 Main St",
                City = "Sofia",
                PhoneNumber = "0888123456",
                PostCode = "1000"
            };

            bool result = await service.PlaceOrderAsync(userId, input);

            Assert.That(result, Is.True);

            Order? order = await dbContext.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.UserId == userId);
            Assert.That(order, Is.Not.Null);
            Assert.That(order!.TotalPrice, Is.EqualTo(100));
            Assert.That(order.Items.Count, Is.EqualTo(1));

            List<CartItem> cart = await dbContext.CartsItems.Where(c => c.UserId == userId).ToListAsync();
            Assert.That(cart.Count, Is.EqualTo(0));
        }
        [Test]
        public async Task PlaceOrderAsync_ShouldReturnFalse_WhenUserIdIsNull()
        {
            OrderInputModel input = new OrderInputModel
            {
                Address = "Nope",
                City = "Nowhere",
                PhoneNumber = "000",
                PostCode = "0000"
            };

            bool result = await service.PlaceOrderAsync(null, input);

            Assert.That(result, Is.False);
        }
        [Test]
        public async Task PlaceOrderAsync_ShouldReturnFalse_WhenCartIsEmpty()
        {
            dbContext.CartsItems.RemoveRange(dbContext.CartsItems);
            await dbContext.SaveChangesAsync();

            OrderInputModel input = new OrderInputModel
            {
                Address = "Empty",
                City = "Cart",
                PhoneNumber = "000",
                PostCode = "0000"
            };

            bool result = await service.PlaceOrderAsync(userId, input);

            Assert.That(result, Is.False);
        }
        [Test]
        public async Task GetUserOrderHistoryAsync_ShouldReturnOrders_WhenUserExists()
        {
            Guid supplementId = Guid.NewGuid();
            string userId = "testUser123";

            Supplement supplement = new Supplement
            {
                Id = supplementId,
                Name = "Test Supplement",
                Brand = "Test Brand",
                Description = "Test Description",
                Price = 25.00m,
                Stock = 10,
                ImageUrl = "https://example.com/image.png"
            };

            CartItem cartItem = new CartItem
            {
                UserId = userId,
                SupplementId = supplementId,
                Quantity = 2
            };

            await dbContext.Supplements.AddAsync(supplement);
            await dbContext.CartsItems.AddAsync(cartItem);
            await dbContext.SaveChangesAsync();

            OrderInputModel input = new OrderInputModel
            {
                Address = "123 Test Street",
                City = "Testville",
                PhoneNumber = "123456789",
                PostCode = "12345"
            };

            bool placed = await service.PlaceOrderAsync(userId, input);
            Assert.That(placed, Is.True);

            IEnumerable<UserOrderHistoryViewmodel>? result = await service.GetUserOrderHistoryAsync(userId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetUserOrderHistoryAsync_ShouldReturnNull_WhenUserIdIsNull()
        {
            IEnumerable<UserOrderHistoryViewmodel>? result = await service.GetUserOrderHistoryAsync(null);

            Assert.That(result, Is.Null);
        }
        [Test]
        public async Task GetUserOrderHistoryAsync_ShouldReturnEmpty_WhenNoOrdersExist()
        {
            IEnumerable<UserOrderHistoryViewmodel>? result = await service.GetUserOrderHistoryAsync(userId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Any(), Is.False);
        }
        [Test]
        public async Task GetAllOrdersWithStatusPendingAsync_ShouldReturnPendingOrders()
        {
            OrderInputModel input = new OrderInputModel
            {
                Address = "Test Address",
                City = "Pending City",
                PhoneNumber = "0123456789",
                PostCode = "1234"
            };

            await service.PlaceOrderAsync(userId, input);

            IEnumerable<OrdersWithStatusPendingViewmodel>? result = await service.GetAllOrdersWithStatusPendingAsync();

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Count(), Is.EqualTo(1));
            Assert.That(result.First().City, Is.EqualTo("Pending City"));
        }
        [Test]
        public async Task GetUserOrderHistoryAsync_ShouldMapSupplementDetailsCorrectly()
        {
            OrderInputModel input = new OrderInputModel
            {
                Address = "History Street",
                City = "Histon",
                PhoneNumber = "0899123456",
                PostCode = "2000"
            };

            await service.PlaceOrderAsync(userId, input);

            IEnumerable<UserOrderHistoryViewmodel>? result = await service.GetUserOrderHistoryAsync(userId);
            UserOrderHistoryViewmodel? order = result!.FirstOrDefault();

            Assert.That(order, Is.Not.Null);
            Assert.That(order!.Supplements.Count(), Is.EqualTo(1));

            var supplement = order.Supplements.First();

            Assert.That(supplement.Name, Is.EqualTo("Whey Protein"));
            Assert.That(supplement.Brand, Is.EqualTo("Optimum"));
            Assert.That(supplement.TotalPrice, Is.EqualTo((2 * 50).ToString()));
        }
    }
}
