using Microsoft.AspNetCore.Identity;
using PowerNutrition.Data.Seeding.Interfaces;

namespace PowerNutrition.Data.Seeding
{
    public class IdentitySeeder : IIdentitySeeder
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;

        private readonly string[] roles = { "User" , "Manager" };

        public IdentitySeeder(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task SeedRolesAndDefaultManager()
        {
            await this.SeedRolesAsync();
            await this.SeedDefaultManagerAndUserAsync();
        }
        
        private async Task SeedRolesAsync()
        {
            foreach(string role in roles)
            {
                bool roleExists = await this.roleManager
                    .RoleExistsAsync(role);

                if(!roleExists)
                {
                    IdentityRole newRole = new IdentityRole(role);

                    if(newRole != null)
                    {
                        await this.roleManager
                            .CreateAsync(newRole);
                    }
                }
            }
        }

        private async Task SeedDefaultManagerAndUserAsync()
        {
            string managerEmail = "manager@powernutrition.com";
            string managerPassword = "123456Aa-";

            IdentityUser? managerSeeded = await this.userManager.FindByEmailAsync(managerEmail);

            if (managerSeeded == null)
            {
                IdentityUser manager = new IdentityUser()
                {
                    UserName = managerEmail,
                    Email = managerEmail
                };

                await this.userManager.CreateAsync(manager, managerPassword);
                await this.userManager.AddToRoleAsync(manager, "Manager");
            }


            string defaultUserEmail = "defaultuser@abv.com";
            string defaultUserPassword = "123456Aa-";

            IdentityUser? userSeeded = await this.userManager.FindByEmailAsync(defaultUserEmail);

            if (userSeeded == null)
            {
                IdentityUser defaultUser = new IdentityUser()
                {
                    UserName = defaultUserEmail,
                    Email = defaultUserEmail
                };

                await this.userManager.CreateAsync(defaultUser, defaultUserPassword);
                await this.userManager.AddToRoleAsync(defaultUser, "User");
            }
        }
    }
}
