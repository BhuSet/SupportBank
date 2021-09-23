using System.Collections.Generic;

namespace SupportBank
{
    class Account
    {
        public int Accountid {get;set;}
        public string Name {get;set;}
        public List<Transaction> IncomingTransactions;
        public List<Transaction> OutgoingTransactions;

        public Account(string name)
        {
            //Accountid = Id;
            Name = name;
            IncomingTransactions = new List <Transaction>();
            OutgoingTransactions = new List <Transaction>();
        }

        
        public static decimal calculateAmount(List<Transaction> transactions)
        {
            decimal total = 0;
            transactions.ForEach(transaction => total += transaction.Amount);
            return total;
        }

    }
}