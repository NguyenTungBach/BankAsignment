using System;
using System.Collections.Generic;
using System.Globalization;

namespace BankAsignment.Entity
{
    public class TransactionHistory
    {
        public string Id { get; set; }
        public double Amount { get; set; }
        public string SenderAccountNumber { get; set; } // (string FK from Account): ai gửi đến
        public string ReceiverAccountNumber { get; set; } // (string FK from Account): ai nhận tiền
        public int Type { get; set; } // withdraw (1), deposit (2), transfer (3)
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public DateTime DeleteAt { get; set; }

        CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN"); 
        public TransactionHistory()
        {
            GenerateAccountNumber();
            CreateAt = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
            UpdateAt = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
            DeleteAt = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
        }
        
        public void GenerateAccountNumber()
        {
            Id = Guid.NewGuid().ToString();
        }

        public Dictionary<string, string> CheckValid()
        {
            var errors = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(SenderAccountNumber))
            {
                errors.Add("senderAccountNumber","Sender Account Number can not be null or empty");
            }
            if (string.IsNullOrEmpty(ReceiverAccountNumber))
            {
                errors.Add("receiverAccountNumber","Receiver Account Number can not be null or empty");
            }
            return errors;
        }

        public override string ToString()
        {
            if (Type == 1)
            {
                return $"Id = {Id}\n" + 
                       $"Amount = {Amount}\n" +
                       $"SenderAccountNumber = {SenderAccountNumber}\n" +
                       $"ReceiverAccountNumber = {ReceiverAccountNumber}\n" +
                       "Type = Deposit\n" +
                       $"CreateAt = {CreateAt.ToString("dd/MM/yyyy")}\n" +
                       $"UpdateAt = {UpdateAt.ToString("dd/MM/yyyy")}\n" +
                       $"DeleteAt = {DeleteAt.ToString("dd/MM/yyyy")}";       
            } else if (Type == 2)
            {
                return $"Id = {Id}\n" + 
                       $"Amount = {Amount}\n" +
                       $"SenderAccountNumber = {SenderAccountNumber}\n" +
                       $"ReceiverAccountNumber = {ReceiverAccountNumber}\n" +
                       "Type = Withdraw\n" +
                       $"CreateAt = {CreateAt.ToString("dd/MM/yyyy")}\n" +
                       $"UpdateAt = {UpdateAt.ToString("dd/MM/yyyy")}\n" +
                       $"DeleteAt = {DeleteAt.ToString("dd/MM/yyyy")}";   
            }
            return $"Id = {Id}\n" + 
                   $"Amount = {Amount}\n" +
                   $"SenderAccountNumber = {SenderAccountNumber}\n" +
                   $"ReceiverAccountNumber = {ReceiverAccountNumber}\n" +
                   "Type = Transfer\n" +
                   $"CreateAt = {CreateAt.ToString("dd/MM/yyyy")}\n" +
                   $"UpdateAt = {UpdateAt.ToString("dd/MM/yyyy")}\n" +
                   $"DeleteAt = {DeleteAt.ToString("dd/MM/yyyy")}";   
        }
        
    }
}