using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopHub.Business.Dtos.User;

namespace ShopHub.Web.ViewModels;

public class UserViewModel
{
    public UserDto UserDto { get; set; } = null!;
    [ValidateNever]
    public IEnumerable<SelectListItem> RoleList { get; set; } = null!;
}
