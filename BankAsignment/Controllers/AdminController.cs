using System;
using System.Collections.Generic;
using BankAsignment.Entity;
using BankAsignment.Model;
using BankAsignment.Until;

namespace BankAsignment.Controllers
{
    public class AdminController:IAdminController
    {
        private IAdminModel _adminModel;
        private IAccountModel _accountModel;
        private static int lockByCount;
        public AdminController()
        {
            _adminModel = new AdminModel();
            _accountModel = new AccountModel();
        }
        //////////////////////////////////////////////////////
        //1. Input Start
        //1.1 Register Input Start
        private Admin GetAdminInformation()
        {
            var admin = new Admin();
            Console.WriteLine("Please enter username: ");
            admin.UserName = Console.ReadLine();
            Console.WriteLine("Please enter password: ");
            admin.Password = Console.ReadLine();
            Console.WriteLine("Please enter password confirm: ");
            admin.PasswordConfirm = Console.ReadLine();
            return admin;
        }
        //1.1 Register Input End
        
        //1.2 Login Input Start
        private Admin GetLoginInformation()
        {
            Admin admin = new Admin();
            Console.WriteLine("Please enter username: ");
            admin.UserName = Console.ReadLine();
            Console.WriteLine("Please enter password: ");
            admin.Password = Console.ReadLine();
            return admin;
        }
        //1.2 Login Input End
        
        //1.3 UpdateInfo Input Start
        private Admin GetAccountUpdateInfo()
        {
            Admin admin = new Admin();
            Console.WriteLine("Please enter username: ");
            admin.UserName = Console.ReadLine();
            return admin;
        }
        //1.3 UpdateInfo Input End
        
        //1.4 UpdateInfo Input Start
        private Admin GetAccountUpdatePassword()
        {
            Admin admin = new Admin();
            Console.WriteLine("Please enter password: ");
            admin.Password = Console.ReadLine();
            Console.WriteLine("Please enter password confirm: ");
            admin.PasswordConfirm = Console.ReadLine();
            return admin;
        }
        //1.4 UpdateInfo Input End
        
        //1.5 Create Account Input Start
        private Account GetAccountInformation()
        {
            var account = new Account();
            Console.WriteLine("Please enter username: ");
            account.UserName = Console.ReadLine();
            Console.WriteLine("Please phone number: ");
            account.Phone = Console.ReadLine();
            Console.WriteLine("Please enter password: ");
            account.Password = Console.ReadLine();
            Console.WriteLine("Please enter password confirm: ");
            account.PasswordConfirm = Console.ReadLine();
            return account;
        }
        //1.5 Create Account Input End
        // Input End
        
        //2. Check Exist Start
            // Check Admin Start
        //2.1 Check User Admin Existed Start
        private bool CheckExistUser(string username)
        {
            return _adminModel.FindByUserName(username) != null; // kiểm tra sự tồn tại của username
        }
        //2.1 Check User Admin Existed Start
        
        //2.2 Check Account Number Admin Existed Start
        private bool CheckExistAdminNumber(string accountNumber)
        {
            return _adminModel.FindByAccountNumber(accountNumber) != null; // kiểm tra sự tồn tại của Account Number
        }
        //2.2 Check Account Number Admin Existed Start
            // Check Admin End
        
            // Check Account Start
        //2.3 Check Account Number Existed Start
        private bool CheckExistAccountNumber(string accountNumber)
        {
            return _accountModel.FindByAccountNumber(accountNumber) != null; // kiểm tra sự tồn tại của Account Number
        }
        //2.3 Check Account Number Existed Start
        
        //2.4 Check Phone Number Existed Start
        private bool CheckExistPhoneNumber(string phone)
        {
            return _accountModel.FindByPhoneNumber(phone) != null; // kiểm tra sự tồn tại của Phone Number
        }
        //2.4 Check Phone Number Existed End
            // Check Account End
        // Check Exist End
        //////////////////////////////////////////////////////  
        
