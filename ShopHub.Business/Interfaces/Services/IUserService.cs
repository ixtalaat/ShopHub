using ShopHub.Business.Dtos.User;

namespace ShopHub.Business.Interfaces.Services;

public interface IUserService
{
    Task<IReadOnlyList<UserListDto>> GetAllWithRoleAsync(CancellationToken cancellationToken = default);
}
