using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using myshop.Entities.Models;
using ShopHub.Business.Dtos.User;
using ShopHub.Business.Interfaces.Repositories;
using ShopHub.Business.Interfaces.Services;
using ShopHub.Entities.Constants;

namespace ShopHub.Business.Services;

internal class UserService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    RoleManager<IdentityRole> roleManager,
    UserManager<ApplicationUser> userManager) : IUserService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<IReadOnlyList<UserListDto>> GetAllWithRoleAsync(CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.Users.GetAllWithRolesAsync(cancellationToken);
    }

    public async Task<UserDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id, cancellationToken);

        if (user is null)
            return null;

        var userDto = _mapper.Map<UserDto>(user);

        var roleName = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(roleName))
        {
            userDto.RoleId = (await _roleManager.Roles
                .Where(r => r.Name == roleName)
                .Select(r => r.Id)
                .FirstOrDefaultAsync(cancellationToken))!;
        }

        return userDto;
    }

    public async Task UpdateAsync(UserDto userDto, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userDto.Id);

        if (user is null)
            throw new Exception("User not found.");

        var isSuperAdmin = await _userManager.IsInRoleAsync(user, Roles.SuperAdmin);

        if (isSuperAdmin)
        {
            throw new UnauthorizedAccessException("Super Admin accounts cannot be modified.");
        }

        // Update basic information
        user.FullName = userDto.FullName;

        // Update lock status
        await _userManager.SetLockoutEndDateAsync(
            user,
            userDto.IsLocked ? DateTimeOffset.MaxValue : null);

        // Get the selected role
        var newRole = await _roleManager.FindByIdAsync(userDto.RoleId);

        if (newRole is null)
            throw new Exception("Role not found.");

        // Get current roles
        var currentRoles = await _userManager.GetRolesAsync(user);

        // Replace role only if it changed
        if (!currentRoles.Contains(newRole.Name!))
        {
            if (currentRoles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

                if (!removeResult.Succeeded)
                    throw new Exception("Failed to remove existing role.");
            }

            var addResult = await _userManager.AddToRoleAsync(user, newRole.Name!);

            if (!addResult.Succeeded)
                throw new Exception("Failed to assign new role.");
        }

        var updateResult = await _userManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
            throw new Exception("Failed to update user.");

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}