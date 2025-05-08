using System.Text;

namespace ConsoleApp21;
public class Restaurant(IList<Dish> menu)
{
    public readonly Dictionary<int, Dish> Menu = menu.ToDictionary(x => x.Id, x => x);
    public Dictionary<int, List<Order>> OrdersHistory = [];
    public decimal Profit { get; set; }

    public bool ProfitCount { get; private set; } = false;

    public Queue<Order> CurrentOrders = new();

    public List<Guest> Guests { get; set; } = [];

    // типа регистрация))
    public void AddGuest(params Guest[] guests)
    {
        foreach(var guest in guests)
        {
            Guests.Add(guest);
            guest.CookOrder += CookDish;
        }
    }

    public void AddOrder(int guestId, params int[] dishId)
    {
        try
        {
            IList<Dish> dishList = [];

            if(Menu == null)
            {
                throw new ArgumentNullException(nameof(Menu), "В нашем ресторане ещё не сделали меню");
            }
            
            if(dishId == null || dishId.Count() == 0)
            {
                throw new ArgumentNullException(nameof(dishId), "В нашем ресторане пустоту не подают");
            }

            Guest currentGuest = Guests.FirstOrDefault(x => x.Id == guestId) ??
                throw new InvalidOperationException($"Клиента с id={guestId} не существует");

            foreach(var id in dishId)
            {
                if(!Menu.TryGetValue(id, out Dish d))
                {
                    throw new InvalidOperationException($"Невозможно заказать блюдо. Блюда с таким id={id} не существует");
                }

                dishList.Add(d);
            }

            Order order = null;

            if(dishList.Count > 1)
            {
                order = new DishesOrder() { Id = currentGuest.Cart.Orders.Count, Dishes = dishList, Status = OrderStatus.Done, DoneDate = DateTime.UtcNow };
            }
            else
            {
                order = new DishOrder() { Id = currentGuest.Cart.Orders.Count, Value = dishList[0], Status = OrderStatus.Done, DoneDate = DateTime.UtcNow };
            }

            currentGuest.Cart.Orders.Add(order);

            Console.WriteLine($"{currentGuest.Name}, заказ успешно добавлен в корзину.");
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public void CookDish(int guestId, int orderId)
    {
        try
        {
            var guest = Guests.FirstOrDefault(x => x.Id == guestId);
            var order = guest.Cart.Orders.FirstOrDefault(x => x.Id == orderId);

            if(order is DishOrder)
            {
                Console.WriteLine($"Готовится {(order as DishOrder).Value.Name} для {guest.Name}");
                Thread.Sleep(2000);
                order.Status = OrderStatus.Cooked;
                Console.WriteLine($"Заказ: \"{(order as DishOrder).Value.Name} для {guest.Name}\" готов!");
            }
            else
            {
                StringBuilder sb = new();
                sb.Append($"Готовится ");
                var o = order as DishesOrder;
                int limit = o.Dishes.Count;
                for(int i = 0; i < limit; i++)
                {
                    if(i != limit - 1)
                    {
                        sb.Append($"{o.Dishes[i].Name}, ");
                    }
                    else
                    {
                        sb.AppendLine($"{o.Dishes[i].Name} для {guest.Name}");
                    }
                }

                var response = sb.ToString();

                Console.WriteLine(response);
                Thread.Sleep(2000);
                order.Status = OrderStatus.Cooked;
                Console.WriteLine($"Заказ готов!");
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
