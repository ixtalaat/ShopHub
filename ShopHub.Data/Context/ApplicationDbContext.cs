using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using myshop.Entities.Models;

namespace myshop.Data.Context
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        
    }
}
