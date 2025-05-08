public class Dish
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }

    public override string ToString() => $"{Id}: name = {Name}, price = {Price:C2}";
}
