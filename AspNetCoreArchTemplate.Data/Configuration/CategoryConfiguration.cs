namespace PowerNutrition.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using PowerNutrition.Data.Models;
    using static PowerNutrition.GCommon.ApplicationConstants.CategoryConstants;
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> entity)
        {
            entity
                .HasKey(c => c.Id);

            entity
                .Property(c => c.Name)
                .HasMaxLength(CategoryNameMaxLength)
                .IsRequired();
            
            entity
                .HasData(this.SeedCategories());
        }

        private ICollection<Category> SeedCategories()
        {
            ICollection<Category> categories = new List<Category>()
            {
                 new Category { Id = 1, Name = "Protein" },
                 new Category { Id = 2, Name = "Creatine" },
                 new Category { Id = 3, Name = "Glutamine" },
                 new Category { Id = 4, Name = "Vitamins" },
            };

            return categories;
        }
    }
}
