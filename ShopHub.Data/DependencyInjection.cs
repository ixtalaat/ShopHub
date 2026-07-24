using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using myshop.Data.Context;
using myshop.Entities.Models;
using ShopHub.Business.Interfaces.Repositories;
using ShopHub.Data.Repositories;

namespace ShopHub.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentity<ApplicationUser, IdentityRole>(
            options => options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(4)
        ).AddDefaultTokenProviders()
         .AddDefaultUI()
         .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
