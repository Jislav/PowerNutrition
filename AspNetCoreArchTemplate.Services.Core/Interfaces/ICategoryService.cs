

using PowerNutrition.Web.ViewModels.Category;
using PowerNutrition.Web.ViewModels.Supplement;

namespace PowerNutrition.Services.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<ICollection<CategoriesListViewmodel>> GetAllCategoriesAsync();

        Task<bool> AddCategoryAsync(AddCategoryInputModel inputModel);

        Task<CategoryDeleteInputModel?> GetCategoryForDeletingAsync(string? id);

        Task<bool> PersistCategoryDelete(CategoryDeleteInputModel inputModel);

        Task<bool> CheckIfCategoryExists(int? id);
    }
}
