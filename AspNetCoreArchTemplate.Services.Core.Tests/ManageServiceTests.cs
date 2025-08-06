namespace PowerNutrition.Services.Core.Tests
{
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using PowerNutrition.Data;
    using PowerNutrition.Data.Models;
    using PowerNutrition.Data.Models.Enums;
    using PowerNutrition.Services.Core;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using PowerNutrition.Web.ViewModels.Manage;

    [TestFixture]
    public class ManageServiceTests
    {
        private PowerNutritionDbContext dbContext;
        private ManageService manageService;

        [SetUp]
        public void Setup()
        {
            DbContextOptions<PowerNutritionDbContext> options = new DbContextOptionsBuilder<PowerNutritionDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            dbContext = new PowerNutritionDbContext(options);

            manageService = new ManageService(dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        [Test]
        public async Task ApproveOrderAsync_ValidOrderId_ApprovesOrderAndReturnsId()
        {
            Order order = new Order
            {
                UserId = Guid.NewGuid().ToString(),
                Status = OrderStatus.Pending,
                Address = "Test Address",
                PhoneNumber = "1234567890",
                PostCode = "0000",
                City = "Test City"
            };

            await dbContext.Orders.AddAsync(order);
            await dbContext.SaveChangesAsync();

            Guid? approvedOrderId = await manageService.ApproveOrderAsync(order.Id.ToString());

            Assert.That(approvedOrderId, Is.EqualTo(order.Id));
            Order? updatedOrder = await dbContext.Orders.FindAsync(order.Id);
            Assert.That(updatedOrder!.Status, Is.EqualTo(OrderStatus.Approved));
        }

        [Test]
        public async Task ApproveOrderAsync_InvalidGuid_ReturnsNull()
        {
            Guid? approvedOrderId = await manageService.ApproveOrderAsync("not-a-guid");

            Assert.That(approvedOrderId, Is.Null);
        }

        [Test]
        public async Task ApproveOrderAsync_NonExistentOrderId_ReturnsNull()
        {
            string nonExistentOrderId = Guid.NewGuid().ToString();

            Guid? approvedOrderId = await manageService.ApproveOrderAsync(nonExistentOrderId);

            Assert.That(approvedOrderId, Is.Null);
        }

        [Test]
        public async Task GetAllSupplementsEditListAsync_ReturnsAllSupplementsWithCorrectData()
        {
            Category category = new Category { Id = 1, Name = "Protein" };
            Supplement supplement = new Supplement
            {
                Id = Guid.NewGuid(),
                Name = "Whey Protein",
                Brand = "Optimum",
                Description = "Test Description",
                ImageUrl = "https://example.com/whey.jpg",
                Price = 49.99m,
                Stock = 10,
                Category = category
            };

            await dbContext.Categories.AddAsync(category);
            await dbContext.Supplements.AddAsync(supplement);
            await dbContext.SaveChangesAsync();

            IEnumerable<AllSupplemenetsEditViewmodel> supplements =
                await manageService.GetAllSupplementsEditListAsync();

            Assert.That(supplements, Is.Not.Null);
            AllSupplemenetsEditViewmodel? first = supplements.FirstOrDefault();
            Assert.That(first, Is.Not.Null);
            Assert.That(first.Name, Is.EqualTo(supplement.Name));
            Assert.That(first.Brand, Is.EqualTo(supplement.Brand));
            Assert.That(first.Category, Is.EqualTo(category.Name));
            Assert.That(first.Price, Is.EqualTo(supplement.Price.ToString("f2")));
            Assert.That(first.Quantity, Is.EqualTo(supplement.Stock.ToString()));
        }

        [Test]
        public async Task GetAllSupplementsDeleteListAsync_ReturnsAllSupplementsWithCorrectData()
        {
            Category category = new Category { Id = 2, Name = "Vitamins" };
            Supplement supplement = new Supplement
            {
                Id = Guid.NewGuid(),
                Name = "Vitamin C",
                Brand = "Nature's Bounty",
                Description = "Test Description",
                ImageUrl = "https://example.com/vitamins.jpg",
                Price = 19.99m,
                Stock = 5,
                Category = category
            };

            await dbContext.Categories.AddAsync(category);
            await dbContext.Supplements.AddAsync(supplement);
            await dbContext.SaveChangesAsync();

            IEnumerable<AllSupplementsDeleteViewmodel> supplements =
                await manageService.GetAllSupplementsDeleteListAsync();

            Assert.That(supplements, Is.Not.Null);
            AllSupplementsDeleteViewmodel? first = supplements.FirstOrDefault();
            Assert.That(first, Is.Not.Null);
            Assert.That(first.Name, Is.EqualTo(supplement.Name));
            Assert.That(first.Brand, Is.EqualTo(supplement.Brand));
            Assert.That(first.Category, Is.EqualTo(category.Name));
            Assert.That(first.Price, Is.EqualTo(supplement.Price.ToString("f2")));
            Assert.That(first.Quantity, Is.EqualTo(supplement.Stock.ToString()));
        }
    }
}