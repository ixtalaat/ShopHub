using ShopHub.Business.Dtos.Category;

namespace ShopHub.Business.Interfaces.Services;

public interface ICategoryService
{
    Task<IReadOnlyList<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<CategoryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task CreateAsync(CategoryDto category, CancellationToken cancellationToken = default);

    Task UpdateAsync(CategoryDto category, CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
