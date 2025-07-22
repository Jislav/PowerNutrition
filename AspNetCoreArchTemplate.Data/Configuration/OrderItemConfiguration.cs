namespace PowerNutrition.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using PowerNutrition.Data.Models;
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> entity)
        {
            entity
                .HasKey(oi => new { oi.OrderId, oi.SupplementId });

            entity
                .HasOne(oi => oi.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(oi => oi.OrderId);

            entity
                .HasOne(oi => oi.Supplement)
                .WithMany(s => s.Orders)
                .HasForeignKey(oi => oi.SupplementId);

            entity
                .HasQueryFilter(oi => oi.Supplement.IsDeleted == false);
        }
    }
}
