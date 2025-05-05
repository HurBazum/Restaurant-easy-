using System.ComponentModel.Design;
using System.Data.SqlTypes;

List<Guest> guests = [
    new("Даша", 11.12m),
    new("Марк", 1.3m),
    new("Тарас", 5.52m)
    ];


ICollection<Dish> Menu = [
    new() {Id = 1, Name = "Бургер", Price = 4.59m },
    new() { Id = 2, Name = "Картошка фри", Price = 2.59m },
    new() { Id = 3, Name = "Кофе", Price = 1.99m }
    ];

Restaurant restaurant = new(Menu);

restaurant.AddOrder(1, guests[0]);
restaurant.CookDish();
restaurant.AddOrder(2, guests[1]);
guests[1].Refill(2.01m);
restaurant.AddOrder(2, guests[1]);
restaurant.CookDish();
restaurant.CheckAmountOfOrders();



public class Guest(string name, decimal money)
{
    public string Name { get; set; } = name;
    private decimal _availableMoney = money;
    public decimal AvailableMoney 
    { 
        get => _availableMoney;
        set
        {
            if(value < 0)
            {
                throw new InvalidOperationException("Недостаточно средств для совершения данной операции!");
            }
            else
            {
                _availableMoney = value;
            }
        }
    }

    public void Buy(decimal price)
    {
        AvailableMoney -= price;
    }

    public void Refill(decimal money)
    {
        try
        {
            if(money <= 0)
            {
                throw new InvalidOperationException("Невозможно добавить такую сумму на счёт");
            }
            AvailableMoney += money;
            Console.WriteLine($"{Name}, деньги в размере {money:C2} успешно зачислены на счёт");
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public void CheckBalance()
    {
        Console.WriteLine($"{Name}, Ваш баланс равен {AvailableMoney:C2}");
    }
}

public class Restaurant(ICollection<Dish> menu)
{
    public readonly Dictionary<int, Dish> Menu = menu.ToDictionary(x => x.Id, x => x);

    public decimal Profit { get; set; }

    public bool ProfitCount { get; private set; } = false;

    public Queue<string> CurrentOrders = new();

    public void AddOrder(int dishId, Guest guest)
    {
        try
        {
            if(CurrentOrders == null)
            {
                throw new InvalidOperationException($"Невозможно работать с неинициализированной очередью");
            }
            if(Menu == null)
            {
                throw new ArgumentNullException(nameof(Menu), "В нашем ресторане ещё не сделали меню");
            }
            if(Menu[dishId] == null)
            {
                throw new InvalidOperationException($"Невозможно заказать блюдо. Блюда с таким id={dishId} не существует");
            }
            guest.Buy(Menu[dishId].Price);
            CurrentOrders.Enqueue($"{Menu[dishId].Name} для {guest.Name} ({Menu[dishId].Price:C2})");
            Profit += Menu[dishId].Price;
            if(ProfitCount == true)
            {
                ProfitCount = false;
            }
            Console.WriteLine($"Добавлен заказ: {Menu[dishId].Name} для {guest.Name}, Цена - {Menu[dishId].Price:C2}");
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    public void CookDish()
    {
        try
        {
            if(CurrentOrders == null)
            {
                throw new InvalidOperationException($"Невозможно работать с неинициализированной очередью");
            }
            if(CurrentOrders.Count == 0)
            {
                CheckAmountOfOrders();
            }
            else
            {
                var dish = CurrentOrders.Dequeue();
                Console.WriteLine($"Готовится: {dish}");
                if(CurrentOrders.Count == 0)
                {
                    Console.WriteLine("Все заказы готовы!");
                }
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public void CheckAmountOfOrders()
    {
        try
        {
            if(CurrentOrders == null)
            {
                throw new InvalidOperationException($"Невозможно работать с неинициализированной очередью");
            }
            if(CurrentOrders.Count == 0)
            {
                if(ProfitCount == false)
                {
                    Console.WriteLine($"Нет заказов\nРесторан заработал: {Profit:C2}");
                    ProfitCount = true;
                }
                else
                {
                    return;
                }
            }
            else
            {
                var d = CurrentOrders.Count % 100;
                if(d == 1)
                {
                    Console.WriteLine($"Очередь: {CurrentOrders.Count} заказ остался");
                }
                else if(d < 5)
                {
                    Console.WriteLine($"Очередь: {CurrentOrders.Count} заказа осталось");
                }
                else
                {
                    Console.WriteLine($"Очередь: {CurrentOrders.Count} заказов осталось");
                }
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}

public class Dish
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }

    public override string ToString() => $"{Id}: name = {Name}, price = {Price:C2}";
}