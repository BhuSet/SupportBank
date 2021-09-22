using System;

namespace SupportBank
{
    class Transaction
    {
        public DateTime Date;
        public string From;
        public string To;
        public string Narrative;
        public decimal Amount;

        public static Transaction FromCsv(string csvLine)
        {
            string[] fields = csvLine.Split(',');
            Transaction transaction = new Transaction();
            transaction.Date = Convert.ToDateTime(fields[0]);
            transaction.From = fields[1];
            transaction.To = fields[2];
            transaction.Narrative = fields[3];
            transaction.Amount = Convert.ToDecimal(fields[4]);
            return transaction;
        }
    }
}