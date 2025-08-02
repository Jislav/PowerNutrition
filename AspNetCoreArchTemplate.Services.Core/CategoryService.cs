namespace PowerNutrition.Services.Core
{
    using Microsoft.EntityFrameworkCore;
    using PowerNutrition.Data;
    using PowerNutrition.Data.Models;
    using PowerNutrition.Services.Core.Interfaces;
    using PowerNutrition.Web.ViewModels.Category;
    public class CategoryService : ICategoryService
    {
        private readonly PowerNutritionDbContext dbContext;

        public CategoryService(PowerNutritionDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<ICollection<CategoriesListViewmodel>> GetAllCategoriesAsync()
        {
            ICollection<CategoriesListViewmodel> categories = await this.dbContext
                .Categories
                .Select(c => new CategoriesListViewmodel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();

            return categories;

        }

        public async Task<bool> AddCategoryAsync(AddCategoryInputModel inputModel)
        {
            bool taskResult = false;

            if(inputModel != null)
            {
                Category? categoryExists = await this.dbContext
                    .Categories
                    .FirstOrDefaultAsync(c => c.Name.ToLower() == inputModel.Name.ToLower());

                if(categoryExists == null)
                {
                    Category categoryToAdd = new Category()
                    {
                        Name = inputModel.Name,
                    };

                    await this.dbContext.Categories.AddAsync(categoryToAdd);
                    await this.dbContext.SaveChangesAsync();
                    taskResult = true;
                }
            }
            return taskResult;
        }

        public async Task<CategoryDeleteInputModel?> GetCategoryForDeletingAsync(string? id)
        {
            CategoryDeleteInputModel? inputModel = null;

            if(id != null)
            {
                bool idIsValid = int.TryParse(id, out int categoryId);

                if (idIsValid)
                {
                    Category? categoryToDelete = await this.dbContext
                        .Categories
                        .FirstOrDefaultAsync(c => c.Id == categoryId);

                    if(categoryToDelete != null)
                    {
                        inputModel = new CategoryDeleteInputModel()
                        {
                            Id = categoryToDelete.Id,
                            Name = categoryToDelete.Name
                        };
                    }
                }
            }

            return inputModel;
        }

        public async Task<bool> PersistCategoryDelete(CategoryDeleteInputModel inputModel)
        {
            bool taskResult = false;

            if(inputModel != null)
            {
                Category? categoryToDelete = await this.dbContext
                    .Categories
                    .Include(c => c.Supplements)
                    .FirstOrDefaultAsync(c => c.Id == inputModel.Id);

                if(categoryToDelete != null)
                {
                    foreach(Supplement supplement in categoryToDelete.Supplements)
                    {
                        supplement.IsDeleted = true;
                    }
                }

                this.dbContext.Categories.Remove(categoryToDelete!);
                await this.dbContext .SaveChangesAsync();
                taskResult = true;
            }

            return taskResult;
        }
    }
}
