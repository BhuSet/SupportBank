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

    }
}