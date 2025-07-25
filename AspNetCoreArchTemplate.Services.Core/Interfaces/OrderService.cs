namespace PowerNutrition.Services.Core.Interfaces
{
    using Microsoft.EntityFrameworkCore;
    using PowerNutrition.Data;
    using PowerNutrition.Data.Models;
    using PowerNutrition.Web.ViewModels.Order;
    public class OrderService : IOrderService
    {
        private readonly PowerNutritionDbContext dbContext;

        public OrderService(PowerNutritionDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<UserOrderHistoryViewmodel>> GetUserOrderHistoryAsync(string? userId)
        {
            IEnumerable<UserOrderHistoryViewmodel>? currentUserOrders = null;

            if (userId != null)
            {
                currentUserOrders = await this.dbContext
                .Orders
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Supplement)
                .AsNoTracking()
                .Where(o => o.UserId.ToLower() == userId.ToLower())
                .Select(o => new UserOrderHistoryViewmodel
                {
                    Id = o.Id.ToString(),
                    Address = o.Address,
                    City = o.City,
                    PostCode = o.PostCode,
                    PhoneNumber = o.PhoneNumber,
                    OrderStatus = o.Status.ToString(),
                    UserId = o.UserId,
                    Supplements = o.Items
                                    .Select(i => new OrderSupplementDetailsViewmodel
                                    {
                                        Name = i.Supplement.Name,
                                        Brand = i.Supplement.Brand,
                                        TotalPrice = (i.Quantity * i.Supplement.Price).ToString()
                                    })
                                    .ToArray()
                })
                .ToArrayAsync();
            }
            return currentUserOrders;
        }

        public async Task<bool> PlaceOrderAsync(string? userId, OrderInputModel input)
        {
            if(userId != null)
            {
                List<CartItem> userCartItems = await this.dbContext
                .CartsItems
                .Include(c => c.Supplement)
                .Where(c => c.UserId == userId)
                .ToListAsync();

                if (userCartItems.Count == 0)
                {
                    return false;
                }

                decimal totalPrice = userCartItems.Sum(ci => ci.Supplement.Price * ci.Quantity);

                Order orderToPlace = new Order
                {
                    Address = input.Address,
                    City = input.City,
                    PostCode = input.PostCode,
                    PhoneNumber = input.PhoneNumber,
                    TotalPrice = totalPrice,
                    UserId = userId!,
                    Items = userCartItems.Select(ci => new OrderItem
                    {
                        SupplementId = ci.SupplementId,
                        Quantity = ci.Quantity,
                    })
                    .ToList()
                };

                await this.dbContext.Orders.AddAsync(orderToPlace);
                this.dbContext.CartsItems.RemoveRange(userCartItems);
                await this.dbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }
    }
}
