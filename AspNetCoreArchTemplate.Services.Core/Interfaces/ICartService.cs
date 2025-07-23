using PowerNutrition.Web.ViewModels.Cart;

namespace PowerNutrition.Services.Core.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<AllCartItemsViewmodel?>> GetAllCartItemsAsync(string? userId);
    }
}
