using System;
using System.Collections.Generic;
using BankAsignment.Entity;
using BankAsignment.Model;
using BankAsignment.Until;

namespace BankAsignment.Controllers
{
    public class UserController:IUserController
    {
        private IAccountModel _accountModel;
        public UserController()
        {
            _accountModel = new AccountModel();
        }
        //////////////////////////////////////////////////////  
        //1. Input Start
        //1.1 Register Input Start
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
        //1.1 Register Input End
        
        //1.2 Login Input Start
        private Account GetLoginInformation()
        {
            Account account = new Account();
            Console.WriteLine("Please enter username: ");
            account.UserName = Console.ReadLine();
            Console.WriteLine("Please enter password: ");
            account.Password = Console.ReadLine();
            return account;
        }
        //1.2 Login Input End
        
        //1.3 UpdateInfo Input Start
        private Account GetAccountUpdateInfo()
        {
            Account account = new Account();
            Console.WriteLine("Please enter username: ");
            account.UserName = Console.ReadLine();
            Console.WriteLine("Please phone number: ");
            account.Phone = Console.ReadLine();
            return account;
        }
        //1.3 UpdateInfo Input End
        
        //1.4 UpdateInfo Input Start
        private Account GetAccountUpdatePassword()
        {
            Account account = new Account();
            Console.WriteLine("Please enter password: ");
            account.Password = Console.ReadLine();
            Console.WriteLine("Please enter password confirm: ");
            account.PasswordConfirm = Console.ReadLine();
            return account;
        }
        //1.4 UpdateInfo Input End
        
        //1.5 Get Deposit Transaction Info Input Start
        private TransactionHistory GetTransactionInfo()
        {
            TransactionHistory transactionHistory = new TransactionHistory();
            Console.WriteLine("Please enter amount>=10000: ");
            double amount = Convert.ToDouble(Console.ReadLine());
            while (true)
            {
                if (amount<10000)
                {
                    Console.WriteLine("Wrong, please enter amount >=10000: ");
                }
                else
                {
                    transactionHistory.Amount = amount;
                    break;
                }
            }
            return transactionHistory;
        }
        //1.5 Get Deposit Transaction Info Input End
        
        //1.6 Get Transfer Transaction Info Input Start
        private TransactionHistory GetTransferTransactionInfo()
        {
            TransactionHistory transactionHistory = new TransactionHistory();
            Console.WriteLine("Please enter amount>=10000: ");
            double amount = Convert.ToDouble(Console.ReadLine());
            while (true)
            {
                if (amount<10000)
                {
                    Console.WriteLine("Wrong, please enter amount >=10000: ");
                }
                else
                {
                    transactionHistory.Amount = amount;
                    break;
                }
            }
            Console.WriteLine("Please enter ReceiverAccountNumber: ");
            string receiverAccountNumber = Console.ReadLine();
            while (true)
            {
                if (!CheckExistAccountNumber(receiverAccountNumber)) //2.2 Check Account Number Existed
                {
                    Console.WriteLine("Receiver Account Number are not existed! ");
                }
                else
                {
                    Console.WriteLine("Found Receiver Account Number! ");
                    transactionHistory.ReceiverAccountNumber = receiverAccountNumber;
                    break;
                }
            }
            return transactionHistory;
        }
        //1.6 Get Transfer Transaction Info Input Start
        // Input End
        
        //2. Check Exist Start
        //2.1 Check User Existed Start
        private bool CheckExistUser(string username)
        {
            return _accountModel.FindByUserName(username) != null; // kiểm tra sự tồn tại của username
        }
        //2.1 Check User Existed Start
        
        //2.2 Check Account Number Existed Start
        private bool CheckExistAccountNumber(string accountNumber)
        {
            return _accountModel.FindByAccountNumber(accountNumber) != null; // kiểm tra sự tồn tại của Account Number
        }
        //2.2 Check Account Number Existed End
        
        //2.3 Check Phone Number Existed Start
        private bool CheckExistPhoneNumber(string phone)
        {
            return _accountModel.FindByPhoneNumber(phone) != null; // kiểm tra sự tồn tại của Phone Number
        }
        //2.3 Check Phone Number Existed End
        
        //2.4 Check Check Balance Is OK Existed Start
        private bool CheckBalanceIsOK(double amount,double loginBalance)
        {
            Console.WriteLine($"{loginBalance} - {amount} = {loginBalance - amount}");
            return (loginBalance - amount) > 30000;
        }
        //2.4 Check Check Balance Is OK Existed End
        // Check Exist End
        //////////////////////////////////////////////////////  
        
