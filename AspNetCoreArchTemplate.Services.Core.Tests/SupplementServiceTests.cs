using Microsoft.EntityFrameworkCore;
using PowerNutrition.Data;
using PowerNutrition.Data.Models;
using PowerNutrition.Web.ViewModels.Supplement;
using PowerNutrition.Services.Core;

[TestFixture]
public class SupplementServiceTests
{
    private PowerNutritionDbContext dbContext;
    private SupplementService supplementService;
    private int proteinCategoryId;
    private int vitaminCategoryId;
    private Supplement supplement1;
    private Supplement supplement2;
    private Category vitamins;
    private Category protein;

    [SetUp]
    public void Setup()
    {
        DbContextOptions<PowerNutritionDbContext> options = new DbContextOptionsBuilder<PowerNutritionDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        dbContext = new PowerNutritionDbContext(options);

        vitamins = new Category { Id = 1, Name = "Vitamins" };
        protein = new Category { Id = 2, Name = "Protein" };
        proteinCategoryId = protein.Id;
        vitaminCategoryId = vitamins.Id;

        supplement1 = new Supplement
        {
            Id = Guid.NewGuid(),
            Name = "Whey Protein",
            Description = "Protein supplement",
            Brand = "Optimum",
            ImageUrl = "https://example.com/whey.jpg",
            Price = 29.99m,
            Stock = 10,
            Weight = 2.0,
            CategoryId = proteinCategoryId,
            Category = protein
        };

        supplement2 = new Supplement
        {
            Id = Guid.NewGuid(),
            Name = "Vitamin C",
            Description = "Immune booster",
            Brand = "Nature's Bounty",
            ImageUrl = "https://example.com/vitc.jpg",
            Price = 9.99m,
            Stock = 15,
            Weight = 0.2,
            CategoryId = vitaminCategoryId,
            Category = vitamins
        };

        dbContext.Supplements.AddRange(supplement1, supplement2);
        dbContext.Categories.AddRange(vitamins, protein);
        dbContext.SaveChanges();

        supplementService = new SupplementService(dbContext);

    }
    [TearDown]
    public void TearDown()
    {
        dbContext.Dispose();
    }
    [Test]
    public async Task GetAllSupplementsAsync_WithNullCategory_ReturnsAllSupplements()
    {
        SupplementsPageViewModel result = await supplementService.GetAllSupplementsAsync(null);

        Assert.IsNotNull(result);
        Assert.That(result.Supplements.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetAllSupplementsAsync_WithValidCategoryFilter_ReturnsFilteredSupplements()
    {
        SupplementsPageViewModel result = await supplementService.GetAllSupplementsAsync(1);

        Assert.IsNotNull(result);
        Assert.That(result.Supplements.Count(), Is.EqualTo(1));
        Assert.That(result.CategoryName, Is.EqualTo("Vitamins"));
    }
    [Test]
    public async Task GetDetailsForSupplementAsync_ValidId_ReturnsCorrectDetails()
    {
        string supplementId = (await dbContext.Supplements.FirstAsync()).Id.ToString();

        DetailsSupplementViewmodel? result = await supplementService.GetDetailsForSupplementAsync(supplementId);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(supplementId));
        Assert.That(result.Name, Is.EqualTo("Whey Protein"));
        Assert.That(result.Description, Is.EqualTo("Protein supplement"));
        Assert.That(result.Brand, Is.EqualTo("Optimum"));
        Assert.That(result.Name, Is.EqualTo("Whey Protein"));
        Assert.That(result.ImageUrl, Is.EqualTo("https://example.com/whey.jpg"));
        Assert.That(result.Price, Is.EqualTo("29.99"));
        Assert.That(result.Quantity, Is.EqualTo("10"));
        Assert.That(result.Weight, Is.EqualTo("2"));
        Assert.That(result.Category, Is.EqualTo("Protein"));
    }

    [Test]
    public async Task GetDetailsForSupplementAsync_InvalidId_ReturnsNull()
    {

        DetailsSupplementViewmodel? result = await supplementService.GetDetailsForSupplementAsync(Guid.NewGuid().ToString());

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetDetailsForSupplementAsync_NullId_ReturnsNull()
    {
        DetailsSupplementViewmodel? result = await supplementService.GetDetailsForSupplementAsync(null);

        Assert.That(result, Is.Null);
    }
    [Test]
    public async Task GetSupplementToDelete_ValidId_ReturnsCorrectModel()
    {
        string id = supplement1.Id.ToString();

        SupplementDeleteInputModel? result = await supplementService.GetSupplementToDelete(id);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(id));
        Assert.That(result.Name, Is.EqualTo("Whey Protein"));
    }
    [Test]
    public async Task DeleteSupplement_ValidId_SetsIsDeletedTrue()
    {
        SupplementDeleteInputModel inputModel = new SupplementDeleteInputModel
        {
            Id = supplement1.Id.ToString(),
            Name = supplement1.Name
        };

        bool result = await supplementService.DeleteSupplement(inputModel);
        Supplement? deletedSupplement = await dbContext.Supplements.FindAsync(supplement1.Id);

        Assert.That(result, Is.True);
        Assert.That(deletedSupplement.IsDeleted, Is.True);
    }

    [Test]
    public async Task DeleteSupplement_InvalidId_ReturnsFalse()
    {
        SupplementDeleteInputModel inputModel = new SupplementDeleteInputModel
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Nonexistent"
        };

        bool result = await supplementService.DeleteSupplement(inputModel);

        Assert.That(result, Is.False);
    }

