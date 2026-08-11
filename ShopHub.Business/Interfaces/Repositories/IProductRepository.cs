using ShopHub.Entities.Models;
using ShopHub.Business.Dtos.Product;

namespace ShopHub.Business.Interfaces.Repositories;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<IReadOnlyList<ProductListDto>> GetAllWithCategoryAsync(CancellationToken cancellationToken = default);
}
