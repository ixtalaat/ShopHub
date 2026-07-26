using ShopHub.Business.Dtos.User;

namespace ShopHub.Business.Interfaces.Repositories;

public interface IUserRepository
{
    Task<IReadOnlyList<UserListDto>> GetAllWithRolesAsync(CancellationToken cancellationToken = default);
}
