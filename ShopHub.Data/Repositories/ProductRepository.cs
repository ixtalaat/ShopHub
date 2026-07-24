using Microsoft.EntityFrameworkCore;
using myshop.Data.Context;
using myshop.Entities.Models;
using ShopHub.Business.Dtos.Product;
using ShopHub.Business.Interfaces.Repositories;

namespace ShopHub.Data.Repositories;

internal class ProductRepository(ApplicationDbContext context) : GenericRepository<Product>(context), IProductRepository
{
    public async Task<IReadOnlyList<ProductListDto>> GetAllWithCategoryAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Products
        .AsNoTracking()
        .Select(p => new ProductListDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            CategoryName = p.Category.Name
        })
        .ToListAsync(cancellationToken);
    }
}
