using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using myshop.Web.ViewModels;
using ShopHub.Business.Interfaces.Services;
using ShopHub.Entities.Constants;
using System.Security.Claims;

namespace myshop.Web.Areas.Admin.Controllers;
[Authorize(Policy = Policies.AdminAccess)]
public class UserController(IUserService userService, RoleManager<IdentityRole> roleManager) : Controller
{
    private readonly IUserService _userService = userService;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> GetData()
    {
        var UserListDto = await _userService.GetAllWithRoleAsync();

        return Json(new { data = UserListDto });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string? id, CancellationToken cancellationToken)
    {
        if (id == null)
        {
            return NotFound();
        }

        var userDto = await _userService.GetByIdAsync(id, cancellationToken);
        var roles = await _roleManager.Roles.ToListAsync();
        UserViewModel userViewModel = new UserViewModel()
        {
            UserDto = userDto!,
            RoleList = roles.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            })
        };

        return View(userViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UserViewModel userViewModel)
    {
        if (ModelState.IsValid)
        {
            
            await _userService.UpdateAsync(userViewModel.UserDto);

            TempData["Update"] = "Data has Updated Successfully";
            return RedirectToAction("Index");
        }

        return View(userViewModel.UserDto);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(string? id, CancellationToken cancellationToken)
    {
        if (id == null)
        {
            return NotFound();
        }

        var claimsIdentity = (ClaimsIdentity)User.Identity!;
        var currentUserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        if (id == currentUserId)
        {
            return Json(new { success = false, message = "Error while Deleting, You can not delete yourself :>" });
        }

        var userDto = await _userService.GetByIdAsync(id, cancellationToken);

        if (userDto == null)
        {
            return Json(new { success = false, message = "Error while Deleting" });
        }

        await _userService.DeleteAsync(userDto.Id, cancellationToken);

        return Json(new { success = true, message = "User has been Deleted" });
    }
}
