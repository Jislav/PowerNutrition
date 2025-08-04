namespace PowerNutrition.Web.ViewModels.Manage
{
    public class TopSellersViewmodel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        public int SalesCount { get; set; }

        public decimal Price { get; set; }

        public decimal TotalProfit { get; set; }
    }
}
