namespace PowerNutrition.Web.ViewModels.Supplement
{
    using PowerNutrition.Web.ViewModels.Category;
    using System.ComponentModel.DataAnnotations;
    using static PowerNutrition.GCommon.ApplicationConstants.SupplementConstants;
    using static PowerNutrition.GCommon.ErrorMessages.SupplementInputModelMessages;
    public class AddSupplementInputModel
    {
        [MinLength(SupplementNameMinxLength, ErrorMessage = SupplementNameLengthMessage)]
        [MaxLength(SupplementNameMaxLength, ErrorMessage = SupplementNameLengthMessage)]
        [Required(ErrorMessage = SupplementNameRequiredMessage)]
        public string Name { get; set; } = null!;

        [MinLength(SupplementDescriptionMinLength, ErrorMessage = SupplementDescriptionLengthMessage)]
        [MaxLength(SupplementDescriptionMaxLength, ErrorMessage = SupplementDescriptionLengthMessage)]
        [Required(ErrorMessage = SupplementDescriptionRequiredMessage)]
        public string Description { get; set; } = null!;

        [MinLength(SupplementBrandMinLength, ErrorMessage = SupplementBrandLengthMessage)]
        [MaxLength(SupplementBrandMaxLength, ErrorMessage = SupplementBrandLengthMessage)]
        [Required(ErrorMessage = SupplementBrandRequiredMessage)]
        public string Brand { get; set; } = null!;

        [Required(ErrorMessage = SupplementPriceRequiredMessage)]
        [Range(1.00, double.MaxValue, ErrorMessage = "Price must be a more than 1.00")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = SupplementImageUrlRequiredMessage)]
        public string ImageUrl { get; set; } = null!;

        [Required(ErrorMessage = SupplementStockRequiredMessage)]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be a positive number")]
        public int Stock { get; set; }

        [Required(ErrorMessage = SupplementWeigthRequiredMessage)]
        [Range(0.01, double.MaxValue, ErrorMessage = "Weight must be a positive number")]
        public double Weigth { get; set; }

        [Required(ErrorMessage = SupplementCategoryRequiredMessage)]
        public int CategoryId { get; set; }

        public ICollection<CategoriesListViewmodel>? Categories { get; set; }
    }
}
