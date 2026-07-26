using ShopHub.Business.Dtos.Product;
using ShopHub.Business.Dtos.User;

namespace ShopHub.Business.Interfaces.Services;

public interface IUserService
{
    Task<IReadOnlyList<UserListDto>> GetAllWithRoleAsync(CancellationToken cancellationToken = default);

    Task<UserDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    Task UpdateAsync(UserDto product, CancellationToken cancellationToken = default);

    Task DeleteAsync(string currentUserId, string targetUserId, CancellationToken cancellationToken = default);
}
