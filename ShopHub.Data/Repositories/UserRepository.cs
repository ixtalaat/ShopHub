using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using myshop.Data.Context;
using myshop.Entities.Models;
using ShopHub.Business.Dtos.User;
using ShopHub.Business.Interfaces.Repositories;
using ShopHub.Entities.Constants;

namespace ShopHub.Data.Repositories;

internal class UserRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : IUserRepository
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<IReadOnlyList<UserListDto>> GetAllWithRolesAsync(CancellationToken cancellationToken = default)
    {
        var superAdminRoleId = await _context.Roles
            .Where(r => r.Name == Roles.SuperAdmin)
            .Select(r => r.Id)
            .FirstOrDefaultAsync(cancellationToken);
        
        // without super admin
        var users = await _context.Users
            .AsNoTracking()
            .Where(u => !_context.UserRoles
                .Any(ur => ur.UserId == u.Id && ur.RoleId == superAdminRoleId))
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

    public async Task<ApplicationUser?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.Users.FindAsync(id, cancellationToken);
    }
}