    [Test]
    public async Task DeleteSupplement_NullId_ReturnsFalse()
    {
        SupplementDeleteInputModel inputModel = new SupplementDeleteInputModel
        {
            Id = null!,
            Name = "No Id"
        };

        bool result = await supplementService.DeleteSupplement(inputModel);

        Assert.That(result, Is.False);
    }

    [Test]
    public async Task DeleteSupplement_MalformedId_ReturnsFalse()
    {
        SupplementDeleteInputModel inputModel = new SupplementDeleteInputModel
        {
            Id = "invalid-guid",
            Name = "Malformed Id"
        };

        bool result = await supplementService.DeleteSupplement(inputModel);

        Assert.That(result, Is.False);
    }
    [Test]
    public async Task GetSupplementForEditAsync_ValidId_ReturnsEditModel()
    {
        SupplementEditInputModel? result = await supplementService.GetSupplementForEditAsync(supplement1.Id.ToString());

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(supplement1.Id.ToString()));
        Assert.That(result.Name, Is.EqualTo(supplement1.Name));
    }

    [Test]
    public async Task GetSupplementForEditAsync_InvalidId_ReturnsNull()
    {
        SupplementEditInputModel? result = await supplementService.GetSupplementForEditAsync(Guid.NewGuid().ToString());

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetSupplementForEditAsync_NullId_ReturnsNull()
    {
        SupplementEditInputModel? result = await supplementService.GetSupplementForEditAsync(null);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetSupplementForEditAsync_MalformedId_ReturnsNull()
    {
        SupplementEditInputModel? result = await supplementService.GetSupplementForEditAsync("invalid-guid");

        Assert.That(result, Is.Null);
    }
    [Test]
    public async Task PersistEditSupplementAsync_ValidInput_UpdatesAndReturnsId()
    {
        SupplementEditInputModel editModel = new SupplementEditInputModel
        {
            Id = supplement1.Id.ToString(),
            Name = "Updated Name",
            Description = "Updated Description",
            Brand = "Updated Brand",
            Price = 49.99m,
            ImageUrl = "https://example.com/updated.jpg",
            Weigth = 2.5,
            Stock = 20,
            CategoryId = supplement1.CategoryId!.Value
        };

        Guid? result = await supplementService.PersistEditSupplementAsync(editModel);
        Supplement? updatedSupplement = await dbContext.Supplements.FindAsync(supplement1.Id);

        Assert.That(result, Is.EqualTo(supplement1.Id));
        Assert.That(updatedSupplement, Is.Not.Null);
        Assert.That(updatedSupplement!.Name, Is.EqualTo("Updated Name"));
        Assert.That(updatedSupplement.Description, Is.EqualTo("Updated Description"));
        Assert.That(updatedSupplement.Brand, Is.EqualTo("Updated Brand"));
        Assert.That(updatedSupplement.Price, Is.EqualTo(49.99m));
        Assert.That(updatedSupplement.ImageUrl, Is.EqualTo("https://example.com/updated.jpg"));
        Assert.That(updatedSupplement.Weight, Is.EqualTo(2.5));
        Assert.That(updatedSupplement.Stock, Is.EqualTo(20));
        Assert.That(updatedSupplement.CategoryId, Is.EqualTo(supplement1.CategoryId));
    }

    [Test]
    public async Task PersistEditSupplementAsync_InvalidCategoryId_ReturnsNull()
    {
        SupplementEditInputModel editModel = new SupplementEditInputModel
        {
            Id = supplement1.Id.ToString(),
            Name = "Name",
            Description = "Description",
            Brand = "Brand",
            Price = 10m,
            ImageUrl = "https://example.com/image.jpg",
            Weigth = 1.0,
            Stock = 5,
            CategoryId = 999
        };

        Guid? result = await supplementService.PersistEditSupplementAsync(editModel);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task PersistEditSupplementAsync_InvalidId_ReturnsNull()
    {
        SupplementEditInputModel editModel = new SupplementEditInputModel
        {
            Id = "invalid-guid",
            Name = "Name",
            Description = "Description",
            Brand = "Brand",
            Price = 10m,
            ImageUrl = "https://example.com/image.jpg",
            Weigth = 1.0,
            Stock = 5,
            CategoryId = supplement1.CategoryId!.Value
        };

        Guid? result = await supplementService.PersistEditSupplementAsync(editModel);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task PersistEditSupplementAsync_NonexistentSupplement_ReturnsNull()
    {
        SupplementEditInputModel editModel = new SupplementEditInputModel
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Name",
            Description = "Description",
            Brand = "Brand",
            Price = 10m,
            ImageUrl = "https://example.com/image.jpg",
            Weigth = 1.0,
            Stock = 5,
            CategoryId = supplement1.CategoryId!.Value
        };

        Guid? result = await supplementService.PersistEditSupplementAsync(editModel);

        Assert.That(result, Is.Null);
    }
    [Test]
    public async Task PersistAddSupplementAsync_ValidInput_AddsAndReturnsNewId()
    {
        AddSupplementInputModel inputModel = new AddSupplementInputModel
        {
            Name = "New Supplement",
            Description = "New Description",
            Brand = "New Brand",
            Price = 19.99m,
            ImageUrl = "https://example.com/new.jpg",
            Stock = 25,
            Weigth = 1.5,
            CategoryId = vitaminCategoryId
        };

        Guid? newId = await supplementService.PersistAddSupplementAsync(inputModel);
        Supplement? addedSupplement = await dbContext.Supplements.FindAsync(newId);

        Assert.That(newId, Is.Not.Null);
        Assert.That(addedSupplement, Is.Not.Null);
        Assert.That(addedSupplement!.Name, Is.EqualTo(inputModel.Name));
        Assert.That(addedSupplement.Description, Is.EqualTo(inputModel.Description));
    }

    [Test]
    public async Task PersistAddSupplementAsync_InvalidCategory_ReturnsNull()
    {
        AddSupplementInputModel inputModel = new AddSupplementInputModel
        {
            Name = "Invalid Category Supp",
            Description = "Desc",
            Brand = "Brand",
            Price = 9.99m,
            ImageUrl = "https://example.com/invalid.jpg",
            Stock = 10,
            Weigth = 0.5,
            CategoryId = 999
        };

        Guid? result = await supplementService.PersistAddSupplementAsync(inputModel);

        Assert.That(result, Is.Null);
    }

}
