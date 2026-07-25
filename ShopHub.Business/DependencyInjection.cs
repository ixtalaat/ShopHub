using Microsoft.Extensions.DependencyInjection;
using ShopHub.Business.Interfaces.Services;
using ShopHub.Business.Services;

namespace ShopHub.Business;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());

        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        return services;
    }
}
