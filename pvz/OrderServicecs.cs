using pvz;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GMWOGMarketplace
{
    public static class OrderService
    {
        public static void ShowHistory()
        {
            Console.WriteLine("\n История заказов");

            var orders = Program.db.Order.Where(o => o.UserId == Program.currentUser.Id).ToList();

            if (orders.Count == 0)
            {
                Console.WriteLine("У вас еще нет заказов.");
                return;
            }

            Console.WriteLine("Сортировать по дате: 1 - сначала новые, 2 - сначала старые");
            string sortChoice = Console.ReadLine();

            List<Order> sortedOrders;
            if (sortChoice == "2")
                sortedOrders = orders.OrderBy(o => o.OrderDate).ToList();
            else
                sortedOrders = orders.OrderByDescending(o => o.OrderDate).ToList();

            foreach (var order in sortedOrders)
            {
                Console.WriteLine($"\nЗаказ №{order.Id} от {order.OrderDate}");
                var pvz = Program.db.PickupPoint.First(p => p.Id == order.PickupPointId);
                Console.WriteLine($"ПВЗ: {pvz.Address} | Сумма: {order.TotalAmount} руб.");

                var details = Program.db.OrderDetail.Where(d => d.OrderId == order.Id).ToList();
                Console.WriteLine("Товары в заказе:");
                foreach (var d in details)
                {
                    var product = Program.db.Product.First(p => p.Id == d.ProductId);
                    Console.WriteLine($"  - {product.Name} x{d.Quantity} = {d.PriceAtPurchase * d.Quantity} руб.");
                }
            }
        }
    }
}