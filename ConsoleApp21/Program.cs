using System.ComponentModel.Design;
using System.Data.SqlTypes;
using System.Reflection.Metadata.Ecma335;

namespace ConsoleApp21;

class Program
{
    static void Main(string[] args)
    {    
        Guest[] guests = [
            new(1, "Даша", 11.12m),
            new(2, "Марк", 1.3m),
            new(3, "Тарас", 5.52m)
        ];


        IList<Dish> Menu = [
            new() { Id = 1, Name = "Бургер", Price = 4.59m },
            new() { Id = 2, Name = "Картошка фри", Price = 2.59m },
            new() { Id = 3, Name = "Кофе", Price = 1.99m }
        ];

        Restaurant restaurant = new(Menu);

        restaurant.AddGuest(guests);


        //restaurant.AddManyDishOrder(1, 1);

        restaurant.AddOrder(1, []);


        //для теста
        try
        {
            var order = restaurant.Guests[0].Cart.Orders.FirstOrDefault() ?? throw new Exception("У данного геста нет заказов(");

            restaurant.Guests[0].PayTheBill(order.Id);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}