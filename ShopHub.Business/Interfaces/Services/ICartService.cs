using ShopHub.Entities.Models;

namespace ShopHub.Business.Interfaces.Services;

public interface ICartService
{
    List<CartItem> GetCart();
    Task AddItemAsync(
        int productId,
        int quantity = 1,
        CancellationToken cancellationToken = default);
    void RemoveItem(int productId);

    void IncreaseQuantity(int productId);

    void DecreaseQuantity(int productId);

    void ClearCart();
}