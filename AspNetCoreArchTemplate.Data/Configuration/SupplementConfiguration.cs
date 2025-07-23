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

            entity
                .HasData(this.SeedSupplements());

        }
        private ICollection<Supplement> SeedSupplements()
        {
            ICollection<Supplement> supplements = new List<Supplement>()
            {
                 new Supplement
                {
                    Id = Guid.Parse("4696209f-f3f3-47e7-a149-a2611ffc07c7"),
                    Name = "Vitamin D3",
                    Description = "Supports bone health and immune system.",
                    Brand = "NutraWell",
                    Price = 12.99M,
                    ImageUrl = "https://m.media-amazon.com/images/I/51jjjqpi94L._AC_.jpg",
                    Stock = 50,
                    Weight = 0.05,
                    CategoryId = 4,
                    IsDeleted = false
                },
                new Supplement
                {
                    Id = Guid.Parse("ebe5d688-3dc6-4637-86d9-4139bcd1043d"),
                    Name = "Whey Protein",
                    Description = "High-quality whey protein for muscle recovery.",
                    Brand = "PureStrength",
                    Price = 39.99M,
                    ImageUrl = "https://au.atpscience.com/cdn/shop/articles/web04.jpg?v=1702341918&width=2048",
                    Stock = 30,
                    Weight = 2.0,
                    CategoryId = 1,
                    IsDeleted = false
                },
                new Supplement
                {
                    Id = Guid.Parse("d2e21316-1713-4523-b684-2d833716be3d"),
                    Name = "Micronized Creatine",
                    Description = "Micronized creatine monohydrate for faster absorption.",
                    Brand = "MuscleTech",
                    Price = 25.99M,
                    ImageUrl = "https://cdn.shopify.com/s/files/1/0944/0726/files/Creatine-Monohydrate-NCAA-Approved-Supplements-for-College-Students-with-No-Banned-Substances-Safe-for-Test_1800x1800_687a7f60-8727-4d77-a80b-0c522e3e9b1f_480x480.webp?v=1680728035",
                    Stock = 40,
                    Weight = 0.3,
                    CategoryId = 2,
                    IsDeleted = false
                },
                new Supplement
                {
                    Id = Guid.Parse("effaafbc-ffec-4c8b-a1c8-750dd479ba9e"),
                    Name = "L-Glutamine Powder",
                    Description = "Supports muscle recovery and immune health.",
                    Brand = "Optimum Nutrition",
                    Price = 21.50M,
                    ImageUrl = "https://m.media-amazon.com/images/I/81WBLQck12L._UF1000,1000_QL80_.jpg",
                    Stock = 35,
                    Weight = 0.25,
                    CategoryId = 3,
                    IsDeleted = false
                },
                new Supplement
                {
                    Id = Guid.Parse("42b37657-90fe-4f7d-baed-5ae26d88dbe6"),
                    Name = "Multivitamin Complex",
                    Description = "Daily multivitamin with essential nutrients.",
                    Brand = "HealthCore",
                    Price = 19.99M,
                    ImageUrl = "https://2.bgbong.xyz/newdir/article-img8/cvs-mens-gummy-vitamins-a-comprehensive-guide-to-supporting-mens-health-mfuyiqfz77xnr.png",
                    Stock = 100,
                    Weight = 0.1,
                    CategoryId = 4,
                    IsDeleted = false
                }
            };

            return supplements;
        }
    }
}
