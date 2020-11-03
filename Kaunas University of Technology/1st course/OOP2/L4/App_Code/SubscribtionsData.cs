using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L4.App_Code
{
    /// <summary>
    /// class to store date and subscribtion list
    /// </summary>
    public class SubscribtionsData : IComparable<SubscribtionsData>
    {
        public List<Subscribtion> Subscribtions { get; private set; }
        public DateTime Date { get; private set; }

        public SubscribtionsData(List<Subscribtion> subscribtions, DateTime date)
        {
            Subscribtions = subscribtions;
            Date = date;
        }

        public int CompareTo(SubscribtionsData other)
        {
            return Date.CompareTo(other.Date);
        }
    }
}