namespace PowerNutrition.Data.Models
{
    using PowerNutrition.Data.Models.Enums;
    public class Order
    {
        public Guid Id { get; set; }

        public string Address { get; set; } = null!;

        public string City { get; set; } = null!;

        public string PostCode { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public decimal TotalPrice { get; set; }

        public OrderStatus Status { get; set; }

        public string UserId { get; set; } = null!;

        public virtual ApplicationUser User { get; set; } = null!;

        public ICollection<OrderItem> Items { get; set; }
            = new HashSet<OrderItem>();

    }
}
