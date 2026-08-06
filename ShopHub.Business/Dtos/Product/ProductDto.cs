using Microsoft.AspNetCore.Http;
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
    public string? ImageUrl { get; set; }
    [AllowedExtensions(new[] { ".jpg", ".jpeg", ".png", ".webp" },
        ErrorMessage = "Only JPG, JPEG, PNG and WEBP images are allowed.")]
    [MaxFileSize(2 * 1024 * 1024,
        ErrorMessage = "Maximum image size is 2 MB.")]
    public IFormFile? Image { get; set; }
    [Required]
    public decimal Price { get; set; }

    [Required]
    [DisplayName("Category")]
    public int CategoryId { get; set; }
}
