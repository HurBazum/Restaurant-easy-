public class ClientCart()
{
    public IList<Order> Orders { get; set; } = [];

    public decimal TotalPrice => Orders.Where(x => x.Status == OrderStatus.Paid).Sum(d => d.Price);
}
