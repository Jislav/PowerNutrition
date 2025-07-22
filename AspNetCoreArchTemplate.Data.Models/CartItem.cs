namespace PowerNutrition.Data.Models
{
    public class CartItem
    {
        public Guid CartId { get; set; }

        public Cart Cart { get; set; } = null!;

        public Guid SupplementId { get; set; }

        public Supplement Supplement { get; set; } = null!;

        public int Quantity { get; set; }
    }
}
