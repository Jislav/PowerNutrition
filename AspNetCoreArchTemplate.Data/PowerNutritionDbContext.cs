namespace PowerNutrition.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using PowerNutrition.Data.Models;
    using System.Reflection;

    public class PowerNutritionDbContext : IdentityDbContext
    {
        public PowerNutritionDbContext(DbContextOptions<PowerNutritionDbContext> options)
            : base(options)
        {

        }

        public virtual DbSet<Supplement> Supplements { get; set; } = null!;

        public virtual DbSet<Category> Categories { get; set; } = null!;

        public virtual DbSet<Cart> Carts { get; set; } = null!;

        public virtual DbSet<CartItem> CartsItems { get; set; } = null!;

        public virtual DbSet<Order> Orders { get; set; } = null!;

        public virtual DbSet<OrderItem> OrdersItems { get; set; } = null!;

        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
