using ShopHub.Business.Enums;

namespace ShopHub.Business.Dtos.Product;

public class ProductQueryParameters
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public ProductSortBy SortBy { get; set; } = ProductSortBy.Name;
    public SortDirection SortDirection { get; set; } = SortDirection.Asc;
}
