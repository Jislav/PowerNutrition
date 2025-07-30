using System.ComponentModel.DataAnnotations;

namespace PowerNutrition.Web.ViewModels.Supplement
{
    public class SupplementDeleteInputModel
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;
    }
}
