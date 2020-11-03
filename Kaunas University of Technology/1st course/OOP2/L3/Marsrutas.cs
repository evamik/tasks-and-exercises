using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace L2
{
    public class Marsrutas : IComparable<Marsrutas>, IEquatable<Marsrutas>
    {
        public string Kelias { get; private set; }
        public float Atstumas { get; set; }
        

        public Marsrutas(string miestas)
        {
            Kelias = miestas;
            Atstumas = 0;
        }

        public Marsrutas(string kelias, float atstumas)
        {
            Kelias = kelias;
            Atstumas = atstumas;
        }

        public bool Yra(string miestas)
        {
            string[] miestai = Kelias.Replace(", ", ",").Split(',');
            foreach(string m in miestai)
                if (m == miestas)
                    return true;
            return false;
        }

        public void Deti(string miestas, float atstumas)
        {
            Kelias = Kelias + ", " + miestas;
            Atstumas += atstumas;
        }

        public string Paskutinis()
        {
            string[] miestai = Kelias.Replace(", ", ",").Split(',');
            return miestai[miestai.Length - 1];
        }

        public override string ToString()
        {
            return string.Format("| {0, -60} | {1, 12} |", Kelias, Atstumas);
        }

        public int CompareTo(Marsrutas other)
        {
            if (other == null)
                return 1;
            if (Atstumas.CompareTo(other.Atstumas) != 0)
                return Atstumas.CompareTo(other.Atstumas);
            else
                return Kelias.CompareTo(other.Kelias);
        }

        public bool Equals(Marsrutas other)
        {
            if (other == null)
                return false;
            if (Atstumas.Equals(other.Atstumas) && Kelias.Equals(other.Kelias))
                return true;
            return false;
        }
    }
}