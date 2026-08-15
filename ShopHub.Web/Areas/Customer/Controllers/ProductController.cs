using Microsoft.AspNetCore.Mvc;
using ShopHub.Business.Dtos.Product;
using ShopHub.Business.Enums;
using ShopHub.Business.Interfaces.Services;
using ShopHub.Web.ViewModels;

namespace ShopHub.Web.Areas.Customer.Controllers;

[Area("Customer")]
public class ProductController(
    IProductService productService,
    ICategoryService categoryService) : Controller
{
    private readonly IProductService _productService = productService;
    private readonly ICategoryService _categoryService = categoryService;

    [HttpGet]
    public async Task<IActionResult> Index(
        int pageNumber = 1,
        int pageSize = 10,
        string? search = null,
        ProductSortBy sortBy = ProductSortBy.Name,
        SortDirection sortDirection = SortDirection.Asc,
        CancellationToken cancellationToken = default)
    {
        var query = new ProductQueryParameters
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = search,
            SortBy = sortBy,
            SortDirection = sortDirection
        };

        var products = await _productService.GetPagedWithCategoryAsync(query, cancellationToken);
        var categories = await _categoryService.GetAllAsync(cancellationToken);

        var viewModel = new ProductBrowseViewModel
        {
            Products = products,
            Categories = categories,
            Query = query
        };

        return View(viewModel);
    }
}
