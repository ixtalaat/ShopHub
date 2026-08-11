using Microsoft.AspNetCore.Identity;
using ShopHub.Entities.Models;
using ShopHub.Entities.Constants;

namespace ShopHub.Data.Seed;

public static class SuperAdminSeeder
{
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
    {
        const string adminEmail = "admin@shophub.com";
        const string adminPassword = "Password123$";

        var admin = await userManager.FindByEmailAsync(adminEmail);

        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "Super Admin",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(admin, adminPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, Roles.SuperAdmin);
            }
        }
    }
}

