using ShopHub.Business;
using ShopHub.Business.Interfaces.Services;
using ShopHub.Data;
using ShopHub.Entities.Constants;
using ShopHub.Web.Services;

namespace ShopHub.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllersWithViews();
        services.AddRazorPages().AddRazorRuntimeCompilation();


        services.AddHttpContextAccessor();
        services.AddMemoryCache();
        services.AddDistributedMemoryCache();
        services.AddSession();
        services.AddScoped<ICartSession, SessionCartSession>();

        services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.AdminAccess, policy =>
            {
                policy.RequireRole(Roles.Admin, Roles.SuperAdmin);
            });
        });

        // Data layer Services
        services.AddDataServices(configuration);
        // Business layer Services
        services.AddBusinessServices(configuration);

        return services;
    }
}
