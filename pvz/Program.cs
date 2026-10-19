using GMWOGMarketplace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pvz
{
    internal class Program
    {
        public static pvzEntities db = new pvzEntities();
        public static User currentUser = null;

        static void Main(string[] args)
        {
            bool running = true;
            while (running)
            {
                Console.WriteLine("\n Маркетплейс GMWOG ");
                Console.WriteLine("1. Просмотр товаров");

                if (currentUser == null)
                {
                    Console.WriteLine("2. Вход");
                    Console.WriteLine("3. Регистрация");
                }
                else
                {
                    Console.WriteLine($"Вы вошли как: {currentUser.Nickname}");
                    Console.WriteLine("2. Корзина");
                    Console.WriteLine("3. История заказов");
                    Console.WriteLine("4. Выйти");
                }

                Console.WriteLine("0. Выход");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();

                if (choice == "1") ProductService.ShowProducts();
                else if (choice == "2")
                {
                    if (currentUser == null) AuthService.Login();
                    else CartService.ManageCart();
                }
                else if (choice == "3")
                {
                    if (currentUser == null) AuthService.Register();
                    else OrderService.ShowHistory();
                }
                else if (choice == "4" && currentUser != null)
                {
                    currentUser = null;
                    Console.WriteLine("Вы вышли из аккаунта.");
                }
                else if (choice == "0") running = false;
                else Console.WriteLine("Неверный пункт.");
            }
        }
    }
}