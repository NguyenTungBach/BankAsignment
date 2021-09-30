using System;
using BankAsignment.Controllers;
using BankAsignment.Entity;
using BankAsignment.View;

namespace BankAsignment
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("—— Ngan hang Spring Hero BAnk ——");
                Console.WriteLine("1. Đang ky tai khoan.");
                Console.WriteLine("2. Đang nhap he thong");
                Console.WriteLine("3. Exit");
                Console.WriteLine("==========================");
                Console.WriteLine("Please enter your choice (0-2)");
                var choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        while (true)
                        {
                            Console.WriteLine("Đang ky tai khoan: 1. Admin 2.User 0.Back");
                            var choiceRegister = Convert.ToInt32(Console.ReadLine());
                            switch (choiceRegister)
                            {
                                case 1:
                                    Console.WriteLine("Đang ky tai khoan Admin");
                                    AdminController adminController = new AdminController();
                                    adminController.CreateAdmin();
                                    break;
                                case 2:
                                    Console.WriteLine("Đang ky tai khoan User");   
                                    UserController userController = new UserController();
                                    userController.Register();
                                    break;
                            }
                            if (choiceRegister == 0)
                            {
                                Console.WriteLine("Back");    
                                break;
                            }
                        }
                        break;
                    case 2:
                        while (true)
                        {
                            Console.WriteLine("Đang nhap tai khoan: 1. Admin 2.User 0.Back");
                            var choiceRegister = Convert.ToInt32(Console.ReadLine());
                            switch (choiceRegister)
                            {
                                case 1:
                                    Console.WriteLine("Đang nhap tai khoan Admin");   
                                    AdminView adminView = new AdminView();
                                    adminView.GenerateAdminMenu();
                                    break;
                                case 2:
                                    Console.WriteLine("Đang nhap tai khoan User");    
                                    UserView userView = new UserView();
                                    userView.GenerateUserMenu();
                                    break;
                            }
                            if (choiceRegister == 0)
                            {
                                Console.WriteLine("Back");    
                                break;
                            }
                        }
                        break;
                }

                if (choice == 3)
                {
                    Console.WriteLine("Program Manage Bank Finnish\n");
                    break;
                }
            }
        }
    }
}