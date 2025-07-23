

using PowerNutrition.Web.ViewModels.Supplement;

namespace PowerNutrition.Services.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<SupplementCategoryDropDownFilterViewmodel>> GetAllCategoriesAsync();
    }
}
