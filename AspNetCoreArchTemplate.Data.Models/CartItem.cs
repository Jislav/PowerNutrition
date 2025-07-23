namespace PowerNutrition.Data.Models
{
    public class CartItem
    {
        public string UserId { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;

        public Guid SupplementId { get; set; }

        public Supplement Supplement { get; set; } = null!;

        public int Quantity { get; set; }
    }
}
