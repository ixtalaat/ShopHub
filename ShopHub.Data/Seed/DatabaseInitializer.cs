using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using myshop.Data.Context;
using myshop.Entities.Models;

namespace ShopHub.Data.Seed;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(IHost app)
    {
        using var scope = app.Services.CreateScope();

        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        await context.Database.MigrateAsync();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await RoleSeeder.SeedAsync(roleManager);
        await SuperAdminSeeder.SeedAsync(userManager);
    }
}
