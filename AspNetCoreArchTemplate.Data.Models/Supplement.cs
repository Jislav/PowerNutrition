namespace PowerNutrition.Data.Models
{
    public class Supplement
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Brand { get; set; } = null!;

        public decimal Price { get; set; }

        public string ImageUrl { get; set; } = null!;

        public int Stock { get; set; }

        public double Weight { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; } = null!;

        public bool IsDeleted { get; set; }

        public virtual ICollection<OrderItem> Orders { get; set; }
            = new List<OrderItem>();

    }
}
