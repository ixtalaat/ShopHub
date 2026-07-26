using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using myshop.Data.Context;
using myshop.Entities.Models;
using ShopHub.Business.Dtos.User;
using ShopHub.Business.Interfaces.Repositories;

namespace ShopHub.Data.Repositories;

internal class UserRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : IUserRepository
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<IReadOnlyList<UserListDto>> GetAllWithRolesAsync(CancellationToken cancellationToken = default)
    {
        var users = await _context.Users
        .AsNoTracking()
        .ToListAsync(cancellationToken);

        var result = new List<UserListDto>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);

            result.Add(new UserListDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email!,
                Role = roles.FirstOrDefault()!,
                IsLocked = user.LockoutEnd.HasValue &&
                           user.LockoutEnd > DateTimeOffset.UtcNow
            });
        }

        return result;
    }
}
