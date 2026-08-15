using ShopHub.Entities.Models;

namespace ShopHub.Web.ViewModels;

public class CartViewModel
{
    public required IEnumerable<CartItem> CartList { get; set; }
}
