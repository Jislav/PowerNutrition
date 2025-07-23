namespace PowerNutrition.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using PowerNutrition.Data.Models;
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> entity)
        {
            entity
                .HasKey(ci => new { ci.UserId, ci.SupplementId });

            entity
                .HasOne(ci => ci.User)
                .WithMany(u => u.CartItems)
                .HasForeignKey(ci => ci.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(ci => ci.Supplement)
                .WithMany()
                .HasForeignKey(ci => ci.SupplementId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .Property(ci => ci.Quantity)
                .IsRequired()
                .HasDefaultValue(1);

            entity
                .HasQueryFilter(ci => ci.Supplement.IsDeleted == false);
                
        }
    }
}
