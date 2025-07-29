namespace PowerNutrition.Web.ViewModels.Order
{
    using static PowerNutrition.GCommon.ErrorMessages.OrderInputModelMessages;
    using System.ComponentModel.DataAnnotations;
    using static PowerNutrition.GCommon.ApplicationConstants.OrderConstants;
    public class OrderInputModel
    {
        [MinLength(OrderAddressMinLength, ErrorMessage = OrderAddressLengthMessage)]
        [MaxLength(OrderAddressMaxLength, ErrorMessage = OrderAddressLengthMessage)]
        [Required(ErrorMessage = OrderAddressRequiredMessage)]
        public string Address { get; set; } = null!;

        [MinLength(OrderCityMinLength, ErrorMessage = OrderCityLengthMessage)]
        [MaxLength(OrderCityMaxLength, ErrorMessage = OrderCityLengthMessage)]
        [Required(ErrorMessage = OrderCityRequiredMessage)]
        public string City { get; set; } = null!;

        [MinLength(OrderPostCodeMinLength, ErrorMessage = OrderPostCodeLengthMessage)]
        [MaxLength(OrderPostCodeMaxLength, ErrorMessage = OrderPostCodeLengthMessage)]
        [Required(ErrorMessage = OrderPostCodeRequiredMessage)]
        public string PostCode { get; set; } = null!;

        [RegularExpression(OrderPhoneNumberPattern, ErrorMessage = OrderPhoneNumberLengthMessage)]
        [Required(ErrorMessage = OrderPhoneNumberRequiredMessage)]
        public string PhoneNumber { get; set; } = null!;

    }
}
