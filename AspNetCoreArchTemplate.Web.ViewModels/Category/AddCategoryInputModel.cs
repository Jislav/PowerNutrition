namespace PowerNutrition.Web.ViewModels.Category
{
    using System.ComponentModel.DataAnnotations;
    using static PowerNutrition.GCommon.ApplicationConstants.CategoryConstants;
    using static PowerNutrition.GCommon.ErrorMessages.CategoryInputModelMessages;
    public class AddCategoryInputModel
    {
        [MinLength(CategoryNameMinLength, ErrorMessage = CategoryNameLengthMessage)]
        [MaxLength(CategoryNameMaxLength, ErrorMessage = CategoryNameLengthMessage)]
        [Required(ErrorMessage = CategoryNameRequiredMessage)]
        public string Name { get; set; } = null!;
    }
}
