using Microsoft.AspNetCore.Identity;

namespace PowerNutrition.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool VipStatus { get; set; }

        public int PowerPoints { get; set; }

        public virtual ICollection<Order> Orders { get; set; } 
            = new HashSet<Order>();

        public virtual ICollection<CartItem> CartItems { get; set; }
            = new HashSet<CartItem>();
    }
}
