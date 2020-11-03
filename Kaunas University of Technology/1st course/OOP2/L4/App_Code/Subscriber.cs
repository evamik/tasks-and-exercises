using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L4.App_Code
{
    /// <summary>
    /// class to store subscriber data (string surname, string address, **double cost)
    /// </summary>
    public class Subscriber : IComparable<Subscriber>, IEquatable<Subscriber>
    {
        public string Surname { get; private set; }
        public string Address { get; private set; }
        public double Cost { get; private set; }
        public List<DateTime> OrderedDates { get; private set; }

        public Subscriber(string surname, string address)
        {
            Surname = surname;
            Address = address;
            Cost = 0;
            OrderedDates = new List<DateTime>();
        }

        public void AddDate(DateTime date)
        {
            OrderedDates.Add(date);
        }

        public void AddCost(double cost)
        {
            Cost += cost;
        }

        public int CompareTo(Subscriber other)
        {
            if (Address.CompareTo(other.Address) != 0)
                return Address.CompareTo(other.Address);
            else
                return Surname.CompareTo(other.Surname);
        }

        public bool Equals(Subscriber other)
        {
            return Address.Equals(other.Address) && Surname.Equals(other.Surname);
        }

        public override string ToString()
        {
            return $"| {Surname, -12} | {Address, -12} | {Cost, 7:0.00} |";
        }
    }
}