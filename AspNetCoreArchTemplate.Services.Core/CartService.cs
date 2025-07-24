namespace PowerNutrition.Services.Core
{
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

        public async Task<bool> AddToCartAsync(string? userId, string? supplementId)
        {
            if(userId != null && supplementId != null)
            {
                CartItem? cartItem = await this.dbContext
                    .CartsItems
                    .FirstOrDefaultAsync(ci => userId.ToLower() == ci.UserId.ToLower() && ci.SupplementId.ToString().ToLower() == supplementId.ToLower());

                if(cartItem == null)
                {
                    CartItem newCartItem = new CartItem()
                    {
                        UserId = userId,
                        SupplementId = Guid.Parse(supplementId)
                    };

                    await this.dbContext.AddAsync(newCartItem);
                }
                else
                {
                    cartItem.Quantity++;
                }

                Supplement supplementToModify = await this.dbContext
                    .Supplements
                    .FirstOrDefaultAsync(s => s.Id.ToString().ToLower() == supplementId);

                supplementToModify!.Stock--;

                await this.dbContext.SaveChangesAsync();

                return true;
            }

            return false;
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

        public async Task<bool> RemoveFromCartAsync(string? userId, string? supplementId)
        {
            int stockToRenew = 0;

            if(userId != null && supplementId != null)
            {
                CartItem? cartItem = await this.dbContext
                    .CartsItems
                    .FirstOrDefaultAsync(ci => userId.ToLower() == ci.UserId.ToLower() && ci.SupplementId.ToString().ToLower() == supplementId.ToLower());

                if (cartItem != null)
                {
                    stockToRenew = cartItem.Quantity;
                    this.dbContext.Remove(cartItem);

                    Supplement supplementToModify = await this.dbContext
                        .Supplements
                        .FirstOrDefaultAsync(s => s.Id.ToString().ToLower() == supplementId);

                    supplementToModify!.Stock += stockToRenew;

                    await this.dbContext.SaveChangesAsync();

                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
