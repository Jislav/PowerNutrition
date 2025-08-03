using PowerNutrition.Web.ViewModels.Category;

namespace PowerNutrition.Web.ViewModels.Supplement
{
    public class SupplementsPageViewModel
    {
        public IEnumerable<CategoriesListViewmodel>? Categories { get; set; }

        public IEnumerable<AllSupplementsViewmodel> Supplements { get; set; } = null!;

        public int? CategoryId { get; set; }

        public string? CategoryName { get; set; }
    }
}
