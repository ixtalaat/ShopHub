using AutoMapper;
using ShopHub.Business.Dtos.User;
using ShopHub.Business.Interfaces.Repositories;
using ShopHub.Business.Interfaces.Services;

namespace ShopHub.Business.Services;

internal class UserService(
    IUnitOfWork unitOfWork,
    IMapper mapper) : IUserService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public Task<IReadOnlyList<UserListDto>> GetAllWithRoleAsync(CancellationToken cancellationToken = default)
    {
        return _unitOfWork.Users.GetAllWithRolesAsync(cancellationToken);
    }
}
