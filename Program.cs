using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace SupportBank
{
    class Program
    {
        enum menu
        {
            List_All = 1,
            List_one_account = 2

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
                Console.WriteLine("Amount to owe = " + Account.calculateAmount(account.Value.OutgoingTransactions));
                Console.WriteLine("Amount owed   = " + Account.calculateAmount(account.Value.IncomingTransactions));
            }
        }

        public static void ListAccount(Dictionary<string, Account> accounts, string accountname)
        {
            Console.WriteLine(accounts[accountname].Name);
            Console.WriteLine("Transaction\n");
            accounts[accountname].OutgoingTransactions.ForEach(transaction => Transaction.PrintTransaction(transaction));
            accounts[accountname].IncomingTransactions.ForEach(transaction => Transaction.PrintTransaction(transaction));

        }
        
        static void Main(string[] args)
        {
            List<Transaction> transactions = ReadAllTransactions("C:\\Training\\Support Bank\\Transactions2014.csv");

            Dictionary<string, Account> accounts = CreateAllAccounts(transactions);

            while (true)
            {
                Console.WriteLine("Select an option\n1. List All \n2. List a account");
                menu option = (menu) int.Parse(Console.ReadLine());
                
                switch (option)
                {
                    case (menu.List_All):
                        ListAllAccounts(accounts);
                        break;

                    case (menu.List_one_account):
                        Console.WriteLine("Enter Account Name:");
                        var accountname = Console.ReadLine();
                        ListAccount(accounts, accountname);
                        break;
                }

            }

        }

    }
}
