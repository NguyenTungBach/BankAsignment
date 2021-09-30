using System.Collections.Generic;
using BankAsignment.Entity;
using BankAsignment.Until;
using MySql.Data.MySqlClient;

namespace BankAsignment.Model
{
    public class AdminModel:IAdminModel
    {
        private static readonly string SaveCommand = "insert into admins (accountNumber, userName, passwordHash, salt,status,createAt,updateAt,deleteAt) values (@accountNumber,@userName,@passwordHash,@salt,@status,@createAt,@updateAt,@deleteAt)";
        private static readonly string UpdateInfoCommand = "update admins set userName = @userName, updateAt = @updateAt where accountNumber = @accountNumber";
        private static readonly string UpdatePasswordCommand = "update admins set passwordHash = @passwordHash, salt = @salt, updateAt = @updateAt where accountNumber = @accountNumber";
        private static readonly string SelectByAdminCommand = "select * from admins where accountNumber = @accountNumber";
        private static readonly string SelectByAdminUserNameCommand = "select * from admins where userName = @userName";

        private static readonly string LockOrUnlockCommand = "update accounts set status = @status where account_number = @account_number";
        
        private static readonly string FindAllTransactionHistoryCommand = "select * from transaction_history";
        private static readonly string FindAllAccountTransactionCommand = "select * from transaction_history where senderAccountNumber = @senderAccountNumber OR receiverAccountNumber = @senderAccountNumber";
        
        // 0.1 đăng ký tài khoản Admin
        public Admin Save(Admin admin)
        {
            using (var cnn = ConnectionHelper.getConnection())
            {
                cnn.Open();
                MySqlCommand cmd = cnn.CreateCommand();
                cmd.CommandText = SaveCommand;
                cmd.Parameters.AddWithValue("@accountNumber", admin.AccountNumber);
                cmd.Parameters.AddWithValue("@userName", admin.UserName);
                cmd.Parameters.AddWithValue("@passwordHash", admin.PasswordHash);
                cmd.Parameters.AddWithValue("@salt", admin.Salt);
                cmd.Parameters.AddWithValue("@status", admin.Status);
                cmd.Parameters.AddWithValue("@createAt", admin.CreateAt);
                cmd.Parameters.AddWithValue("@updateAt", admin.UpdateAt);
                cmd.Parameters.AddWithValue("@deleteAt", admin.DeleteAt);
                cmd.Prepare();
                int result = cmd.ExecuteNonQuery(); // Số bản ghi trả về
                if (result>0)
                {
                    return admin;
                }    
            }
            return null;
        }
        
        //1. danh sách lịch sử giao dịch
        public List<TransactionHistory> ListTransactionHistory()
        {
            List<TransactionHistory> list = new List<TransactionHistory>();
            using (var cnn = ConnectionHelper.getConnection())
            {
                cnn.Open();
                MySqlCommand cmd = cnn.CreateCommand();
                cmd.CommandText = FindAllTransactionHistoryCommand;
                cmd.Prepare();

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var transaction = new TransactionHistory()
                    {
                        Id = reader.GetString("id"),
                        SenderAccountNumber = reader.GetString("senderAccountNumber"),
                        ReceiverAccountNumber = reader.GetString("receiverAccountNumber"),
                        Amount = reader.GetDouble("amount"),
                        Type = reader.GetInt32("type"),
                        CreateAt = reader.GetDateTime("createAt"),
                        UpdateAt = reader.GetDateTime("updateAt"),
                        DeleteAt = reader.GetDateTime("deleteAt")
                    };
                    list.Add(transaction);
                }
                return list;
            }
        }
        
        // 2. Khóa và mở khóa người dùng
        public bool LockOrUnLock(string idAccount, int status)
        {
            using (var cnn = ConnectionHelper.getConnection())
            {
                cnn.Open();
                MySqlCommand cmd = cnn.CreateCommand();
                cmd.CommandText = LockOrUnlockCommand;
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@account_number", idAccount);
                cmd.Prepare();
                int result = cmd.ExecuteNonQuery(); // Số bản ghi trả về
                if (result>0)
                {
                    return true;
                }    
            }
            return false;
        }
        
