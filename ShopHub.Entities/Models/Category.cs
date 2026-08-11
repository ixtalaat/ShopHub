using System.ComponentModel.DataAnnotations;

namespace ShopHub.Entities.Models;

public class Category
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;
    public DateTime CreatedTime { get; set; } = DateTime.Now;
}
