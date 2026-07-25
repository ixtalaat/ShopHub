using ShopHub.Business;
using ShopHub.Data;
using ShopHub.Entities.Constants;

namespace myshop.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllersWithViews();
        services.AddRazorPages().AddRazorRuntimeCompilation();

        services.AddHttpContextAccessor();

        services.AddDistributedMemoryCache();
        services.AddSession();

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
        services.AddBusinessServices();

        return services;
    }
}
