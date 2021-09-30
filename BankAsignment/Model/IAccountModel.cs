using System;
using System.Collections.Generic;
using BankAsignment.Entity;

namespace BankAsignment.Model
{
    public interface IAccountModel
    {
        Account Save(Account account); // 0.1 đăng ký tài khoản
        TransactionHistory Deposit(TransactionHistory deposit); // 1. thực hiện gửi tiền
        TransactionHistory Withdraw(TransactionHistory WithDraw); // 2. thực hiện rút tiền
        TransactionHistory Transfer(TransactionHistory WithDraw); // 3. thực hiện chuyển tiền
        Account FindByAccountNumber(string accountNumber); // 4.1 tìm tài khoản theo số tài khoản, truy vấn số dư
        Account FindByUserName(string username); // 4.2 tìm tài khoản theo username
        Account FindByPhoneNumber(string phone); // 4.3 tìm tài khoản theo username
        Account UpdateInfo(string id, Account updateAccount);  // 5. thay đổi thông tin cá nhân
        Account UpdatePassword(string id, Account updateAccount); // 6. thay đổi mật khẩu
        List<Account> FindAllAccounts(); //7. Tìm tất cả Account
        List<TransactionHistory> FindTransactionHistoryByAccountNumber(string accountNumber); //8. Sao kê theo Account Number
    }
}