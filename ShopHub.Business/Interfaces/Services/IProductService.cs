using ShopHub.Entities.Models;
using ShopHub.Business.Dtos.Product;

namespace ShopHub.Business.Interfaces.Services;

public interface IProductService
{
    Task<IReadOnlyList<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProductListDto>> GetAllWithCategoryAsync(CancellationToken cancellationToken = default);

    Task<ProductDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task CreateAsync(ProductDto product, CancellationToken cancellationToken = default);

    Task UpdateAsync(ProductDto product, CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
