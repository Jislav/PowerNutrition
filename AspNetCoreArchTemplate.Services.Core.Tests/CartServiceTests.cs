namespace PowerNutrition.Services.Core.Tests
{
    using Microsoft.EntityFrameworkCore;
    using PowerNutrition.Data;
    using PowerNutrition.Data.Models;
    using PowerNutrition.Services.Core;
    using PowerNutrition.Web.ViewModels.Cart;

    [TestFixture]
    public class CartServiceTests
    {
        private PowerNutritionDbContext dbContext;
        private CartService service;

        private string userId;
        private Guid supplementId;

        private string testUserId;
        private Guid supplementId1;
        private Guid supplementId2;

        [SetUp]
        public async Task SetUp()
        {
            DbContextOptions<PowerNutritionDbContext> options = new DbContextOptionsBuilder<PowerNutritionDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            dbContext = new PowerNutritionDbContext(options);

            userId = "user123";
            supplementId = Guid.NewGuid();

            testUserId = "testUser";
            supplementId1 = Guid.NewGuid();
            supplementId2 = Guid.NewGuid();

            Supplement mainSupplement = new Supplement
            {
                Id = supplementId,
                Name = "Test Supplement",
                Description = "Protein supplement for muscle gain",
                Brand = "Test Brand",
                Stock = 5,
                Price = 20.0m,
                ImageUrl = "http://example.com/img.png"
            };

            Supplement supplementOne = new Supplement
            {
                Id = supplementId1,
                Name = "Supplement 1",
                Description = "Protein supplement for muscle gain",
                Brand = "Test Brand",
                Price = 10.5m,
                Stock = 10,
                ImageUrl = "url1"
            };

            Supplement supplementTwo = new Supplement
            {
                Id = supplementId2,
                Name = "Supplement 2",
                Description = "Protein supplement for muscle gain",
                Brand = "Test Brand",
                Price = 20.0m,
                Stock = 5,
                ImageUrl = "url2"
            };

            dbContext.Supplements.AddRange(mainSupplement, supplementOne, supplementTwo);

            dbContext.CartsItems.AddRange(
                new CartItem
                {
                    UserId = testUserId,
                    SupplementId = supplementId1,
                    Quantity = 2
                },
                new CartItem
                {
                    UserId = testUserId,
                    SupplementId = supplementId2,
                    Quantity = 3
                },
                new CartItem
                {
                    UserId = "otherUser",
                    SupplementId = Guid.NewGuid(),
                    Quantity = 1
                });

            await dbContext.SaveChangesAsync();

            service = new CartService(dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        [Test]
        public async Task AddToCartAsync_ShouldAddNewCartItem_WhenNotInCart()
        {
            bool result = await service.AddToCartAsync(userId, supplementId.ToString());

            Assert.That(result, Is.True);

            CartItem? cartItem = await dbContext.CartsItems.FirstOrDefaultAsync(ci =>
                ci.UserId == userId && ci.SupplementId == supplementId);

            Assert.That(cartItem, Is.Not.Null);

            Supplement? updatedSupplement = await dbContext.Supplements.FindAsync(supplementId);
            Assert.That(updatedSupplement!.Stock, Is.EqualTo(4));
        }

        [Test]
        public async Task AddToCartAsync_ShouldIncreaseQuantity_WhenAlreadyInCart()
        {
            if (!await dbContext.CartsItems.AnyAsync(ci => ci.UserId == userId && ci.SupplementId == supplementId))
            {
                dbContext.CartsItems.Add(new CartItem
                {
                    UserId = userId,
                    SupplementId = supplementId,
                    Quantity = 1
                });
                await dbContext.SaveChangesAsync();
            }

            bool result = await service.AddToCartAsync(userId, supplementId.ToString());

            Assert.That(result, Is.True);

            CartItem cartItem = await dbContext.CartsItems.FirstAsync(ci => ci.UserId == userId && ci.SupplementId == supplementId);
            Assert.That(cartItem.Quantity, Is.EqualTo(2));

            Supplement? updatedSupplement = await dbContext.Supplements.FindAsync(supplementId);
            Assert.That(updatedSupplement!.Stock, Is.EqualTo(4));
        }

        [Test]
        public async Task AddToCartAsync_ShouldReturnFalse_WhenStockIsZero()
        {
            Supplement? supplement = await dbContext.Supplements.FindAsync(supplementId);
            supplement!.Stock = 0;
            await dbContext.SaveChangesAsync();

            bool result = await service.AddToCartAsync(userId, supplementId.ToString());

            Assert.That(result, Is.False);

            CartItem? cartItem = await dbContext.CartsItems.FirstOrDefaultAsync(ci => ci.UserId == userId && ci.SupplementId == supplementId);
            Assert.That(cartItem, Is.Null);
        }

        [Test]
        public async Task GetAllCartItemsAsync_ShouldReturnCartItems_ForGivenUser()
        {
            IEnumerable<AllCartItemsViewmodel>? result = await service.GetAllCartItemsAsync(testUserId);

            Assert.That(result, Is.Not.Null);
            List<AllCartItemsViewmodel> items = result!.ToList();
            Assert.That(items.Count, Is.EqualTo(2));

            AllCartItemsViewmodel? firstItem = items.FirstOrDefault(i => i.Id == supplementId1.ToString());
            Assert.That(firstItem, Is.Not.Null);
            Assert.That(firstItem!.Name, Is.EqualTo("Supplement 1"));
            Assert.That(firstItem.Price, Is.EqualTo("10.50"));
            Assert.That(firstItem.Amount, Is.EqualTo("2"));
            Assert.That(firstItem.ImageUrl, Is.EqualTo("url1"));

            AllCartItemsViewmodel? secondItem = items.FirstOrDefault(i => i.Id == supplementId2.ToString());
            Assert.That(secondItem, Is.Not.Null);
            Assert.That(secondItem!.Name, Is.EqualTo("Supplement 2"));
            Assert.That(secondItem.Price, Is.EqualTo("20.00"));
            Assert.That(secondItem.Amount, Is.EqualTo("3"));
            Assert.That(secondItem.ImageUrl, Is.EqualTo("url2"));
        }

        [Test]
        public async Task GetAllCartItemsAsync_ShouldReturnNull_WhenUserIdIsNull()
        {
            IEnumerable<AllCartItemsViewmodel>? result = await service.GetAllCartItemsAsync(null);

            Assert.That(result, Is.Null);
        }
        [Test]
        public async Task RemoveFromCartAsync_ShouldRemoveCartItemAndUpdateStock_WhenItemExists()
        {
            CartItem newCartItem = new CartItem
            {
                UserId = userId,
                SupplementId = supplementId,
                Quantity = 2
            };
            dbContext.CartsItems.Add(newCartItem);

            Supplement? supplementBefore = await dbContext.Supplements.FindAsync(supplementId);
            int initialStock = supplementBefore!.Stock;

            await dbContext.SaveChangesAsync();

            bool result = await service.RemoveFromCartAsync(userId, supplementId.ToString());

            Assert.That(result, Is.True);

            CartItem? removedCartItem = await dbContext.CartsItems.FirstOrDefaultAsync(ci => ci.UserId == userId && ci.SupplementId == supplementId);
            Assert.That(removedCartItem, Is.Null);

            Supplement? updatedSupplement = await dbContext.Supplements.FindAsync(supplementId);
            Assert.That(updatedSupplement!.Stock, Is.EqualTo(initialStock + 2));
        }

        [Test]
        public async Task RemoveFromCartAsync_ShouldReturnFalse_WhenCartItemDoesNotExist()
        {
            bool result = await service.RemoveFromCartAsync(userId, supplementId.ToString());

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task RemoveFromCartAsync_ShouldReturnFalse_WhenUserIdIsNull()
        {
            bool result = await service.RemoveFromCartAsync(null, supplementId.ToString());

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task RemoveFromCartAsync_ShouldReturnFalse_WhenSupplementIdIsNull()
        {
            bool result = await service.RemoveFromCartAsync(userId, null);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task RemoveFromCartAsync_ShouldReturnFalse_WhenCartItemBelongsToDifferentUser()
        {
            CartItem cartItemForOtherUser = new CartItem
            {
                UserId = "otherUser",
                SupplementId = supplementId,
                Quantity = 1
            };
            dbContext.CartsItems.Add(cartItemForOtherUser);
            await dbContext.SaveChangesAsync();

            bool result = await service.RemoveFromCartAsync(userId, supplementId.ToString());

            Assert.That(result, Is.False);

            CartItem? cartItemStillExists = await dbContext.CartsItems.FirstOrDefaultAsync(ci => ci.UserId == "otherUser" && ci.SupplementId == supplementId);
            Assert.That(cartItemStillExists, Is.Not.Null);
        }
    }
}