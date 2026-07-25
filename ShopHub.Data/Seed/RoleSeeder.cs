using Microsoft.AspNetCore.Identity;
using ShopHub.Entities.Constants;

namespace ShopHub.Data.Seed;

public static class RoleSeeder
{
    public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roles =
        {
            Roles.SuperAdmin,
            Roles.Admin,
            Roles.Customer
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
