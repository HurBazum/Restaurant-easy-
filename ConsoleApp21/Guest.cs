namespace ConsoleApp21;
public class Guest(int id, string name, decimal money)
{
    public int NextOrderId
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;
    private decimal _availableMoney = money;
    public ClientCart Cart { get; set; } = new();
    public event CookDishHandler CookOrder;

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

    public void PayTheBill(int orderId)
    {
        var orderForPaying = Cart.Orders.FirstOrDefault(x => x.Id == orderId);

        if(orderForPaying == null)
        {
            throw new InvalidOperationException($"Нет заказа с id = {orderId}");
        }

        Buy(orderForPaying.Price);

        orderForPaying.Status = OrderStatus.Paid;

        orderForPaying.Message += $"Заказ оплачен {DateTime.UtcNow}";
        CookOrder?.Invoke(Id, orderForPaying.Id);
    }
}
