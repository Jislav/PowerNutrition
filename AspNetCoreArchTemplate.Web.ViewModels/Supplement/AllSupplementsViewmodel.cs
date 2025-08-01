﻿using PowerNutrition.Web.ViewModels.Category;

namespace PowerNutrition.Web.ViewModels.Supplement
{
    public class AllSupplementsViewmodel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Brand { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        public string Category { get; set; } = null!;

        public IEnumerable<CategoriesListViewmodel> CategoryDropDownFilter { get; set; } = null!;

        public string Price { get; set; } = null!;

        public string Quantity { get; set; } = null!;

        public string Weigth { get; set; } = null!;
    }
}
