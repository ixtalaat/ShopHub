using Microsoft.AspNetCore.Http;
using ShopHub.Business.Interfaces.Repositories;
using ShopHub.Business.Interfaces.Services;
using ShopHub.Entities.Models;

namespace ShopHub.Business.Services;

public class CartService(IUnitOfWork unitOfWork, ICartSession cartSession) : ICartService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICartSession _cartSession = cartSession;

    private const string CartKey = "Cart";
    public List<CartItem> GetCart()
    {
        return _cartSession.Get();
    }
    public async Task AddItemAsync(int productId, int quantity = 1, CancellationToken cancellationToken = default)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");

        var product = await _unitOfWork.Products
            .GetByIdAsync(productId, cancellationToken);

        if (product is null)
            throw new Exception("Product not found.");

        var cart = GetCart();

        var existingItem = cart.FirstOrDefault(
            x => x.ProductId == productId);

        if (existingItem is not null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            cart.Add(new CartItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Price = product.Price,
                Quantity = quantity,
                ImageUrl = product.ImageUrl
            });
        }

        SaveCart(cart);
    }
    public void RemoveItem(int productId)
    {
        var cart = GetCart();

        var item = cart.FirstOrDefault(
            x => x.ProductId == productId);

        if (item is null)
            return;

        cart.Remove(item);

        SaveCart(cart);
    }
    public void DecreaseQuantity(int productId)
    {
        var cart = GetCart();

        var item = cart.FirstOrDefault(
            x => x.ProductId == productId);

        if (item is null)
            return;

        item.Quantity--;

        if (item.Quantity <= 0)
            cart.Remove(item);

        SaveCart(cart);
    }
    public void IncreaseQuantity(int productId)
    {
        var cart = GetCart();

        var item = cart.FirstOrDefault(
            x => x.ProductId == productId);

        if (item is null)
            return;

        item.Quantity++;

        SaveCart(cart);
    }
    public void ClearCart()
    {
        var cart = GetCart();
        cart.Clear();
        SaveCart(cart);
    }
    private void SaveCart(List<CartItem> cart)
    {
        _cartSession.Set(cart);
    }
}
