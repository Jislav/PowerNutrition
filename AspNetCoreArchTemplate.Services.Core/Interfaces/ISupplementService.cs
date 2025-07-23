namespace PowerNutrition.Services.Core.Interfaces
{
using PowerNutrition.Web.ViewModels.Supplement;
    public interface ISupplementService
    {
        Task<IEnumerable<AllSupplementsViewmodel>> GetAllSupplementsAsync();

        Task<DetailsSupplementViewmodel?> GetDetailsForSupplementAsync(string? id);
    }
}
