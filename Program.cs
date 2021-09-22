using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace SupportBank
{
    class Program
    {
        enum Options
        {

        }
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            
            List<Transaction> transactions = File.ReadAllLines("C:\\Training\\Support Bank\\Transactions2014.csv")
                                .Skip(1)
                                .Select(line => Transaction.FromCsv(line))
                                .ToList();

            List<Account> accounts = CreateAllAccounts(transactions);
            
            Console.WriteLine(accounts.Count);

            while(true)
            {
                Console.WriteLine("Select an option\n1. ListAll \n2. List a account");
                var option = (Console.ReadLine());
                switch (option)
                {
                    case "1":
                            ListAllAccounts(accounts);
                            break;

                    case "2":
                            Console.WriteLine("Enter Account Name:");
                            var accountname = Console.ReadLine();
                            ListAccount(accounts , accountname);
                            break;
                }

            }

        }

        public static List<Account> CreateAllAccounts(List<Transaction> transactions)
        {
                      
            Dictionary<string, Account> account = new Dictionary<string, Account>();

            foreach(Transaction transaction in transactions)
            {
                if(!account.ContainsKey(transaction.From))
                    account.Add(transaction.From, new Account(transaction.From));
                if(!account.ContainsKey(transaction.To))
                    account.Add(transaction.To, new Account(transaction.To));
                
                account[transaction.From].OutgoingTransactions.Add(transaction);
                account[transaction.To].IncomingTransactions.Add(transaction);
                
            }
            return account.Values.ToList();
        }

        public static void ListAllAccounts(List<Account> accounts)
        {
            foreach(Account account in accounts)
            {
                Console.WriteLine("Name : " + account.Name);
                Console.WriteLine("Amount to owe = " + calculateAmount(account.OutgoingTransactions));
                Console.WriteLine("Amount owed   = " + calculateAmount(account.IncomingTransactions));
            }
        }

        public static decimal calculateAmount (List<Transaction> transactions)
        {
            decimal total = 0;
            transactions.ForEach(transaction => total += transaction.Amount);
            return total;
        }

        public static void ListAccount(List<Account> accounts, string accountname)
        {
            Account accounttolist = accounts.Find(account => account.Name == accountname);
            Console.WriteLine("Name :" + accounttolist.Name);
            Console.WriteLine("Transaction\n");
            accounttolist.OutgoingTransactions.ForEach(transaction => PrintTransaction(transaction));
            accounttolist.IncomingTransactions.ForEach(transaction => PrintTransaction(transaction));

        }

        public static void PrintTransaction(Transaction transaction)
        {
            Console.WriteLine(transaction.Date.ToString("MM-dd-yyyy") + "\t"+ transaction.From + "\t" + transaction.To + "\t" + transaction.Narrative + "\t" + transaction.Amount);
        }
    }
}
