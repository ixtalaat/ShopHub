namespace ShopHub.Entities.Models;

public class OrderDetail
{
    public int Id { get; set; }

    public int OrderHeaderId { get; set; }
    public OrderHeader OrderHeader { get; set; } = null!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public decimal Price { get; set; }

    public int Count { get; set; }


}
