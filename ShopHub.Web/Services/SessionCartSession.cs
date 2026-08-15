using ShopHub.Business.Interfaces.Services;
using ShopHub.Entities.Models;
using ShopHub.Web.Extensions;

namespace ShopHub.Web.Services;

public class SessionCartSession(IHttpContextAccessor httpContextAccessor) : ICartSession
{
    private const string CartKey = "Cart";

    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public List<CartItem> Get()
    {
        return _httpContextAccessor.HttpContext?
            .Session
            .GetObject<List<CartItem>>(CartKey)
            ?? [];
    }

    public void Set(List<CartItem> cart)
    {
        _httpContextAccessor.HttpContext?
            .Session
            .SetObject(CartKey, cart);
    }

    public void Clear()
    {
        _httpContextAccessor.HttpContext?
            .Session
            .Remove(CartKey);
    }
}