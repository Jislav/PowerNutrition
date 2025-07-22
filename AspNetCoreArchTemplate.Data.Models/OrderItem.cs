namespace PowerNutrition.Data.Models
{
    public class OrderItem
    {
        public Guid OrderId { get; set; }

        public Order Order { get; set; } = null!;

        public Guid SupplementId { get; set; }

        public Supplement Supplement { get; set; } = null!;

        public int Quantity { get; set; }
    }
}