        // 0.1  đăng ký Admin
        public Admin CreateAdmin()
        {
            //1. Nhập dữ liệu
            Admin admin;
            bool isValid; // default false
            do
            {
                admin = GetAdminInformation(); //1.1 Register Input
                //2. Validate dữ liệu
                Dictionary<string, string> errors = admin.CheckValid();
                // Check unique username
                if (CheckExistUser(admin.UserName)) //2.1 Check User Admin Existed
                {
                    errors.Add("username_duplicate","Duplicate username, please choose another!");
                }
                isValid = errors.Count == 0;
                // Trong trường hợp nhập thông tin lỗi thì in ra thông báo
                if (!isValid)
                {
                    foreach (var error in errors)
                    {
                        Console.WriteLine(error.Value);
                    }
                    Console.WriteLine("Please return account information!\n");
                }
            } while (!isValid); // chừng nào còn lỗi thì quay lại nhập
            // Check unique account number
            if (CheckExistAdminNumber(admin.AccountNumber)) //2.2 Check Account Number Admin Existed
            {
                admin.GenerateAccountNumber();
            }
            //2. Mã hóa
            admin.EncryptPassword();
            //3. Save vào database
            var result = _adminModel.Save(admin);
            if (result == null)
            {
                return null;
            }
            Console.WriteLine("Register success!");
            return result;
        }
        
        // 0.2 đăng nhập Admin
        public Admin Login()
        {
            bool isValid;
            Admin admin;
            do
            {
                do
                {
                    admin = GetLoginInformation(); //1.2 Login Input
                    Dictionary<string, string> errors = admin.CheckValidLogin();
                    // Check exist username
                    if (!CheckExistUser(admin.UserName)) // nếu user name không tồn tại (//2.1 Check User Admin Existed)
                    {
                        errors.Add("username_not_existed","Username are not existed, please check again!");
                    }
                    isValid = errors.Count == 0;
                    // Trong trường hợp nhập thông tin lỗi thì in ra thông báo
                    if (!isValid)
                    {
                        foreach (var error in errors)
                        {
                            Console.WriteLine(error.Value);
                        }
                        Console.WriteLine("Please return account information!\n");
                    }
                } while (!isValid);
                Admin existingAccount = _adminModel.FindByUserName(admin.UserName);
                if (existingAccount!=null && HashUtil.ComparePasswordHash(admin.Password,existingAccount.Salt,existingAccount.PasswordHash) && existingAccount.Status != 0)
                {
                    Console.WriteLine("Login Success!\n");
                    return existingAccount;
                } else if (existingAccount.Status == 0)
                {
                    Console.WriteLine("This account admin has been lock\n");
                }
                else
                {
                    Console.WriteLine("Login Fails!\n");
                }
            } while (true);
        }

        // 1. danh sách người dùng
        public void ShowListUser()
        {
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("Show account information");
            List<Account> list = _accountModel.FindAllAccounts();
            for (int i = 0; i < list.Count; i++)
            {
                Account account = list[i];
                Console.WriteLine("================================");
                Console.WriteLine(account.ToString());
                Console.WriteLine("================================");
            }
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");

        }
        
        // 2. danh sách lịch sử giao dịch
        public void ShowListTransactionHistory()
        {
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("Show Transaction History information");
            List<TransactionHistory> list = _adminModel.ListTransactionHistory();
            for (int i = 0; i < list.Count; i++)
            {
                TransactionHistory transaction = list[i];
                Console.WriteLine("================================");
                Console.WriteLine(transaction.ToString());
                Console.WriteLine("================================");
            }
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");
        }

        // 3. tìm kiếm người dùng theo tên
        public Account FindUserByUserName()
        {
            Console.WriteLine("Find User By User Name");
            Console.WriteLine("Enter User Name");
            string userName = Console.ReadLine();
            Account account = _accountModel.FindByUserName(userName);
            if (account != null)
            {
                Console.WriteLine("Found Account existed");
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine(account.ToString());
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");
                return account;
            }
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("Not found Account existed");
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");
            return null;
        }
        
