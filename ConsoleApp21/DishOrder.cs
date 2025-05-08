public class DishOrder : Order
{

    /// <summary>
    /// Dish or List<Dish>
    /// </summary>
    public Dish Value { get; set; } = null!;
    public override decimal Price => Value.Price;
}
