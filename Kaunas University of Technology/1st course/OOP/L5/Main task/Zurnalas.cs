using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace programa
{
    class Zurnalas : Leidinys
    {
        public long ISBN { get; set; }
        public int Numeris { get; set; }

        public Zurnalas(long isbn, int numeris, string pavadinimas, string tipas, string leidykla, int metai, int pskaicius, int tirazas) : 
            base(pavadinimas, tipas, leidykla, metai, pskaicius, tirazas)
        {
            ISBN = isbn;
            Numeris = numeris;
            Pavadinimas = pavadinimas;
            Tipas = tipas;
            Leidykla = leidykla;
            Metai = metai;
            PSkaicius = pskaicius;
            Tirazas = tirazas;
        }

        public Zurnalas(string[] duomenys) : base(duomenys)
        {
            Uzpildymas(duomenys);
        }

        public override void Uzpildymas(string[] duomenys)
        {
            base.Uzpildymas(duomenys);
            ISBN = long.Parse(duomenys[6]);
            Numeris = int.Parse(duomenys[7]);
            IsleidimoData = new DateTime(Metai, Numeris, 1);
        }

        public bool ArSenas()
        {
            //Console.WriteLine(DateTime.Now.Subtract(new DateTime(Metai, 1, 1)).Days / (365.25 / 12) + "<<<");
            return DateTime.Now.Subtract(new DateTime(Metai, 1, 1)).Days / (365.25 / 12) > 1;
        }

        public override string ToString()
        {
            return base.ToString() + String.Format(" {0, 10} | {1, 15} |", ISBN, Numeris);
        }
    }
}