using ShopHub.Business.Dtos.Category;
using ShopHub.Business.Dtos.Common;
using ShopHub.Business.Dtos.Product;

namespace ShopHub.Web.ViewModels;

public class ProductBrowseViewModel
{
    public PagedResult<ProductListDto> Products { get; set; } = null!;
    public IReadOnlyList<CategoryDto> Categories { get; set; } = [];
    public ProductQueryParameters Query { get; set; } = new();
}
