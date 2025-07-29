namespace PowerNutrition.Web.ViewModels.Order
{
    public class OrderDetailsViewModel
    {
        public string Address { get; set; } = null!;

        public string City { get; set; } = null!;

        public string PostCode { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public IEnumerable<OrderDetailsItemsListViewmodel> Items { get; set; } = null!;

        public string TotalPrice { get; set; } = null!;
    }
}
