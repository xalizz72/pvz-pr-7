using pvz;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GMWOGMarketplace
{
    public static class CartService
    {
        public static void AddToCart(Product product)
        {
            var existingItem = Program.db.CartItem.FirstOrDefault(c => c.UserId == Program.currentUser.Id && c.ProductId == product.Id);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                CartItem newItem = new CartItem
                {
                    UserId = Program.currentUser.Id,
                    ProductId = product.Id,
                    Quantity = 1
                };
                Program.db.CartItem.Add(newItem);
            }

            Program.db.SaveChanges();
            Console.WriteLine($"Товар '{product.Name}' добавлен в корзину.");
        }

        public static void ManageCart()
        {
            Console.WriteLine("\n Ваша корзина: ");
            var cartItems = Program.db.CartItem.Where(c => c.UserId == Program.currentUser.Id).ToList();

            if (cartItems.Count == 0)
            {
                Console.WriteLine("Корзина пуста.");
                return;
            }

            decimal total = 0;
            foreach (var item in cartItems)
            {
                var product = Program.db.Product.First(p => p.Id == item.ProductId);
                decimal sum = product.Price * item.Quantity;
                total += sum;
                Console.WriteLine($"ID корзины: {item.Id} | {product.Name} | Кол-во: {item.Quantity} | Сумма: {sum} руб.");
            }

            Console.WriteLine($"\nИтого к оплате: {total} руб.");

            Console.WriteLine("\nДействия:");
            Console.WriteLine("1. Купить весь товар из корзины");
            Console.WriteLine("2. Купить один товар (по ID корзины)");
            Console.WriteLine("0. Назад");
            Console.Write("Выбор: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                CheckoutCart(cartItems);
            }
            else if (choice == "2")
            {
                Console.Write("Введите ID позиции из корзины: ");
                if (int.TryParse(Console.ReadLine(), out int cartId))
                {
                    var itemToBuy = cartItems.FirstOrDefault(c => c.Id == cartId);
                    if (itemToBuy != null) CheckoutCart(new List<CartItem> { itemToBuy });
                    else Console.WriteLine("Позиция не найдена.");
                }
            }
        }

        private static void CheckoutCart(List<CartItem> itemsToCheckout)
        {
            Console.WriteLine("\n Оформление заказов ");

            var pickupPoints = Program.db.PickupPoint.ToList();
            Console.WriteLine("Доступные пункты выдачи:");
            foreach (var pp in pickupPoints)
            {
                Console.WriteLine($"ID: {pp.Id} | Адрес: {pp.Address}");
            }

            Console.Write("Выберите ID пункта выдачи: ");
            if (!int.TryParse(Console.ReadLine(), out int ppId)) return;

            var selectedPvz = pickupPoints.FirstOrDefault(p => p.Id == ppId);
            if (selectedPvz == null)
            {
                Console.WriteLine("ПВЗ не найден.");
                return;
            }

            Order newOrder = new Order
            {
                UserId = Program.currentUser.Id,
                PickupPointId = selectedPvz.Id,
                OrderDate = DateTime.Now,
                TotalAmount = 0
            };
            Program.db.Order.Add(newOrder);
            Program.db.SaveChanges();

            decimal totalAmount = 0;

            foreach (var cartItem in itemsToCheckout)
            {
                var product = Program.db.Product.First(p => p.Id == cartItem.ProductId);

                OrderDetail detail = new OrderDetail
                {
                    OrderId = newOrder.Id,
                    ProductId = product.Id,
                    Quantity = cartItem.Quantity,
                    PriceAtPurchase = product.Price
                };
                Program.db.OrderDetail.Add(detail);
                totalAmount += product.Price * cartItem.Quantity;

                Program.db.CartItem.Remove(cartItem);
            }

            newOrder.TotalAmount = totalAmount;
            Program.db.SaveChanges();

            Console.WriteLine($"\nЗаказ №{newOrder.Id} успешно оформлен. Сумма: {totalAmount} руб. ПВЗ: {selectedPvz.Address}");
        }
    }
}