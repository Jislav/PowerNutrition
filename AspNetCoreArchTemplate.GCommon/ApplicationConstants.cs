namespace PowerNutrition.GCommon
{
    public class ApplicationConstants
    {
        public static class SupplementConstants
        {
            public const int SupplementNameMinxLength = 10;
            public const int SupplementNameMaxLength = 100;

            public const int SupplementDescriptionMinLength = 10;
            public const int SupplementDescriptionMaxLength = 250;

            public const int SupplementBrandMinLength = 3;
            public const int SupplementBrandMaxLength = 50;

            public const double SupplementWeigthMinValue = 0.25;
            public const double SupplementWeigthMaxValue = 10;

        }

        public static class CategoryConstants
        {
            public const int CategoryNameMinLength = 3;
            public const int CategoryNameMaxLength = 35;
        }

        public static class OrderConstants
        {
            public const int OrderAddressMinLength = 10;
            public const int OrderAddressMaxLength = 150;

            public const int OrderCityMinLength = 3;
            public const int OrderCityMaxLength = 80;

            public const int OrderPostCodeMinLength = 4;
            public const int OrderPostCodeMaxLength = 12;

        }
    }
}
