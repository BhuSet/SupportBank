using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using NLog;
using NLog.Config;
using NLog.Targets;
using Newtonsoft.Json; 

namespace SupportBank
{
    class Bank
    {
        public List<Transaction> Transactions {get; set;}
        public Dictionary<string, Account> Accounts {get;set;}

        enum menu
        {
            List_All = 1,
            List_one_account = 2

        }

        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public void CreateDetails()
        {
            Transactions = ReadAllTransactionsFromCsv("C:\\Training\\Support Bank\\Transactions2014.csv");
            //Transactions.RemoveAll(transaction => string.IsNullOrEmpty(transaction.From)); // Removes all the empty list elements
            Logger.Info("File Processing Completed !!!");
            
            Accounts = CreateAllAccounts();
            
        }

        public void CreateDetails(string filename)
        {
            Transactions = ReadTransactions(filename);
            Logger.Info("File Processing Completed !!!");
            
            Accounts = CreateAllAccounts();
            
        }

        public List<Transaction> ReadTransactions(string filename)
        {
            if(filename.EndsWith(".csv"))
                return ReadAllTransactionsFromCsv(filename);
            else if(filename.EndsWith(".json"))
                return ReadAllTransactionsFromJson(filename);
            throw new ArgumentOutOfRangeException("File name is not of the correct type.", filename);
        }

        public List<Transaction> ReadAllTransactionsFromCsv(string path) => File.ReadAllLines(path)
                                                .Skip(1)
                                                .Select(line => Transaction.ReadFromCsv(line))
                                                .Where(t => t!= null) // Removes all the empty list elements
                                                .ToList();


        public List<Transaction> ReadAllTransactionsFromJson(string path) =>
                JsonConvert.DeserializeObject<List<Transaction>>(File.ReadAllText(path));

        public Dictionary<string, Account> CreateAllAccounts()
        {
            Dictionary<string, Account> accounts = new Dictionary<string, Account>();
            Console.WriteLine(Transactions.Count);

            foreach (Transaction transaction in Transactions)
            {
                if (!accounts.ContainsKey(transaction.FromAccount))
                    accounts.Add(transaction.FromAccount, new Account(transaction.FromAccount));

                if (!accounts.ContainsKey(transaction.ToAccount))
                    accounts.Add(transaction.ToAccount, new Account(transaction.ToAccount));

                accounts[transaction.FromAccount].OutgoingTransactions.Add(transaction);
                accounts[transaction.ToAccount].IncomingTransactions.Add(transaction);
            }
            Console.WriteLine(accounts.Count);
            return accounts;
        }

        public void ListAllAccounts()
        {
            foreach (KeyValuePair<string, Account> account in Accounts)
            {
                decimal Balance = Account.calculateAmount(account.Value.OutgoingTransactions, account.Value.IncomingTransactions);
                Console.WriteLine(string.Format("{0,-10} {1,-15} {2,-15}",
                                    account.Key,
                                    (Balance < 0 ? "owe" : "owed"),
                                    Math.Abs(Balance)));
            }
        }

        public void ListAccount(string accountname)
        {
            if(Accounts.ContainsKey(accountname))
            {
                List<Transaction> AllTransaction = (Accounts[accountname].OutgoingTransactions.Union(Accounts[accountname].IncomingTransactions).ToList());
                AllTransaction.OrderBy(x => x.Date);
                AllTransaction.ForEach(transaction => Transaction.PrintTransaction(transaction));
            }
            else
                Console.WriteLine("Cannot Find the Person name");

        }
        
        static void Main(string[] args)
        {
            //Logging
            var config = new LoggingConfiguration();
            var target = new FileTarget { FileName = @"C:\Training\Support Bank\Logs\SupportBank.log", Layout = @"${longdate} ${level} - ${logger}: ${message}" };
            config.AddTarget("File Logger", target);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, target));
            LogManager.Configuration = config;

            Bank NewBank = new Bank();
            
            Console.WriteLine("Enter file path :");
            string filepath = Console.ReadLine();
            
            if(string.IsNullOrEmpty(filepath))
            {
                NewBank.CreateDetails();
            }
            else if(File.Exists(filepath))
            {
                if ((filepath.EndsWith(".csv")) || (filepath.EndsWith(".json")))
                {
                    NewBank.CreateDetails(filepath);
                }
            }
                

            while (true)
            {

                Console.WriteLine("Select an option\n1. List All \n2. List a account");
                menu option = menu.List_All;
                while(!(menu.TryParse(Console.ReadLine(), out option)))
                    Console.WriteLine($"Please enter Valid Option 1 or 2");
                
                switch (option)
                {
                    case (menu.List_All):
                        NewBank.ListAllAccounts();
                        break;

                    case (menu.List_one_account):
                        Console.WriteLine("Enter Account Name:");
                        NewBank.ListAccount(Console.ReadLine());
                        break;

                    default :
                        Console.WriteLine("Please enter Valid option 1 or 2");
                        break;
                }

            }

        }

    }
}
