using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopHub.Business.Dtos.Product;

namespace ShopHub.Web.ViewModels;

public class ProductViewModel
{
    public ProductDto ProductDto { get; set; } = null!;
    [ValidateNever]
    public IEnumerable<SelectListItem> CategoryList { get; set; } = null!;
}
