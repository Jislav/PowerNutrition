namespace PowerNutrition.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using PowerNutrition.Data.Models;
    using static PowerNutrition.GCommon.ApplicationConstants.SupplementConstants;
    public class SupplementConfiguration : IEntityTypeConfiguration<Supplement>
    {
        public void Configure(EntityTypeBuilder<Supplement> entity)
        {
            entity
                .HasKey(s => s.Id);

            entity
                .Property(s => s.Name)
                .HasMaxLength(SupplementNameMaxLength)
                .IsRequired();

            entity
                .Property(s => s.Description)
                .HasMaxLength(SupplementDescriptionMaxLength)
                .IsRequired();

            entity
                .Property(s => s.Brand)
                .HasMaxLength(SupplementBrandMaxLength)
                .IsRequired();

            entity
                .Property(s => s.ImageUrl)
                .IsRequired();

            entity
                .Property(s => s.Price)
                .HasPrecision(18, 4);


            entity
                .HasOne(s => s.Category)
                .WithMany(c => c.Supplements)
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .Property(s => s.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasQueryFilter(s => s.IsDeleted == false);
        }
    }
}
