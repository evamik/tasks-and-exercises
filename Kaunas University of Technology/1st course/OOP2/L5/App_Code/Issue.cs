using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L4.App_Code
{
    /// <summary>
    /// class to store issue data (string code, string title, double monthlyCost) 
    /// </summary>
    public class Issue : IEquatable<Issue>
    {
        public string Code { get; private set; }
        public string Title { get; private set; }
        public double MonthlyCost { get; private set; }

        public Issue(string code, string title, double monthlyCost)
        {
            Code = code;
            Title = title;
            MonthlyCost = monthlyCost;
        }

        public bool Equals(Issue other)
        {
            return Code.Equals(other.Code);
        }

        public override string ToString()
        {
            return $"| {Code, -10} | {Title, -12} | {MonthlyCost, 6:0.00} |";
        }
    }
}