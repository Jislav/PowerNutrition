namespace PowerNutrition.GCommon
{
    public static class ErrorMessages
    {
        public static class OrderInputModelMessages
        {
            public const string OrderAddressRequiredMessage = "Address field is required!";
            public const string OrderAddressLengthMessage = "Address must be between 10 and 150 characters long!";

            public const string OrderCityRequiredMessage = "City field is required!";
            public const string OrderCityLengthMessage = "City must be between 3 and 80 characters long!";

            public const string OrderPostCodeRequiredMessage = "Post code field is required!";
            public const string OrderPostCodeLengthMessage = "Post code must be between 4 and 12 characters long!";

            public const string OrderPhoneNumberRequiredMessage = "Phone number field is required!";
            public const string OrderPhoneNumberLengthMessage = "Phone number must be exacly 10 digits long";
        }

        public static class SupplementInputModelMessages
        {
            public const string SupplementNameRequiredMessage = "Name field is required!";
            public const string SupplementNameLengthMessage = "Name field must be between 10 and 100 characters long!";

            public const string SupplementDescriptionRequiredMessage = "Description field is required!";
            public const string SupplementDescriptionLengthMessage = "Description field must be between 10 and 250 characters long!";

            public const string SupplementBrandRequiredMessage = "Brand field is required!";
            public const string SupplementBrandLengthMessage = "Brand field must be between 3 and 50 characters long!";

            public const string SupplementPriceRequiredMessage = "Price field is required!";

            public const string SupplementImageUrlRequiredMessage = "ImageUrl field is required!";

            public const string SupplementStockRequiredMessage = "Stock field is required!";

            public const string SupplementWeigthRequiredMessage = "Weigth field is required!";

            public const string SupplementCategoryRequiredMessage = "Category field is required!";
        }

        public static class CategoryInputModelMessages
        {
            public const string CategoryNameRequiredMessage = "Name field is required!";
            public const string CategoryNameLengthMessage = "Name must be between 3 and 35 characters long!";
        }
    }
}
