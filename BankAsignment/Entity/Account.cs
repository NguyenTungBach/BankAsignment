using System;
using System.Collections.Generic;
using System.Globalization;
using BankAsignment.Until;

namespace BankAsignment.Entity
{
    public class Account
    {
        public string AccountNumber { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public double Balance { get; set; } // số tiền mặc định là 50000
        public int Status { get; set; } // trạng thái mặc định là 1
        public DateTime CreateAt { get; set; } // ngày mặc định là DateTime.Now
        public DateTime UpdateAt { get; set; } // ngày mặc định là DateTime.Now
        public DateTime DeleteAt { get; set; } // ngày mặc định là DateTime.Now
        
        CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN"); 
        public Account()
        {
            GenerateAccountNumber();
            Balance = 50000;
            Status = 1;
            CreateAt = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
            UpdateAt = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
            DeleteAt = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
            // genarate giá trị cho create at, update at, account number, status
        }

        public Dictionary<string, string> CheckValid()
        {
            var errors = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(UserName))
            {
                errors.Add("username","User name can not be null or empty");
            }
            if (string.IsNullOrEmpty(Password))
            {
                errors.Add("password","Password can not be null or empty");
            }
            if (!Password.Equals(PasswordConfirm))
            {
                errors.Add("passwordComfirm","Password and Confirm Password are not matched!");
            }
            return errors;
        }

        public override string ToString()
        {
            if (Status == 0)
            {
                return $"Account Number = {AccountNumber}\n" + 
                       $"UserName = {UserName}\n" +
                       $"Phone = {Phone}\n" +
                       $"Balance = {double.Parse(Convert.ToString(Balance)).ToString("#,###", cul.NumberFormat)}\n" +
                       "Status = Lock\n" +
                       $"CreateAt = {CreateAt.ToString("dd/MM/yyyy")}\n" +
                       $"UpdateAt = {UpdateAt.ToString("dd/MM/yyyy")}\n" +
                       $"DeleteAt = {DeleteAt.ToString("dd/MM/yyyy")}";     
            }
            return $"Account Number = {AccountNumber}\n" +
                   $"UserName = {UserName}\n" +
                   $"Phone = {Phone}\n" +
                   $"Balance = {double.Parse(Convert.ToString(Balance)).ToString("#,###", cul.NumberFormat)}\n" +
                   "Status = UnLock\n" +
                   $"CreateAt = {CreateAt.ToString("dd/MM/yyyy")}\n" +
                   $"UpdateAt = {UpdateAt.ToString("dd/MM/yyyy")}\n" +
                   $"DeleteAt = {DeleteAt.ToString("dd/MM/yyyy")}";   
        }

        public void EncryptPassword()
        {
            //3.1 Tạo muối
            Salt = HashUtil.RandomString(7);
            //3.2 Băm password với muối
            PasswordHash = HashUtil.HashWithSHA1(Password, Salt);
        }

        public void GenerateAccountNumber()
        {
            AccountNumber = Guid.NewGuid().ToString();
        }

        public Dictionary<string, string> CheckValidLogin()
        {
            var errors = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(UserName))
            {
                errors.Add("username","User name can not be null or empty");
            }
            if (string.IsNullOrEmpty(Password))
            {
                errors.Add("password","Password can not be null or empty");
            }
            return errors;
        }
    }
}