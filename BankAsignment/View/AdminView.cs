using System;
using BankAsignment.Controllers;
using BankAsignment.Entity;

namespace BankAsignment.View
{
    public class AdminView:IAdminView
    {
        private IAdminController adminController;

        public AdminView()
        {
            adminController = new AdminController();
        }

        public void GenerateAdminMenu()
        {
            Admin login = adminController.Login();
            while (true) {
                Console.WriteLine("—— Ngan hang Spring Hero BAnk ——");
                Console.WriteLine("Chao mung “Xuan Hung” quay tro lai. Vui long chon thao tac.");
                Console.WriteLine("1. Danh sach nguoi dung");
                Console.WriteLine("2. Danh sach lich su giao dich");
                Console.WriteLine("3. Tim kiem nguoi dung theo ten");
                Console.WriteLine("4. Tim kiem nguoi dung theo so tai khoan");
                Console.WriteLine("5. Tim kiem nguoi dung theo so dien thoai");
                Console.WriteLine("6. Them nguoi dung moi");
                Console.WriteLine("7. Khoa va mo khoa tai khoan nguoi dung");
                Console.WriteLine("8. Tim kiem lich su giao dich theo so tai khoan");
                Console.WriteLine("9. Thay doi thong tin tai khoan");
                Console.WriteLine("10. Thay doi thong tin mat khau");
                Console.WriteLine("12. Danh sach lich su giao dich co' phan trang voi limit la 2");
                Console.WriteLine("11. Thoat");
                Console.WriteLine("==========================");
                var choice = Convert.ToInt32(Console.ReadLine());
                switch (choice){
                    case 1:
                        adminController.ShowListUser(); // 1. Danh sách người dùng
                        break;
                    case 2:
                        adminController.ShowListTransactionHistory(); // 2. Danh sách lịch sử giao dịch
                        break;
                    case 3:
                        adminController.FindUserByUserName(); // 3. Tìm kiếm người dùng theo tên
                        break;
                    case 4:
                        adminController.FindUserByAccountNumber(); // 4. Tìm kiếm người dùng theo số tài khoản
                        break;
                    case 5:
                        adminController.FindUserByPhoneNumber(); // 5. Tìm kiếm người dùng theo số điện thoại
                        break;
                    case 6:
                        adminController.CreateNewAccount(); // 6. Thêm người dùng mới
                        break;
                    case 7:
                        adminController.LockUnlockUser(); // 7. Khóa và mở tài khoản người dùng
                        break;
                    case 8:
                        adminController.SearchTransactionHistoryByAccountNumber(); // 8. Tìm kiếm lịch sử giao dịch theo số tài khoản
                        break;
                    case 9:
                        adminController.UpdateInformation(login); // 9. Thay đổi thông tin tài khoản admin
                        break;
                    case 10:
                        adminController.UpdatePassword(login); // 10. Thay đổi thông tin mật khẩu admin
                        break;
                    case 12:
                        adminController.ShowListTransactionHistoryByCondition();; // 10. Thay đổi thông tin mật khẩu admin
                        break;
                }
                if (choice == 11){ // Thoát
                    Console.WriteLine("Program Manage Account Finnish\n");
                    Environment.Exit(0);
                }
            }
        }
    }
}