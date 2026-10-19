using pvz;
using System;
using System.Linq;

namespace GMWOGMarketplace
{
    public static class ProductService
    {
        public static void ShowProducts()
        {
            Console.WriteLine("\nСписок товаров");
            var products = Program.db.Product.ToList();

            foreach (var p in products)
            {
                Console.WriteLine($"ID: {p.Id} | {p.Name} | Цена: {p.Price} руб. | {p.Description}");
            }

            if (Program.currentUser != null)
            {
                Console.Write("\nВведите ID товара, чтобы добавить в корзину (или 0 для выхода): ");
                if (int.TryParse(Console.ReadLine(), out int productId) && productId > 0)
                {
                    var product = Program.db.Product.FirstOrDefault(p => p.Id == productId);
                    if (product != null)
                    {
                        CartService.AddToCart(product);
                    }
                    else
                    {
                        Console.WriteLine("Товар не найден.");
                    }
                }
            }
        }
    }
}