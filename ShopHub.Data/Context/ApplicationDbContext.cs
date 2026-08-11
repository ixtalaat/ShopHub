using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopHub.Entities.Models;

namespace ShopHub.Data.Context;

public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {            
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }

    
}
