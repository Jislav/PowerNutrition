﻿namespace PowerNutrition.Data.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public virtual ICollection<Supplement> Supplements { get; set; }
            = new HashSet<Supplement>();
    }
}
