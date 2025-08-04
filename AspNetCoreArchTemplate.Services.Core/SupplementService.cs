namespace PowerNutrition.Services.Core
{
    using Microsoft.EntityFrameworkCore;
    using PowerNutrition.Data;
    using PowerNutrition.Data.Models;
    using PowerNutrition.Services.Core.Interfaces;
    using PowerNutrition.Web.ViewModels.Manage;
    using PowerNutrition.Web.ViewModels.Supplement;
    using PowerNutrition.Data.Models.Enums;

    public class SupplementService : ISupplementService
    {
        private readonly PowerNutritionDbContext dbContext;

        public SupplementService(PowerNutritionDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<SupplementsPageViewModel> GetAllSupplementsAsync(int? categoryFilter)
        {
            SupplementsPageViewModel? supplementsViewmodel = null;

            supplementsViewmodel = new SupplementsPageViewModel()
            {
                Supplements = await this.dbContext
                .Supplements
                .Include(s => s.Category)
                .AsNoTracking()
                .Select(s => new AllSupplementsViewmodel()
                {
                    Id = s.Id.ToString(),
                    Name = s.Name,
                    Brand = s.Brand,
                    ImageUrl = s.ImageUrl,
                    Category = s.Category!.Name,
                    Price = s.Price.ToString("f2"),
                    Quantity = s.Stock.ToString(),
                    Weigth = s.Weight.ToString()
                })
                .ToListAsync()
            };

            if (categoryFilter != null)
            {
                Category? categoryRef = await this.dbContext
                    .Categories
                    .FirstOrDefaultAsync(c => c.Id == categoryFilter);

                if (categoryRef != null)
                {
                    supplementsViewmodel.Supplements = supplementsViewmodel.Supplements.Where(s => s.Category.ToLower() == categoryRef.Name.ToLower());
                    supplementsViewmodel.CategoryName = categoryRef.Name;
                }


            }

            return supplementsViewmodel;

        }
        public async Task<DetailsSupplementViewmodel?> GetDetailsForSupplementAsync(string? id)
        {
            DetailsSupplementViewmodel? supplementDetails = null;

            if (id != null)
            {
                Supplement? supplementToDisplay = await this.dbContext
                .Supplements
                .Include(s => s.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id.ToString().ToLower() == id.ToLower());

                if (supplementToDisplay != null)
                {
                    supplementDetails = new DetailsSupplementViewmodel()
                    {
                        Id = supplementToDisplay.Id.ToString(),
                        Name = supplementToDisplay.Name,
                        Brand = supplementToDisplay.Brand,
                        ImageUrl = supplementToDisplay.ImageUrl,
                        Category = supplementToDisplay.Category!.Name,
                        Price = supplementToDisplay.Price.ToString("f2"),
                        Quantity = supplementToDisplay.Stock.ToString(),
                        Description = supplementToDisplay.Description,
                        Weight = supplementToDisplay.Weight.ToString()
                    };
                }
            }

            return supplementDetails;
        }

        public async Task<SupplementDeleteInputModel?> GetSupplementToDelete(string? supplementId)
        {
            SupplementDeleteInputModel? deleteViewmodel = null;

            bool guidIsValid = Guid.TryParse(supplementId, out Guid parsedGuid);
            if (supplementId != null)
            {
                Supplement? supplementToDelete = await this.dbContext
                    .Supplements
                    .FindAsync(parsedGuid);

                if (supplementToDelete != null)
                {
                    deleteViewmodel = new SupplementDeleteInputModel()
                    {
                        Id = supplementToDelete.Id.ToString(),
                        Name = supplementToDelete.Name,
                    };
                }
            }

            return deleteViewmodel;
        }

        public async Task<Guid?> PersistAddSupplementAsync(AddSupplementInputModel inputModel)
        {
            Guid? newSupplementId = null;

            Category? categoryRef = await this.dbContext
                .Categories
                .FindAsync(inputModel.CategoryId);

            if (categoryRef != null)
            {
                Supplement supplementToAdd = new Supplement()
                {
                    Id = Guid.NewGuid(),
                    Name = inputModel.Name,
                    Description = inputModel.Description,
                    Brand = inputModel.Brand,
                    ImageUrl = inputModel.ImageUrl,
                    Price = inputModel.Price,
                    Stock = inputModel.Stock,
                    Weight = inputModel.Weigth,
                    CategoryId = inputModel.CategoryId
                };

                await this.dbContext.AddAsync(supplementToAdd);
                await this.dbContext.SaveChangesAsync();
                newSupplementId = supplementToAdd.Id;
            }

            return newSupplementId;
        }

        public async Task<SupplementEditInputModel?> GetSupplementForEditAsync(string? supplementId)
        {
            SupplementEditInputModel? supplementToEdit = null;

            if (supplementId != null)
            {
                bool guidIsValid = Guid.TryParse(supplementId, out Guid parsedGuid);

                if (guidIsValid)
                {
                    Supplement? supplement = await this.dbContext
                        .Supplements
                        .FindAsync(parsedGuid);

                    if (supplement != null)
                    {
                        supplementToEdit = new SupplementEditInputModel()
                        {
                            Id = supplement.Id.ToString(),
                            Name = supplement.Name,
                            Description = supplement.Description,
                            Brand = supplement.Brand,
                            Price = supplement.Price,
                            ImageUrl = supplement.ImageUrl,
                            Weigth = supplement.Weight,
                            Stock = supplement.Stock,
                            CategoryId = supplement.CategoryId!.Value
                        };
                    }
                }
            }

            return supplementToEdit;
        }

        public async Task<Guid?> PersistEditSupplementAsync(SupplementEditInputModel inputModel)
        {
            Guid? editedSupplementId = null;

            Category? categoryRef = await this.dbContext
                .Categories
                .FindAsync(inputModel.CategoryId);

            bool guidIsValid = Guid.TryParse(inputModel.Id, out Guid parsedGuid);

            if (categoryRef != null && inputModel != null && guidIsValid)
            {
                Supplement? supplementToEdit = await this.dbContext
                    .Supplements
                    .FindAsync(parsedGuid);

                if (supplementToEdit != null)
                {
                    supplementToEdit.Name = inputModel.Name;
                    supplementToEdit.Description = inputModel.Description;
                    supplementToEdit.Brand = inputModel.Brand;
                    supplementToEdit.ImageUrl = inputModel.ImageUrl;
                    supplementToEdit.Price = inputModel.Price;
                    supplementToEdit.Stock = inputModel.Stock;
                    supplementToEdit.Weight = inputModel.Weigth;
                    supplementToEdit.CategoryId = inputModel.CategoryId;

                    await this.dbContext.SaveChangesAsync();
                    editedSupplementId = supplementToEdit.Id;
                }
            }
            return editedSupplementId;
        }

        public async Task<bool> DeleteSupplement(SupplementDeleteInputModel inputModel)
        {
            bool taskResult = false;

            bool guidIsValid = Guid.TryParse(inputModel.Id, out Guid parsedGuid);

            if (inputModel != null && guidIsValid == true)
            {
                Supplement? supplementToDelete = await this.dbContext
                    .Supplements
                    .FindAsync(parsedGuid);

                if (supplementToDelete != null)
                {
                    supplementToDelete.IsDeleted = true;
                    await this.dbContext.SaveChangesAsync();
                    taskResult = true;
                }
            }
            return taskResult;
        }
        public async Task<IEnumerable<TopSellersViewmodel>> GetTopSellersAsync()
        {
            IEnumerable<TopSellersViewmodel> topSellers = await this.dbContext
                .Supplements
                .Include(s => s.Orders)
                .ThenInclude(oi => oi.Order)
                .Select(s => new TopSellersViewmodel()
                {
                    Id = s.Id.ToString(),
                    Name = s.Name,
                    ImageUrl = s.ImageUrl,
                    SalesCount = s.Orders.Count(o => o.Order.Status == OrderStatus.Approved),
                    Price = s.Price,
                    TotalProfit = s.Price * s.Orders.Count(o => o.Order.Status == OrderStatus.Approved)
                })
                .OrderByDescending(s => s.TotalProfit)
                .ThenByDescending(s => s.Price)
                .Where(s => s.SalesCount > 0)
                .Take(3)
                .ToArrayAsync();

            return topSellers;
        }
    }
}

