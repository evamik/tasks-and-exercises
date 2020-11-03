using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L4.App_Code
{
    /// <summary>
    /// Class to store subscribtion data (string surname, string address, int periodStart, int periodLength, string issueCode, int amount)
    /// </summary>
    public class Subscribtion
    {
        public string Surname { get; private set; }
        public string Address { get; private set; }
        public int PeriodStart { get; private set; }
        public int PeriodLength { get; private set; }
        public string IssueCode { get; private set; }
        public int Amount { get; private set; }

        public Subscribtion(string surname, string address, int periodStart, int periodLength, string issueCode, int amount)
        {
            Surname = surname;
            Address = address;
            PeriodStart = periodStart;
            PeriodLength = periodLength;
            IssueCode = issueCode;
            Amount = amount;
        }

        public override string ToString()
        {
            return $"| {Surname, -12} | {Address, -12} | {PeriodStart, 7} | {PeriodLength, 5} | {IssueCode, -10} | {Amount, 6} |";
        }
    }
}