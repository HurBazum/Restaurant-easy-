public class DishesOrder : Order
{
    public IList<Dish> Dishes { get; set; } = null!;

    public override decimal Price => Dishes.Sum(x => x.Price);
}
