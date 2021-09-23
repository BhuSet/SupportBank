using System.Collections.Generic;
using System.Linq;

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

        
        public static decimal calculateAmount(List<Transaction> Outgoingtransactions, List<Transaction> Incomingtransactions)
        {
            return(Outgoingtransactions.Sum(transaction => transaction.Amount) -
                    Incomingtransactions.Sum(transaction => transaction.Amount) );
        }

    }
}