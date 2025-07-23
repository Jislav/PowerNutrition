namespace PowerNutrition.Web.ViewModels.Supplement
{
    public class AllSupplementsViewmodel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Brand { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        public string Category { get; set; } = null!;

        public IEnumerable<SupplementCategoryDropDownFilterViewmodel> CategoryDropDownFilter { get; set; } = null!;

        public string Price { get; set; } = null!;

        public string Quantity { get; set; } = null!;
    }
}
