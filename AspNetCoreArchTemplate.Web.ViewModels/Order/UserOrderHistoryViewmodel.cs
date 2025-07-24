namespace PowerNutrition.Web.ViewModels.Order
{
    public class UserOrderHistoryViewmodel
    {
        public string Id { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string City { get; set; } = null!;

        public string PostCode { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string OrderStatus { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public ICollection<OrderSupplementDetailsViewmodel> Supplements { get; set; } = null!;
    }
}