        // 3. danh sách lịch sử giao dịch theo Account Number
        public List<TransactionHistory> FindTransactionHistoryByAccountNumber(string accountNumber)
        {
            List<TransactionHistory> list = new List<TransactionHistory>();
            using (var cnn = ConnectionHelper.getConnection())
            {
                cnn.Open();
                MySqlCommand cmd = cnn.CreateCommand();
                cmd.CommandText = FindAllAccountTransactionCommand;
                cmd.Parameters.AddWithValue("@senderAccountNumber", accountNumber);
                cmd.Parameters.AddWithValue("@receiverAccountNumber", accountNumber);
                cmd.Prepare();

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var transaction = new TransactionHistory()
                    {
                        Id = reader.GetString("id"),
                        SenderAccountNumber = reader.GetString("senderAccountNumber"),
                        ReceiverAccountNumber = reader.GetString("receiverAccountNumber"),
                        Amount = reader.GetDouble("amount"),
                        Type = reader.GetInt32("type"),
                        CreateAt = reader.GetDateTime("createAt"),
                        UpdateAt = reader.GetDateTime("updateAt"),
                        DeleteAt = reader.GetDateTime("deleteAt")
                    };
                    list.Add(transaction);
                }
                return list;
            }
        }
        
        //4. Cập nhật database Admin
        //4.1 thay đổi thông tin cá nhân Admin
        public Admin UpdateInfo(string id, Admin updateAccount)
        {
            using (var cnn = ConnectionHelper.getConnection())
            {
                cnn.Open();
                MySqlCommand cmd = cnn.CreateCommand();
                cmd.CommandText = UpdateInfoCommand;
                cmd.Parameters.AddWithValue("@userName", updateAccount.UserName);
                cmd.Parameters.AddWithValue("@updateAt", updateAccount.UpdateAt);
                cmd.Parameters.AddWithValue("@accountNumber", id);
                cmd.Prepare();
                int result = cmd.ExecuteNonQuery(); // Số bản ghi trả về
                if (result>0)
                {
                    return updateAccount;
                }    
            }
            return null;
        }
        
        //4.2 thay đổi mật khẩu Admin
        public Admin UpdatePassword(string id, Admin updateAccount)
        {
            using (var cnn = ConnectionHelper.getConnection())
            {
                cnn.Open();
                MySqlCommand cmd = cnn.CreateCommand();
                cmd.CommandText = UpdatePasswordCommand;
                cmd.Parameters.AddWithValue("@passwordHash", updateAccount.PasswordHash);
                cmd.Parameters.AddWithValue("@salt", updateAccount.Salt);
                cmd.Parameters.AddWithValue("@updateAt", updateAccount.UpdateAt);
                cmd.Parameters.AddWithValue("@accountNumber", id);
                cmd.Prepare();
                int result = cmd.ExecuteNonQuery(); // Số bản ghi trả về
                if (result>0)
                {
                    return updateAccount;
                }    
            }
            return null;
        }
        
        // 6. (ngoài lề) tìm admin theo Account Number
        public Admin FindByAccountNumber(string accountNumber)
        {
            using (var cnn = ConnectionHelper.getConnection())
            {
                cnn.Open();
                MySqlCommand cmd = cnn.CreateCommand();
                cmd.CommandText = SelectByAdminCommand;
                cmd.Parameters.AddWithValue("@accountNumber", accountNumber);
                cmd.Prepare();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Admin admin = new Admin()
                    {
                        AccountNumber = reader.GetString("accountNumber"),
                        UserName = reader.GetString("userName"),
                        PasswordHash = reader.GetString("passwordHash"),
                        Salt = reader.GetString("salt"),
                        Status = reader.GetInt32("status"),
                        CreateAt = reader.GetDateTime("createAt"),
                        UpdateAt = reader.GetDateTime("updateAt"),
                        DeleteAt = reader.GetDateTime("deleteAt")
                    };
                    return admin;
                }
            }
            return null;
        }
        
        // 7. (ngoài lề) tìm admin theo user name
        public Admin FindByUserName(string userName)
        {
            using (var cnn = ConnectionHelper.getConnection())
            {
                cnn.Open();
                MySqlCommand cmd = cnn.CreateCommand();
                cmd.CommandText = SelectByAdminUserNameCommand;
                cmd.Parameters.AddWithValue("@userName", userName);
                cmd.Prepare();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Admin admin = new Admin()
                    {
                        AccountNumber = reader.GetString("accountNumber"),
                        UserName = reader.GetString("userName"),
                        PasswordHash = reader.GetString("passwordHash"),
                        Salt = reader.GetString("salt"),
                        Status = reader.GetInt32("status"),
                        CreateAt = reader.GetDateTime("createAt"),
                        UpdateAt = reader.GetDateTime("updateAt"),
                        DeleteAt = reader.GetDateTime("deleteAt")
                    };
                    return admin;
                }
            }
            return null;
        }
    }
}