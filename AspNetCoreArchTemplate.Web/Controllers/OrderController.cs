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

        public async Task<IActionResult> MyOrders()
        {
            string? userId = this.GetUserId();

            IEnumerable<UserOrderHistoryViewmodel> userOrders = await this.orderService
                .GetUserOrderHistoryAsync(userId);

            return this.View(userOrders);
        }
        [HttpPost]
        public async Task<IActionResult> Purchase(OrderInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return View("Checkout", input);
            }

            string userId = this.GetUserId()!;

            bool taskResult = await this.orderService
                .PlaceOrderAsync(userId, input);

            //TODO: Handle error with custom error page

            return this.RedirectToAction(nameof(Index));
        }
    }
}
