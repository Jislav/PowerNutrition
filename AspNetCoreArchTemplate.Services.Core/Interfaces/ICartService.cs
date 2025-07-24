using PowerNutrition.Web.ViewModels.Cart;

namespace PowerNutrition.Services.Core.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<AllCartItemsViewmodel?>> GetAllCartItemsAsync(string? userId);

        Task<bool> AddToCartAsync(string? userId, string? supplementId);

        Task<bool> RemoveFromCartAsync(string? userId, string? supplementId);
    }
}
