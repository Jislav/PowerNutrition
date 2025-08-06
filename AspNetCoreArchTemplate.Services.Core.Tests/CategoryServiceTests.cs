namespace PowerNutrition.Services.Core.Tests
{
    using NUnit.Framework;
    using PowerNutrition.Data;
    using PowerNutrition.Data.Models;
    using PowerNutrition.Services.Core;
    using PowerNutrition.Web.ViewModels.Category;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;

    [TestFixture]
    public class CategoryServiceTests
    {
        private PowerNutritionDbContext dbContext;
        private CategoryService service;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptions<PowerNutritionDbContext> options = new DbContextOptionsBuilder<PowerNutritionDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            dbContext = new PowerNutritionDbContext(options);

            Category cat1 = new Category { Name = "Supplements" };
            Category cat2 = new Category { Name = "Vitamins" };

            await dbContext.Categories.AddAsync(cat1);
            await dbContext.Categories.AddAsync(cat2);
            await dbContext.SaveChangesAsync();

            service = new CategoryService(dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        [Test]
        public async Task GetAllCategoriesAsync_ReturnsAllCategories()
        {
            ICollection<CategoriesListViewmodel> categories = await service.GetAllCategoriesAsync();

            Assert.That(categories, Is.Not.Null);
            Assert.That(categories.Count, Is.EqualTo(2));
            Assert.That(categories.Any(c => c.Name == "Supplements"), Is.True);
            Assert.That(categories.Any(c => c.Name == "Vitamins"), Is.True);
        }

        [Test]
        public async Task AddCategoryAsync_AddsCategory_WhenNewName()
        {
            AddCategoryInputModel input = new AddCategoryInputModel { Name = "Proteins" };

            bool result = await service.AddCategoryAsync(input);

            Assert.That(result, Is.True);
            bool exists = await dbContext.Categories.AnyAsync(c => c.Name == "Proteins");
            Assert.That(exists, Is.True);
        }

        [Test]
        public async Task AddCategoryAsync_ReturnsFalse_WhenCategoryExists()
        {
            AddCategoryInputModel input = new AddCategoryInputModel { Name = "supplements" };

            bool result = await service.AddCategoryAsync(input);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task AddCategoryAsync_ReturnsFalse_WhenInputIsNull()
        {
            bool result = await service.AddCategoryAsync(null!);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task GetCategoryForDeletingAsync_ReturnsCategory_WhenValidId()
        {
            Category category = await dbContext.Categories.FirstAsync();

            CategoryDeleteInputModel? result = await service.GetCategoryForDeletingAsync(category.Id.ToString());

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Id, Is.EqualTo(category.Id));
            Assert.That(result.Name, Is.EqualTo(category.Name));
        }

        [Test]
        public async Task GetCategoryForDeletingAsync_ReturnsNull_WhenIdIsNull()
        {
            CategoryDeleteInputModel? result = await service.GetCategoryForDeletingAsync(null);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetCategoryForDeletingAsync_ReturnsNull_WhenIdIsInvalid()
        {
            CategoryDeleteInputModel? result = await service.GetCategoryForDeletingAsync("abc");

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetCategoryForDeletingAsync_ReturnsNull_WhenCategoryNotFound()
        {
            CategoryDeleteInputModel? result = await service.GetCategoryForDeletingAsync(int.MaxValue.ToString());

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task PersistCategoryDelete_DeletesCategoryAndMarksSupplementsAsDeleted()
        {
            Category category = new Category { Name = "ToDelete" };

            Supplement supplement = new Supplement
            { 
                Name = "TestSupp",
                Description = "Test Description",
                Brand = "Test Brand",
                ImageUrl = "Some url",
                IsDeleted = false,
            };
            category.Supplements.Add(supplement);

            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();

            CategoryDeleteInputModel input = new CategoryDeleteInputModel { Id = category.Id, Name = category.Name };

            bool result = await service.PersistCategoryDelete(input);

            Assert.That(result, Is.True);

            Category? deletedCategory = await dbContext.Categories.FindAsync(category.Id);
            Assert.That(deletedCategory, Is.Null);

            Supplement? deletedSupplement = await dbContext.Supplements.FindAsync(supplement.Id);
            Assert.That(deletedSupplement, Is.Not.Null);
            Assert.That(deletedSupplement!.IsDeleted, Is.True);
        }

        [Test]
        public async Task PersistCategoryDelete_ReturnsFalse_WhenInputIsNull()
        {
            bool result = await service.PersistCategoryDelete(null!);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task CheckIfCategoryExists_ReturnsTrue_WhenCategoryExists()
        {
            Category category = await dbContext.Categories.FirstAsync();

            bool exists = await service.CheckIfCategoryExists(category.Id);

            Assert.That(exists, Is.True);
        }

        [Test]
        public async Task CheckIfCategoryExists_ReturnsFalse_WhenCategoryDoesNotExist()
        {
            bool exists = await service.CheckIfCategoryExists(int.MaxValue);

            Assert.That(exists, Is.False);
        }

        [Test]
        public async Task CheckIfCategoryExists_ReturnsFalse_WhenIdIsNull()
        {
            bool exists = await service.CheckIfCategoryExists(null);

            Assert.That(exists, Is.False);
        }
    }
}