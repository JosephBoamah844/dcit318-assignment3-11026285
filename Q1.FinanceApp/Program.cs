using System;
using System.Collections.Generic;

namespace Q1.FinanceApp
{
    // a) Record model
    public record Transaction(int Id, DateTime Date, decimal Amount, string Category);

    // b) Payment interface
    public interface ITransactionProcessor
    {
        void Process(Transaction transaction);
    }

    // c) Concrete processors
    public class BankTransferProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"[BankTransfer] Processed {transaction.Amount:C} for {transaction.Category} on {transaction.Date:d}.");
        }
    }

    public class MobileMoneyProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"[MobileMoney] Processed {transaction.Amount:C} for {transaction.Category} on {transaction.Date:d}.");
        }
    }

    public class CryptoWalletProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"[CryptoWallet] Processed {transaction.Amount:C} for {transaction.Category} on {transaction.Date:d}.");
        }
    }

    // d) Base Account
    public class Account
    {
        public string AccountNumber { get; }
        public decimal Balance { get; protected set; }

        public Account(string accountNumber, decimal initialBalance)
        {
            AccountNumber = accountNumber;
            Balance = initialBalance;
        }

        public virtual void ApplyTransaction(Transaction transaction)
        {
            Balance -= transaction.Amount;
            Console.WriteLine($"[Account] {transaction.Amount:C} deducted. New balance: {Balance:C}");
        }
    }

    // e) Sealed SavingsAccount
    public sealed class SavingsAccount : Account
    {
        public SavingsAccount(string accountNumber, decimal initialBalance) : base(accountNumber, initialBalance) { }

        public override void ApplyTransaction(Transaction transaction)
        {
            if (transaction.Amount > Balance)
            {
                Console.WriteLine("Insufficient funds");
                return;
            }

            Balance -= transaction.Amount;
            Console.WriteLine($"[SavingsAccount] {transaction.Amount:C} deducted. Updated balance: {Balance:C}");
        }
    }

    // f) FinanceApp
    public class FinanceApp
    {
        private readonly List<Transaction> _transactions = new();

        public void Run()
        {
            // i) SavingsAccount
            var account = new SavingsAccount("SA-001", 1000m);

            // ii) Transactions
            var t1 = new Transaction(1, DateTime.Now, 120m, "Groceries");
            var t2 = new Transaction(2, DateTime.Now, 300m, "Utilities");
            var t3 = new Transaction(3, DateTime.Now, 250m, "Entertainment");

            // iii) Processors
            ITransactionProcessor p1 = new MobileMoneyProcessor(); // t1
            ITransactionProcessor p2 = new BankTransferProcessor(); // t2
            ITransactionProcessor p3 = new CryptoWalletProcessor(); // t3

            p1.Process(t1);
            p2.Process(t2);
            p3.Process(t3);

            // iv) Apply to account
            account.ApplyTransaction(t1);
            account.ApplyTransaction(t2);
            account.ApplyTransaction(t3);

            // v) Add to list
            _transactions.AddRange(new[] { t1, t2, t3 });

            Console.WriteLine("\nAll transactions recorded:");
            foreach (var t in _transactions)
            {
                Console.WriteLine($"#{t.Id} {t.Category} {t.Amount:C} on {t.Date:g}");
            }

            Console.WriteLine($"\nFinal Balance for {account.AccountNumber}: {account.Balance:C}");
        }
    }

    public static class Program
    {
        public static void Main()
        {
            new FinanceApp().Run();
        }
    }
}
