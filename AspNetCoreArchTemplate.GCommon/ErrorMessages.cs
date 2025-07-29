namespace PowerNutrition.GCommon
{
    public static class ErrorMessages
    {
        public static class OrderInputModelMessages
        {
            public const string OrderAddressRequiredMessage = "Address field is required!";
            public const string OrderAddressLengthMessage = $"Address must be between 10 and 150 characters long!";

            public const string OrderCityRequiredMessage = "City field is required!";
            public const string OrderCityLengthMessage = "City must be between 3 and 80 characters long!";

            public const string OrderPostCodeRequiredMessage = "Post code field is required!";
            public const string OrderPostCodeLengthMessage = "Post code must be between 4 and 12 characters long!";

            public const string OrderPhoneNumberRequiredMessage = "Phone number field is required!";
            public const string OrderPhoneNumberLengthMessage = "Phone number must be exacly 10 digits long";
        }
    }
}
