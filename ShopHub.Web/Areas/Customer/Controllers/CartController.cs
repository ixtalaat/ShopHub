using Microsoft.AspNetCore.Mvc;
using ShopHub.Business.Interfaces.Services;
using ShopHub.Web.ViewModels;

namespace ShopHub.Web.Areas.Customer.Controllers;

[Area("Customer")]
public class CartController(ICartService cartService) : Controller
{
    private readonly ICartService _cartService = cartService;

    [HttpGet]
    public IActionResult Index()
    {
        var cartViewModel = new CartViewModel
        {
            CartList = _cartService.GetCart()
        };

        return View(cartViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Add(
    int productId,
    int quantity = 1)
    {
        await _cartService.AddItemAsync(productId, quantity);

        return RedirectToAction(nameof(Index));
    }
    [HttpPost]
    public async Task<IActionResult> Remove(int productId)
    {
        _cartService.RemoveItem(productId);
        return RedirectToAction(nameof(Index));
    }
    [HttpPost]
    public async Task<IActionResult> Plus(int productId)
    {
        _cartService.IncreaseQuantity(productId);
        return RedirectToAction(nameof(Index));
    }
    [HttpPost]
    public async Task<IActionResult> Minus(int productId)
    {
        _cartService.DecreaseQuantity(productId);
        return RedirectToAction(nameof(Index));
    }
    [HttpPost]
    public async Task<IActionResult> Clear()
    {
        _cartService.ClearCart();
        return RedirectToAction(nameof(Index));
    }
}
