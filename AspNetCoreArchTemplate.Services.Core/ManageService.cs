namespace PowerNutrition.Services.Core
{
    using Microsoft.EntityFrameworkCore;
    using PowerNutrition.Data;
    using PowerNutrition.Services.Core.Interfaces;
    using PowerNutrition.Web.ViewModels.Manage;
    public class ManageService : IManageService
    {
        private readonly PowerNutritionDbContext dbContext;

        public ManageService(PowerNutritionDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<AllSupplementsDeleteViewmodel>> GetAllSupplementsDeleteListAsync()
        {
            IEnumerable<AllSupplementsDeleteViewmodel> supplementsDeleteList = await this.dbContext
               .Supplements
               .Include(s => s.Category)
               .AsNoTracking()
               .Select(s => new AllSupplementsDeleteViewmodel()
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

            return supplementsDeleteList;
        }
    }
}
