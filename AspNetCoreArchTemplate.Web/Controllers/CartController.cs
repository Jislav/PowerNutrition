namespace PowerNutrition.Web.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PowerNutrition.Data.Models;
    using PowerNutrition.Services.Core.Interfaces;
    using PowerNutrition.Web.ViewModels.Cart;
    using System.Threading.Tasks;

    public class CartController : BaseController
    {
        private readonly ICartService cartService;

        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> MyCart()
        {
            string? userId = this.GetUserId();

            IEnumerable<AllCartItemsViewmodel>? cartItems = await this.cartService
                .GetAllCartItemsAsync(userId);

            return this.View(cartItems);
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(string? supplementId)
        {
            string? userId = this.GetUserId();

            bool taskResult = await this.cartService
                .AddToCartAsync(userId, supplementId);

            if(taskResult == false)
            {
                //TODO: Redirect to custom error page
            }

            return this.RedirectToAction("Index", "Supplement");
        }
        [HttpPost]

        public async Task<IActionResult> RemoveFromCart(string? supplementId)
        {
            string? userId = this.GetUserId();

            bool taskResult = await this.cartService
                .RemoveFromCartAsync(userId, supplementId);

            if(taskResult == false)
            {
                //TODO: Redirect to custom error page
            }

            return this.Redirect(nameof(MyCart));
        }
    }
}
