using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace programa
{
    abstract class Leidinys
    {
        public string Pavadinimas { get; set; }
        public string Tipas { get; set; }
        public string Leidykla { get; set; }
        public int Metai { get; set; }
        public int PSkaicius { get; set; }
        public int Tirazas { get; set; }

        public Leidinys(string pavadinimas, string tipas, string leidykla, int metai, int pskaicius, int tirazas)
        {
            Pavadinimas = pavadinimas;
            Tipas = tipas;
            Leidykla = leidykla;
            Metai = metai;
            PSkaicius = pskaicius;
            Tirazas = tirazas;
        }
        public override string ToString()
        {
            return String.Format("| {0, -16} | {1, -12} | {2, -20} | {3, 10} | {4, 8} | {5, 7} |", Pavadinimas, Tipas, Leidykla, Metai, PSkaicius, Tirazas);
        }

        static public bool operator >=(Leidinys lhs, Leidinys rhs)
        {
            if (lhs.Leidykla.CompareTo(rhs.Leidykla) == 0 && lhs.Pavadinimas.CompareTo(rhs.Pavadinimas) >= 0)
                return true;
            else if (lhs.Leidykla.CompareTo(rhs.Leidykla) > 0)
                return true;
            return false;
        }

        static public bool operator <=(Leidinys lhs, Leidinys rhs)
        {
            if (lhs.Leidykla.CompareTo(rhs.Leidykla) == 0 && lhs.Pavadinimas.CompareTo(rhs.Pavadinimas) <= 0)
                return true;
            else if (lhs.Leidykla.CompareTo(rhs.Leidykla) < 0)
                return true;
            return false;
        }
    }
}