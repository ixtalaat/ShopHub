namespace ShopHub.Business.Dtos.User;

public class UserDto
{
    public string Id { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool IsLocked { get; set; }
    public string RoleId { get; set; } = null!;
}
