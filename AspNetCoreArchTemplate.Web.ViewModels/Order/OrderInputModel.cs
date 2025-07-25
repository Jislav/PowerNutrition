namespace PowerNutrition.Web.ViewModels.Order
{
    using PowerNutrition.Data.Models;
    using System.ComponentModel.DataAnnotations;
    using static PowerNutrition.GCommon.ApplicationConstants.OrderConstants;
    public class OrderInputModel
    {
        [MinLength(OrderAddressMinLength)]
        [MaxLength(OrderAddressMaxLength)]
        [Required]
        public string Address { get; set; } = null!;

        [MinLength(OrderCityMinLength)]
        [MaxLength(OrderCityMaxLength)]
        [Required]
        public string City { get; set; } = null!;

        [MinLength(OrderPostCodeMinLength)]
        [MaxLength(OrderPostCodeMaxLength)]
        [Required]
        public string PostCode { get; set; } = null!;

        [RegularExpression(OrderPhoneNumberPattern)]
        [Required]
        public string PhoneNumber { get; set; } = null!;

    }
}
