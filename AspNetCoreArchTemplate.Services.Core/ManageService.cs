namespace PowerNutrition.Services.Core
{
    using Microsoft.EntityFrameworkCore;
    using PowerNutrition.Data;
    using PowerNutrition.Data.Models;
    using PowerNutrition.Services.Core.Interfaces;
    using PowerNutrition.Web.ViewModels.Manage;
    using PowerNutrition.Data.Models.Enums;
    using System;

    public class ManageService : IManageService
    {
        private readonly PowerNutritionDbContext dbContext;

        public ManageService(PowerNutritionDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Guid?> ApproveOrderAsync(string? orderId)
        {
            Guid? approvedOrderId = null!;

            if (orderId != null)
            {
                bool guidIsValid = Guid.TryParse(orderId, out Guid parsedGuid);

                Order? order = await this.dbContext
                    .Orders
                    .FindAsync(parsedGuid);

                if (order != null)
                {
                    order.Status = OrderStatus.Approved;
                    approvedOrderId = order.Id;
                    await this.dbContext.SaveChangesAsync();
                }
            }

            return approvedOrderId;
        }

        public async Task<IEnumerable<AllSupplemenetsEditViewmodel>> GetAllSupplementsEditListAsync()
        {

            IEnumerable<AllSupplemenetsEditViewmodel> supplementsDeleteList = await this.dbContext
               .Supplements
               .Include(s => s.Category)
               .AsNoTracking()
               .Select(s => new AllSupplemenetsEditViewmodel()
               {
                   Id = s.Id.ToString(),
                   Name = s.Name,
                   Brand = s.Brand,
                   ImageUrl = s.ImageUrl,
                   Category = s.Category!.Name,
                   Price = s.Price.ToString("f2"),
                   Quantity = s.Stock.ToString(),
               })
               .ToListAsync();

            return supplementsDeleteList;
        }

        public async Task<IEnumerable<AllSupplementsDeleteViewmodel>> GetAllSupplementsDeleteListAsync()
        {
            IEnumerable<AllSupplementsDeleteViewmodel> supplementsDeleteList = await this.dbContext
               .Supplements
               .Include(s => s.Category)
               .AsNoTracking()
               .Select(s => new AllSupplementsDeleteViewmodel()
               {
                   Id = s.Id.ToString(),
                   Name = s.Name,
                   Brand = s.Brand,
                   ImageUrl = s.ImageUrl,
                   Category = s.Category!.Name,
                   Price = s.Price.ToString("f2"),
                   Quantity = s.Stock.ToString(),
               })
               .ToListAsync();

            return supplementsDeleteList;
        }
    }
}
