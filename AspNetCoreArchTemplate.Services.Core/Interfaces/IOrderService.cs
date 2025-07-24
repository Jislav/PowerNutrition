using PowerNutrition.Web.ViewModels.Order;

namespace PowerNutrition.Services.Core.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<UserOrderHistoryViewmodel>> GetUserOrderHistoryAsync(string? userId);
    }
}
