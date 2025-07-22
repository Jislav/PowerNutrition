using Microsoft.AspNetCore.Identity;

namespace PowerNutrition.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool VipStatus { get; set; }

        public int PowerPoints { get; set; }

        public virtual Cart Cart { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; } 
            = new HashSet<Order>();
    }
}
