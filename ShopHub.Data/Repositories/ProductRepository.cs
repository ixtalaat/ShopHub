using Microsoft.EntityFrameworkCore;
using ShopHub.Data.Context;
using ShopHub.Entities.Models;
using ShopHub.Business.Dtos.Common;
using ShopHub.Business.Dtos.Product;
using ShopHub.Business.Enums;
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
                ImageUrl = p.ImageUrl,
                Price = p.Price,
                CategoryName = p.Category.Name
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<PagedResult<ProductListDto>> GetPagedWithCategoryAsync(
        ProductQueryParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var pageNumber = parameters.PageNumber < 1 ? 1 : parameters.PageNumber;
        var pageSize = parameters.PageSize < 1 ? 10 : parameters.PageSize;

        IQueryable<Product> query = _context.Products.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
        {
            var searchTerm = parameters.SearchTerm.Trim();
            query = query.Where(p =>
                p.Name.Contains(searchTerm) ||
                p.Description.Contains(searchTerm));
        }

        query = ApplySorting(query, parameters.SortBy, parameters.SortDirection);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductListDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                Price = p.Price,
                CategoryName = p.Category.Name
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<ProductListDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    private static IQueryable<Product> ApplySorting(
        IQueryable<Product> query,
        ProductSortBy sortBy,
        SortDirection sortDirection)
    {
        return (sortBy, sortDirection) switch
        {
            (ProductSortBy.Name, SortDirection.Desc) => query.OrderByDescending(p => p.Name),
            (ProductSortBy.Name, _) => query.OrderBy(p => p.Name),
            (ProductSortBy.Price, SortDirection.Desc) => query.OrderByDescending(p => p.Price),
            (ProductSortBy.Price, _) => query.OrderBy(p => p.Price),
            _ => query.OrderBy(p => p.Name)
        };
    }
}
