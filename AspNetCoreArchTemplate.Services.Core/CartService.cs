namespace PowerNutrition.Services.Core
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using PowerNutrition.Data;
    using PowerNutrition.Data.Models;
    using PowerNutrition.Services.Core.Interfaces;
    using PowerNutrition.Web.ViewModels.Cart;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class CartService : ICartService
    {
        private readonly PowerNutritionDbContext dbContext;

        public CartService(PowerNutritionDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<AllCartItemsViewmodel?>> GetAllCartItemsAsync(string? userId)
        {
            IEnumerable<AllCartItemsViewmodel?> userCartItems = null;

            if (userId != null)
            {
                userCartItems = await this.dbContext
                    .CartsItems
                    .Include(ci => ci.Supplement)
                    .Where(ci => ci.UserId.ToLower() == userId.ToLower())
                    .Select(ci => new AllCartItemsViewmodel()
                    {
                        Id = ci.SupplementId.ToString(),
                        Name = ci.Supplement.Name,
                        Price = (ci.Supplement.Price * ci.Quantity).ToString("f2"),
                        Amount = ci.Quantity.ToString(),
                        ImageUrl = ci.Supplement.ImageUrl,
                    })
                    .ToArrayAsync();
            }

            return userCartItems;
        }
    }
}
