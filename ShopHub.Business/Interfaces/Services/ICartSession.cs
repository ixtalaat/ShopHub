using ShopHub.Entities.Models;

namespace ShopHub.Business.Interfaces.Services;

public interface ICartSession
{
    List<CartItem> Get();

    void Set(List<CartItem> cart);

    void Clear();
}
