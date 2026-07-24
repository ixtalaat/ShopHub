using ShopHub.Business;
using ShopHub.Data;

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

        // Data layer Services
        services.AddDataServices(configuration);
        // Business layer Services
        services.AddBusinessServices();

        return services;
    }
}
