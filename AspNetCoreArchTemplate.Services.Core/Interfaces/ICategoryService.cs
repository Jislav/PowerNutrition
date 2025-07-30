

using PowerNutrition.Web.ViewModels.Supplement;

namespace PowerNutrition.Services.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<ICollection<SupplementCategoryDropDownFilterViewmodel>> GetAllCategoriesAsync();
    }
}
