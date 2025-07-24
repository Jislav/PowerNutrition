namespace PowerNutrition.Services.Core.Interfaces
{
    using Microsoft.EntityFrameworkCore;
    using PowerNutrition.Data;
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
    }
}
