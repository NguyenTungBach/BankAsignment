using System.Collections.Generic;
using BankAsignment.Entity;

namespace BankAsignment.Model
{
    public interface IAdminModel
    {
        Admin Save(Admin admin); // 0.1 đăng ký tài khoản Admin
        List<TransactionHistory> ListTransactionHistory(); //1. danh sách lịch sử giao dịch
        bool LockOrUnLock(string idAccount, int status); // 2. Khóa và mở khóa người dùng
        List<TransactionHistory> FindTransactionHistoryByAccountNumber(string accountNumber); // 3. danh sách lịch sử giao dịch theo Account Number
        Admin UpdateInfo(string id, Admin updateAccount); // 4.1 thay đổi thông tin cá nhân Admin
        Admin UpdatePassword(string id, Admin updateAccount); // 4.2 thay đổi mật khẩu Admin
        Admin FindByAccountNumber(string id); // 6. (ngoài lề) tìm admin theo Account Number
        Admin FindByUserName(string userName); // 7. (ngoài lề) tìm admin theo user name
    }
}