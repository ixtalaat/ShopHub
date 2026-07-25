using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShopHub.Business.Dtos.Product;

public class ProductDto
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    [DisplayName("Image")]
    public string? Img { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    [DisplayName("Category")]
    public int CategoryId { get; set; }
}
