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
            
            // path to the csv file
            /*string path = "C:/Training/Support Bank/DodgyTransactions2015.csv";

            var lines = File.ReadAllLines(path).Skip(1);
            foreach (string line in lines)
            {
                var fields = line.Split(',');
                foreach (string field in fields)
                {
                    Console.WriteLine(field);
                }
            }*/
            List<Transaction> transactions = File.ReadAllLines("C:\\Training\\Support Bank\\Transactions2014.csv")
                                .Skip(1)
                                .Select(line => Transaction.FromCsv(line))
                                .ToList();

            List<Account> accounts = new List<Account>();
            
            CreateAllAccounts(accounts, transactions);
            
            Console.WriteLine(accounts.Count);

            while(true)
            {
                Console.WriteLine("Select an option\n 1.ListAll \n2. List a account");
                var option = (Console.ReadLine());
                switch (option)
                {
                    case "1":
                            ListAllAccounts(accounts);
                            break;

                    case "2":
                            Console.WriteLine("Enter Account Name:");
                            var accountname = Console.ReadLine();


                            break;
                }

            }

            

        }

        public static void CreateAllAccounts(List<Account> accounts, List<Transaction> transactions)
        {
            List<string> Employees = new List<string> ();

            foreach (Transaction transaction in transactions)
            {
                if (Employees.Find(employee => employee == transaction.From) == null)
                    Employees.Add(transaction.From);
                if (Employees.Find(employee => employee == transaction.To) == null)
                    Employees.Add(transaction.To);       
            }
            Employees.ForEach(Console.WriteLine);

            foreach(string employee in Employees)
                accounts.Add(CreateAccount(employee, transactions));
            
            
            
        }

        public static Account CreateAccount(string employee, List<Transaction> transactions)
        {
                      
            Account account = new Account(employee);

            foreach(Transaction transaction in transactions)
            {
                if(transaction.From == employee)
                    account.OutgoingTransactions.Add(transaction);
                else if (transaction.To == employee)
                    account.IncomingTransactions.Add(transaction);
            }
            return account;
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
            transactions.ForEach(transaction => total +=transaction.Amount);
            return total;
        }
    }
}
