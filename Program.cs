using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace SupportBank
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Transaction> transactions = ReadAllTransactions("C:\\Training\\Support Bank\\Transactions2014.csv");

            Dictionary<string, Account> accounts = CreateAllAccounts(transactions);

            while (true)
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
                        ListAccount(accounts, accountname);
                        break;
                }

            }

        }

        public static List<Transaction> ReadAllTransactions(string path) => File.ReadAllLines("C:\\Training\\Support Bank\\Transactions2014.csv")
                                                .Skip(1)
                                                .Select(line => Transaction.FromCsv(line))
                                                .ToList();

        public static Dictionary<string, Account> CreateAllAccounts(List<Transaction> transactions)
        {
            Dictionary<string, Account> accounts = new Dictionary<string, Account>();

            foreach (Transaction transaction in transactions)
            {
                if (!accounts.ContainsKey(transaction.From))
                    accounts.Add(transaction.From, new Account(transaction.From));

                if (!accounts.ContainsKey(transaction.To))
                    accounts.Add(transaction.To, new Account(transaction.To));

                accounts[transaction.From].OutgoingTransactions.Add(transaction);
                accounts[transaction.To].IncomingTransactions.Add(transaction);
            }
            return accounts;
        }

        public static void ListAllAccounts(Dictionary<string, Account> accounts)
        {
            foreach (KeyValuePair<string, Account> account in accounts)
            {
                Console.WriteLine(account.Key);
                Console.WriteLine("Amount to owe = " + calculateAmount(account.Value.OutgoingTransactions));
                Console.WriteLine("Amount owed   = " + calculateAmount(account.Value.IncomingTransactions));
            }
        }

        public static decimal calculateAmount(List<Transaction> transactions)
        {
            decimal total = 0;
            transactions.ForEach(transaction => total += transaction.Amount);
            return total;
        }

        public static void ListAccount(Dictionary<string, Account> accounts, string accountname)
        {
            Console.WriteLine(accounts[accountname].Name);
            Console.WriteLine("Transaction\n");
            accounts[accountname].OutgoingTransactions.ForEach(transaction => PrintTransaction(transaction));
            accounts[accountname].IncomingTransactions.ForEach(transaction => PrintTransaction(transaction));

        }

        public static void PrintTransaction(Transaction transaction)
        {
            Console.WriteLine(transaction.Date.ToString("MM-dd-yyyy") + "\t" + transaction.From + "\t" + transaction.To + "\t" + transaction.Narrative + "\t" + transaction.Amount);
        }
    }
}