        // 4. tìm kiếm người dùng theo Account Number
        public Account FindUserByAccountNumber()
        {
            Console.WriteLine("Find User By Account Number");
            Console.WriteLine("Enter Account Number");
            string accountNumber = Console.ReadLine();
            Account account = _accountModel.FindByAccountNumber(accountNumber);
            if (account != null)
            {
                Console.WriteLine("Found Account existed");
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine(account.ToString());
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");
                return account;
            }
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("Not found Account existed");
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");
            return null;
        }
        
        // 5. tìm kiếm người dùng theo Phone Number
        public Account FindUserByPhoneNumber()
        {
            Console.WriteLine("Find User By Phone Number");
            Console.WriteLine("Enter Phone Number");
            string phoneNumber = Console.ReadLine();
            Account account = _accountModel.FindByPhoneNumber(phoneNumber);
            if (account != null)
            {
                Console.WriteLine("Found Account existed");
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine(account.ToString());
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");
                return account;
            }
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("Not found Account existed");
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");
            return null;
        }
        
        // 6. tạo người dùng
        public Account CreateNewAccount()
        {
            //1. Nhập dữ liệu
            Account account;
            bool isValid; // default false
            do
            {
                account = GetAccountInformation(); //1.5 Create Account Input
                //2. Validate dữ liệu
                Dictionary<string, string> errors = account.CheckValid();
                // Check unique username
                if (CheckExistUser(account.UserName)) // nếu user name tồn tại (//2.1 Check User Admin Existed)
                {
                    errors.Add("username_duplicate","Duplicate username, please choose another!");
                }
                // Check phone number
                if (CheckExistPhoneNumber(account.Phone)) //2.4 Check Phone Number Existed
                {
                    errors.Add("phone","Duplicate phone, please choose another!");
                }
                isValid = errors.Count == 0;
                // Trong trường hợp nhập thông tin lỗi thì in ra thông báo
                if (!isValid)
                {
                    foreach (var error in errors)
                    {
                        Console.WriteLine(error.Value);
                    }
                    Console.WriteLine("Please return account information!\n");
                }
            } while (!isValid); // chừng nào còn lỗi thì quay lại nhập
            // Check unique account number
            if (CheckExistAccountNumber(account.AccountNumber)) //2.3 Check Account Number Existed
            {
                account.GenerateAccountNumber();
            }
            //2. Mã hóa
            account.EncryptPassword();
            //3. Save vào database
            var result = _accountModel.Save(account);
            if (result == null)
            {
                return null;
            }
            Console.WriteLine("Register Account success!");
            return result;
        }
        
        // 7. khóa và mở khóa người dùng
        public void LockUnlockUser()
        {
            string accountNumber;
            int status;
            Console.WriteLine("Lock Or Unlock account!");
            while (true)
            {
                Console.WriteLine("Enter account Number");
                accountNumber = Console.ReadLine();
                if (_accountModel.FindByAccountNumber(accountNumber) == null)
                {
                    Console.WriteLine("account Number are not existed");
                }
                else
                {
                    Console.WriteLine("Find account Number existed");
                    break;
                }
            }
            while (true)
            {
                Console.WriteLine("Enter status Number: 0.Lock 1.Unlock"); 
                status = Convert.ToInt32(Console.ReadLine());
                if (status<0 || status>1)
                {
                    Console.WriteLine("Enter Wrong please Enter status Number: 0.Lock 1.Unlock");
                }
                else
                {
                    break;
                }
            }

            if (status == 0 && _adminModel.LockOrUnLock(accountNumber,status) )
            {
                Console.WriteLine("Lock Success");
            } else if (status == 1 && _adminModel.LockOrUnLock(accountNumber,status))
            {
                Console.WriteLine("UnLock Success");
            }
        }

