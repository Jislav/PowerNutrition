namespace PowerNutrition.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PowerNutrition.Services.Core.Interfaces;
    using PowerNutrition.Web.ViewModels.Manage;
    using PowerNutrition.Web.ViewModels.Order;
    using PowerNutrition.Web.ViewModels.Supplement;

    public class ManageController : BaseController
    {
        private readonly IManageService manageService;
        private readonly IOrderService orderService;

        public ManageController(IManageService manageService, IOrderService orderService)
        {
            this.manageService = manageService;
            this.orderService = orderService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Delete()
        {
            IEnumerable<AllSupplementsDeleteViewmodel> supplementsDeleteViewmodel = await this.manageService
                .GetAllSupplementsDeleteListAsync();

            return this.View(supplementsDeleteViewmodel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            IEnumerable<AllSupplemenetsEditViewmodel> supplementsEditViewmodel = await this.manageService
                .GetAllSupplementsEditListAsync();

            return this.View(supplementsEditViewmodel);
        }
        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            IEnumerable<OrdersWithStatusPendingViewmodel>? pendingOrders = await this.orderService
                .GetAllOrdersWithStatusPendingAsync();

            return this.View(pendingOrders);
        }
        [HttpPost]
        public async Task<IActionResult> ApproveOrder(string? orderId)
        {
            if (orderId != null)
            {
                Guid? orderGuid = await this.manageService
                    .ApproveOrderAsync(orderId);

                if (orderGuid != null)
                {
                    return this.RedirectToAction("Details", "Order", new { orderId = orderGuid });
                }
            }

            return this.RedirectToAction(nameof(Orders));
        }
    }
}
