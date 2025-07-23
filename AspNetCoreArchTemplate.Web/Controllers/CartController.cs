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

        public async Task<IActionResult> Index()
        {
            var userId = this.GetUserId();
            IEnumerable<AllCartItemsViewmodel?> cartItems = await this.cartService
                .GetAllCartItemsAsync(userId);

            return View(cartItems);
        }
    }
}
