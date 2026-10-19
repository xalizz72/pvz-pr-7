using pvz;
using System;
using System.Linq;

namespace GMWOGMarketplace
{
    public static class AuthService
    {
        public static void Register()
        {
            Console.WriteLine("\n Регистрация ");
            Console.Write("Введите никнейм: ");
            string nick = Console.ReadLine();

            if (Program.db.User.Any(u => u.Nickname == nick))
            {
                Console.WriteLine("Этот никнейм уже занят.");
                return;
            }
            else
            {
                Console.Write("Введите пароль: ");
                string pass1 = Console.ReadLine();
                Console.Write("Повторите пароль: ");
                string pass2 = Console.ReadLine();

                if (pass1 != pass2)
                {
                    Console.WriteLine("Пароли не совпадают");
                    return;
                }

                User newUser = new User { Nickname = nick, Password = pass1 };
                Program.db.User.Add(newUser);
                Program.db.SaveChanges();

                Console.WriteLine("Регистрация успешна. Теперь вы можете войти.");
            }
        }

        public static void Login()
        {
            Console.WriteLine("\n Вход ");
            Console.Write("Введите никнейм: ");
            string nick = Console.ReadLine();
            Console.Write("Введите пароль: ");
            string pass = Console.ReadLine();

            var user = Program.db.User.FirstOrDefault(u => u.Nickname == nick && u.Password == pass);

            if (user != null)
            {
                Program.currentUser = user;
                Console.WriteLine($"Добро пожаловать, {user.Nickname}!");
            }
            else
            {
                Console.WriteLine("Неверный никнейм или пароль.");
            }
        }
    }
}