        // 8. Tìm kiếm lịch sử giao dịch theo số tài khoản
        public void SearchTransactionHistoryByAccountNumber()
        {
            Console.WriteLine("Enter Account Number");
            string accountNumber = Console.ReadLine();
            if (_accountModel.FindByAccountNumber(accountNumber)==null)
            {
                Console.WriteLine("Account Number are not existed\n");
            }
            else
            {
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("Show Account Transaction History information");
                List<TransactionHistory> list = _adminModel.FindTransactionHistoryByAccountNumber(accountNumber);
                for (int i = 0; i < list.Count; i++)
                {
                    TransactionHistory transaction = list[i];
                    Console.WriteLine("================================");
                    Console.WriteLine(transaction.ToString());
                    Console.WriteLine("================================");
                }
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");
            }
        }

        // 9. cập nhật thông tin cá nhân admin
        public Admin UpdateInformation(Admin login)
        {
            CheckInformation(login);
            //1. nhập thông tin cần update
            Console.WriteLine("Update account information!");
            bool isValid;
            Admin admin = null;
            do
            {
                admin = GetAccountUpdateInfo(); //1.3 UpdateInfo Input
                Dictionary<string, string> errors = new Dictionary<string, string>();
                if (string.IsNullOrEmpty(admin.UserName))
                {
                    errors.Add("username","User name can not be null or empty");
                }
                // Check unique username
                if (CheckExistUser(admin.UserName)) // nếu user name tồn tại (//2.1 Check User Admin Existed)
                {
                    errors.Add("username_duplicate","Duplicate username, please choose another!");
                }
                isValid = errors.Count == 0;
                // Trong trường hợp nhập thông tin lỗi thì in ra thông báo
                if (!isValid)
                {
                    foreach (var error in errors)
                    {
                        Console.WriteLine(error.Value);
                    }
                    Console.WriteLine("Please return account update information!\n");
                }
            } while (!isValid); // chừng nào còn lỗi thì quay lại nhập
            //2.1 Ngày Update
            admin.UpdateAt = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
            //3. Save vào database
            var result = _adminModel.UpdateInfo(login.AccountNumber,admin);
            if (result == null)
            {
                return null;
            }
            Console.WriteLine("Update success!\n");
            return result;
        }
        
        // 9.1 (ngoài lề) Xem thông tin cá nhân của Account
        public void CheckInformation(Admin login)
        {
            Admin admin = _adminModel.FindByAccountNumber(login.AccountNumber);
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine(admin.ToString());
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");
        }
        
        // 10. cập nhật thông tin cá nhân Admin
        public Admin UpdatePassword(Admin login)
        {
            //1. nhập thông tin cần update
            Console.WriteLine("Update account information!");
            bool isValid;
            Admin admin = null;
            do
            {
                admin = GetAccountUpdatePassword(); //1.4 UpdateInfo Input
                Dictionary<string, string> errors = new Dictionary<string, string>();
                if (string.IsNullOrEmpty(admin.Password))
                {
                    errors.Add("password","Password can not be null or empty");
                }
                if (!admin.Password.Equals(admin.PasswordConfirm))
                {
                    errors.Add("passwordComfirm","Password and Confirm Password are not matched!");
                }
                isValid = errors.Count == 0;
                // Trong trường hợp nhập thông tin lỗi thì in ra thông báo
                if (!isValid)
                {
                    foreach (var error in errors)
                    {
                        Console.WriteLine(error.Value);
                    }
                    Console.WriteLine("Please return account update information!\n");
                }
            } while (!isValid); // chừng nào còn lỗi thì quay lại nhập
            //2.1 Ngày Update
            admin.UpdateAt = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
            //3. Mã hóa
            admin.EncryptPassword();
            //4. Save vào database
            var result = _adminModel.UpdatePassword(login.AccountNumber,admin);
            if (result == null)
            {
                return null;
            }
            Console.WriteLine("Update Password success!\n");
            return result;
        }
    }
}