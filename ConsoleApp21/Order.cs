public abstract class Order
{
    public int Id { get; set; }
    public OrderStatus Status { get; set; }


    public string Message { get; set; }

    public DateTime DoneDate { get; set; }
    public virtual decimal Price { get; set; }
}
