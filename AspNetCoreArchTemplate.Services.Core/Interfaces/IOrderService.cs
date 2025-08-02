using PowerNutrition.Web.ViewModels.Order;

namespace PowerNutrition.Services.Core.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<UserOrderHistoryViewmodel>?> GetUserOrderHistoryAsync(string? userId);

        Task<bool> PlaceOrderAsync(string? userId, OrderInputModel input);

        Task<OrderDetailsViewModel?> GetOrderDetailsAsync(string? userId, string orderId);

        Task<IEnumerable<OrdersWithStatusPendingViewmodel>?> GetAllOrdersWithStatusPendingAsync();

    }
}
