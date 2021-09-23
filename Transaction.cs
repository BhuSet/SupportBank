using System;
using NLog;

namespace SupportBank
{
    class Transaction
    {
        public DateTime Date;
        public string FromAccount;
        public string ToAccount;
        public string Narrative;
        public decimal Amount;

        private static readonly ILogger Transaction_Logger = LogManager.GetCurrentClassLogger();

        public static Transaction ReadFromCsv(string csvLine)
        {
            string[] fields = csvLine.Split(',');
            Transaction transaction = new Transaction();
            if(!(DateTime.TryParse(fields[0], out transaction.Date)))
            {
                Transaction_Logger.Error($"Invalid Value: {fields[0]} - Expected Date in MM/DD/YYYY format in Transaction {csvLine}");
                Console.WriteLine("Skipping Invalid Transaction");
                return null; // Returns empty transaction
            }
            
            transaction.FromAccount = fields[1];
            transaction.ToAccount = fields[2];
            transaction.Narrative = fields[3];
            if(!(Decimal.TryParse(fields[4], out transaction.Amount)))
            {
                Transaction_Logger.Error($"Invalid Value : {fields[4]} - Expected Amount in decimal format in Transaction {csvLine}");
                Console.WriteLine("Skipping Invalid Transaction");
                return null; // Returns empty transaction
            }
            return transaction;
        }

        public static void PrintTransaction(Transaction transaction)
        {
            Console.WriteLine(string.Format("{0,-15}{1,-10}{2,-10}{3,-35}{4,-10}",
                                transaction.Date.ToString("MM-dd-yyyy"), 
                                transaction.FromAccount,
                                transaction.ToAccount,
                                transaction.Narrative,
                                "£"+transaction.Amount));
        }
    }
}