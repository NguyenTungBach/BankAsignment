using System;
using BankAsignment.Controllers;
using BankAsignment.Entity;

namespace BankAsignment.View
{
    public class UserView:IUserView
    {
        private IUserController userController;
        Account loggedInAccount;
        public UserView()
        {
            userController = new UserController();
        }
        public void GenerateUserMenu()
        {
            Account login = userController.Login();
            while (true) {
                Console.WriteLine("—— Ngan hang Spring Hero BAnk ——");
                Console.WriteLine("Chao mung “Xuan Hung” quay tro lai. Vui long chon thao tac.");
                Console.WriteLine("1. Gui tien");
                Console.WriteLine("2. Rut Tien");
                Console.WriteLine("3. Chuyen khoan");
                Console.WriteLine("4. Truy van so du");
                Console.WriteLine("5. Thay doi thong tin ca nhan");
                Console.WriteLine("6. Thay doi thong tin mat khau");
                Console.WriteLine("7. Truy van lich su giao dich");
                Console.WriteLine("8. Thoat");
                Console.WriteLine("==========================");
                Console.WriteLine("Please enter your choice (0-2)");
                var choice = Convert.ToInt32(Console.ReadLine());
                switch (choice){
                    case 1:
                        userController.Deposit(login); // 1. Gửi tiền
                        break;
                    case 2:
                        userController.WithDraw(login); // 2. Rút tiền
                        break;
                    case 3:
                        userController.Transfer(login); // 3. Chuyển tiền cho ai đó
                        break;
                    case 4:
                        userController.CheckBalance(login); // 4. Kiểm tra số dư trong tài khoản
                        break;
                    case 5:
                        userController.UpdateInformation(login); // 5. Thay đổi thông tin cá nhân
                        break;
                    case 6:
                        userController.UpdatePassword(login); // 6. Thay đổi thông tin mật khẩu
                        break;
                    case 7:
                        userController.CheckTransactionHistory(login); // 7. Truy vấn giao dịch (sao kê)
                        break;
                }
                if (choice == 8){ // Thoát
                    Console.WriteLine("Program Manage Account Finnish\n");
                    Environment.Exit(0);
                }
            }
        }
    }
}