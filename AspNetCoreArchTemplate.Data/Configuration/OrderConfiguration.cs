namespace PowerNutrition.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using PowerNutrition.Data.Models;
    using static PowerNutrition.GCommon.ApplicationConstants.OrderConstants;
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> entity)
        {
            entity
                .HasKey(o => o.Id);

            entity
                .Property(o => o.Address)
                .HasMaxLength(OrderAddressMaxLength)
                .IsRequired();

            entity
                .Property(o => o.City)
                .HasMaxLength(OrderCityMaxLength)
                .IsRequired();

            entity
                .Property(o => o.PostCode)
                .HasMaxLength(OrderPostCodeMinLength)
                .IsRequired();

            entity
                .Property(o => o.Status)
                .IsRequired();

            entity
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);

            entity
                .Property(o => o.TotalPrice)
                .HasPrecision(18, 4);
        }
    }
}
