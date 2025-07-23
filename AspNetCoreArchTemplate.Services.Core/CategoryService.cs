namespace PowerNutrition.Services.Core
{
    using Microsoft.EntityFrameworkCore;
    using PowerNutrition.Data;
    using PowerNutrition.Services.Core.Interfaces;
    using PowerNutrition.Web.ViewModels.Supplement;
    public class CategoryService : ICategoryService
    {
        private readonly PowerNutritionDbContext dbContext;
        public CategoryService(PowerNutritionDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<SupplementCategoryDropDownFilterViewmodel>> GetAllCategoriesAsync()
        {
            IEnumerable<SupplementCategoryDropDownFilterViewmodel> categories = await this.dbContext
                .Categories
                .Select(c => new SupplementCategoryDropDownFilterViewmodel
                {
                    Id = c.Id.ToString(),
                    CategoryName = c.Name
                })
                .ToListAsync();

            return categories;

        }
    }
}
