namespace PowerNutrition.Services.Core
{
    using Microsoft.EntityFrameworkCore;
    using PowerNutrition.Data;
    using PowerNutrition.Data.Models;
    using PowerNutrition.Services.Core.Interfaces;
    using PowerNutrition.Web.ViewModels.Supplement;

    public class SupplementService : ISupplementService
    {
        private readonly PowerNutritionDbContext dbContext;

        public SupplementService(PowerNutritionDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<AllSupplementsViewmodel>> GetAllSupplementsAsync()
        {
            IEnumerable<AllSupplementsViewmodel> supplements = await this.dbContext
                .Supplements
                .Include(s => s.Category)
                .AsNoTracking()
                .Select(s => new AllSupplementsViewmodel()
                {
                    Id = s.Id.ToString(),
                    Name = s.Name,
                    Brand = s.Brand,
                    ImageUrl = s.ImageUrl,
                    Category = s.Category.Name,
                    Price = s.Price.ToString("f2"),
                    Quantity = s.Stock.ToString(),
                })
                .ToListAsync();

            return supplements;

        }

        public async Task<DetailsSupplementViewmodel?> GetDetailsForSupplementAsync(string? id)
        {
            DetailsSupplementViewmodel? supplementDetails = null;

            if(id != null)
            {
                Supplement? supplementToDisplay = await this.dbContext
                .Supplements
                .Include(s => s.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id.ToString().ToLower() == id.ToLower());

                if(supplementToDisplay != null)
                {
                    supplementDetails = new DetailsSupplementViewmodel()
                    {
                        Id = supplementToDisplay.Id.ToString(),
                        Name = supplementToDisplay.Name,
                        Brand = supplementToDisplay.Brand,
                        ImageUrl = supplementToDisplay.ImageUrl,
                        Category = supplementToDisplay.Category.Name,
                        Price = supplementToDisplay.Price.ToString("f2"),
                        Quantity = supplementToDisplay.Stock.ToString(),
                        Description = supplementToDisplay.Description
                    };
                }
            }

            return supplementDetails;
        }
    }
}
