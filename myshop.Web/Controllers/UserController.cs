using Microsoft.AspNetCore.Mvc;
using ShopHub.Business.Interfaces.Services;

namespace myshop.Web.Controllers;

public class UserController(IUserService userService) : Controller
{
    private readonly IUserService _userService = userService;

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> GetData()
    {
        var UserListDto = await _userService.GetAllWithRoleAsync();

        return Json(new { data = UserListDto });
    }
}
