using ShopHub.Entities.Models;
using ShopHub.Business.Dtos.User;

namespace ShopHub.Business.Interfaces.Repositories;

public interface IUserRepository
{
    Task<IReadOnlyList<UserListDto>> GetAllWithRolesAsync(CancellationToken cancellationToken = default);
    Task<ApplicationUser?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
}
