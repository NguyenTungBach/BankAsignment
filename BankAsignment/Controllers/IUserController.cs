using BankAsignment.Entity;

namespace BankAsignment.Controllers
{
    public interface IUserController
    {
        Account Register(); // 0.1 đăng ký Account
        Account Login(); // 0.2 đăng nhập Account
        void Deposit(Account login);// 1. thực hiện gửi tiền
        void WithDraw(Account login,double loginBalance);// 2. thực hiện rút tiền
        void Transfer(Account login,double loginBalance);// 3. thực hiện chuyển tiền
        double CheckBalance(Account login); // 4. Truy vấn số dư
        Account UpdateInformation(Account login); // 5. Thay đổi thông tin cá nhân Account
        void CheckInformation(Account login); // 5.1 (ngoài lề) Xem thông tin cá nhân của Account
        Account UpdatePassword(Account login); // 6. Thay đổi mật khẩu Account
        void CheckTransactionHistory(Account login); // 7. Truy vấn lịch sử giao dịch

    }
}