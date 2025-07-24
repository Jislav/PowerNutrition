using Microsoft.AspNetCore.Mvc;
using PowerNutrition.Services.Core.Interfaces;
using PowerNutrition.Web.ViewModels.Order;

namespace PowerNutrition.Web.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            string? userId = this.GetUserId();

            IEnumerable<UserOrderHistoryViewmodel> userOrders = await this.orderService
                .GetUserOrderHistoryAsync(userId);

            return this.View(userOrders);
        }
    }
}
