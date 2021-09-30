using BankAsignment.Entity;

namespace BankAsignment.Controllers
{
    public interface IAdminController
    {
        Admin CreateAdmin(); // 0.1  đăng ký Admin
        Admin Login(); // 0.2 đăng nhập Admin
        void ShowListUser(); // 1. danh sách người dùng
        void ShowListTransactionHistory(); // 2. danh sách lịch sử giao dịch
        Account FindUserByUserName(); // 3. tìm kiếm người dùng theo tên
        Account FindUserByAccountNumber(); // 4. tìm kiếm người dùng theo Account Number
        Account FindUserByPhoneNumber(); // 5. tìm kiếm người dùng theo Phone Number
        Account CreateNewAccount(); // 6. tạo người dùng
        void LockUnlockUser(); // 7. khóa và mở khóa người dùng
        void SearchTransactionHistoryByAccountNumber(); // 8. Tìm kiếm lịch sử giao dịch theo số tài khoản
        Admin UpdateInformation(Admin login); // 9. cập nhật thông tin cá nhân admin
        void CheckInformation(Admin login); // 9.1 (ngoài lề) Xem thông tin cá nhân của Account
        Admin UpdatePassword(Admin login); // 10. cập nhật thông tin cá nhân Admin
    }
}