        // 0.1 đăng ký Account
        public Account Register()
        {
            //1. Nhập dữ liệu
            Account account;
            bool isValid; // default false
            do
            {
                account = GetAccountInformation(); //1.1 Register Input
                //2. Validate dữ liệu
                Dictionary<string, string> errors = account.CheckValid();
                // Check unique username
                if (CheckExistUser(account.UserName)) // nếu user name tồn tại (//2.1 Check User Existed)
                {
                    errors.Add("username_duplicate","Duplicate username, please choose another!");
                }
                // Check phone number
                if (CheckExistPhoneNumber(account.Phone)) //2.3 Check Phone Number Existed
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
            if (CheckExistAccountNumber(account.AccountNumber))
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
            Console.WriteLine("Register success!");
            return result;
        }

        // 0.2 đăng nhập Account
        public Account Login()
        {
            bool isValid;
            Account account = null;
            do
            {
                do
                {
                    account = GetLoginInformation(); //1.2 Login Input
                    Dictionary<string, string> errors = account.CheckValidLogin();
                    // Check exist username
                    if (!CheckExistUser(account.UserName)) // nếu user name không tồn tại (//2.1 Check User Existed)
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
                Account existingAccount = _accountModel.FindByUserName(account.UserName);
                if (existingAccount!=null && HashUtil.ComparePasswordHash(account.Password,existingAccount.Salt,existingAccount.PasswordHash) && existingAccount.Status != 0)
                {
                    Console.WriteLine("Login Success!\n");
                    return existingAccount;
                } else if (existingAccount.Status == 0)
                {
                    Console.WriteLine("This account has been lock, please call admin to help\n");
                }
                else
                {
                    Console.WriteLine("Login Fails!\n");
                }
            } while (true);
        }
        
        // 1. thực hiện gửi tiền
        public void Deposit(Account login)
        {
            //1. Nhập dữ liệu
            TransactionHistory transactionHistory= GetTransactionInfo(); //1.5 Get Deposit Transaction Info Input
            // Check unique id
            if (CheckExistAccountNumber(transactionHistory.Id))
            {
                transactionHistory.GenerateAccountNumber();
            }
            //2. Deposit type transaction History
            transactionHistory.Type = 1; // Deposit
            transactionHistory.SenderAccountNumber = login.AccountNumber;
            transactionHistory.ReceiverAccountNumber = login.AccountNumber;
            //3. Save vào database
            var result = _accountModel.Deposit(transactionHistory);
            if (result == null)
            {
                Console.WriteLine("Deposit is not success!");
            }
            else
            {
                Console.WriteLine("Deposit success!");
            }
        }

        // 2. thực hiện rút tiền
        public void WithDraw(Account login,double loginBalance)
        {
            //1. Nhập dữ liệu
            TransactionHistory transactionHistory;
            bool isValid; // default false
            do
            {
                transactionHistory = GetTransactionInfo(); //1.5 Get Deposit Transaction Info Input
                //2. Validate dữ liệu
                Dictionary<string, string> errors = new Dictionary<string, string>();
                // nếu tiền mà dưới 30.000 thì không được rút hoặc gửi. Để còn hoạt động tiền trong ngân hàn
                if (!CheckBalanceIsOK(transactionHistory.Amount,loginBalance)) //2.4 Check Check Balance Is OK Existed
                {
                    errors.Add("balanceIsNotEnough","Balance < 30000 => Is Not Enough, Please Deposit!");
                }
                isValid = errors.Count == 0;
                // Trong trường hợp nhập thông tin lỗi thì in ra thông báo
                if (!isValid)
                {
                    foreach (var error in errors)
                    {
                        Console.WriteLine(error.Value);
                    }
                    Console.WriteLine("Please return Transaction information!\n");
                }
            } while (!isValid); // chừng nào còn lỗi thì quay lại nhập
            //2. Deposit type transaction History
            
            // Check unique id
            if (CheckExistAccountNumber(transactionHistory.Id))
            {
                transactionHistory.GenerateAccountNumber();
            }
            transactionHistory.Type = 2; // WithDraw
            transactionHistory.SenderAccountNumber = login.AccountNumber;
            transactionHistory.ReceiverAccountNumber = login.AccountNumber;
            //3. Save vào database
            var result = _accountModel.Withdraw(transactionHistory);
            if (result == null)
            {
                Console.WriteLine("Withdraw is not success!");
            }
            else
            {
                Console.WriteLine("Withdraw success!");    
            }
        }

        // 3. thực hiện chuyển tiền
        public void Transfer(Account login,double loginBalance)
        {
            //1. Nhập dữ liệu
            TransactionHistory transactionHistory;
            bool isValid; // default false
            do
            {
                transactionHistory = GetTransferTransactionInfo(); //1.6 Get Transfer Transaction Info Input
                //2. Validate dữ liệu
                Dictionary<string, string> errors = new Dictionary<string, string>();
                // nếu tiền mà dưới 30.000 thì không được rút hoặc gửi. Để còn hoạt động tiền trong ngân hàn
                if (!CheckBalanceIsOK(transactionHistory.Amount,loginBalance)) //2.4 Check Check Balance Is OK Existed
                {
                    errors.Add("balanceIsNotEnough","Balance < 30000 => Is Not Enough, Please Deposit!");
                }
                isValid = errors.Count == 0;
                // Trong trường hợp nhập thông tin lỗi thì in ra thông báo
                if (!isValid)
                {
                    foreach (var error in errors)
                    {
                        Console.WriteLine(error.Value);
                    }
                    Console.WriteLine("Please return Transaction information!\n");
                }
            } while (!isValid); // chừng nào còn lỗi thì quay lại nhập
            //2. Deposit type transaction History
            
            // Check unique id
            if (CheckExistAccountNumber(transactionHistory.Id))
            {
                transactionHistory.GenerateAccountNumber();
            }
            transactionHistory.Type = 3; // Transfer
            transactionHistory.SenderAccountNumber = login.AccountNumber;
            //3. Save vào database
            var result = _accountModel.Transfer(transactionHistory);
            if (result == null)
            {
                Console.WriteLine("Transfer is not success!");
            }
            else
            {
                Console.WriteLine("Transfer success!");    
            }
        }
        
        // 4. Truy vấn số dư
        public double CheckBalance(Account login)
        {
            Account account = _accountModel.FindByAccountNumber(login.AccountNumber);
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("Balance (so du) = {0}" ,account.Balance);
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");
            return account.Balance;
        }

        // 5. Thay đổi thông tin cá nhân Account
        public Account UpdateInformation(Account login)
        {
            CheckInformation(login);
            //1. nhập thông tin cần update
            Console.WriteLine("Update account information!");
            bool isValid;
            Account account = null;
            do
            {
                account = GetAccountUpdateInfo(); //1.3 UpdateInfo Input
                Dictionary<string, string> errors = new Dictionary<string, string>();
                if (string.IsNullOrEmpty(account.UserName))
                {
                    errors.Add("username","User name can not be null or empty");
                }
                // Check unique username
                if (CheckExistUser(account.UserName)) // nếu user name tồn tại (//2.1 Check User Existed)
                {
                    errors.Add("username_duplicate","Duplicate username, please choose another!");
                }
                // Check phone number
                if (CheckExistPhoneNumber(account.Phone)) //2.3 Check Phone Number Existed
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
                    Console.WriteLine("Please return account update information!\n");
                }
            } while (!isValid); // chừng nào còn lỗi thì quay lại nhập
            //2.1 Ngày Update
            account.UpdateAt = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
            //3. Save vào database
            var result = _accountModel.UpdateInfo(login.AccountNumber,account);
            if (result == null)
            {
                return null;
            }
            Console.WriteLine("Update success!\n");
            return result;
        }
        
        // 5.1 (ngoài lề) Xem thông tin cá nhân của Account
        public void CheckInformation(Account login)
        {
            Account account = _accountModel.FindByAccountNumber(login.AccountNumber);
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine(account.ToString());
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");
        }

        // 6. Thay đổi mật khẩu Account
        public Account UpdatePassword(Account login)
        {
            //1. nhập thông tin cần update
            Console.WriteLine("Update account information!");
            bool isValid;
            Account account = null;
            do
            {
                account = GetAccountUpdatePassword(); //1.4 UpdateInfo Input
                Dictionary<string, string> errors = new Dictionary<string, string>();
                if (string.IsNullOrEmpty(account.Password))
                {
                    errors.Add("password","Password can not be null or empty");
                }
                if (!account.Password.Equals(account.PasswordConfirm))
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
            account.UpdateAt = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
            //3. Mã hóa
            account.EncryptPassword();
            //4. Save vào database
            var result = _accountModel.UpdatePassword(login.AccountNumber,account);
            if (result == null)
            {
                return null;
            }
            Console.WriteLine("Update Password success!\n");
            return result;
        }

        // 7. Truy vấn lịch sử giao dịch
        public void CheckTransactionHistory(Account login)
        {
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("Show Transaction History information");
            List<TransactionHistory> list = _accountModel.FindTransactionHistoryByAccountNumber(login.AccountNumber);
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
}