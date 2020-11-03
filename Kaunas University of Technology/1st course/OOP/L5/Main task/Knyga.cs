using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace programa
{
    class Knyga : Leidinys
    {
        public long ISBN { get; set; }
        public string Autorius { get; set; }

        public Knyga(long isbn, string autorius, string pavadinimas, string tipas, string leidykla, int metai, int pskaicius, int tirazas) : 
            base(pavadinimas, tipas, leidykla, metai, pskaicius, tirazas)
        {
            ISBN = isbn;
            Autorius = autorius;
            Pavadinimas = pavadinimas;
            Tipas = tipas;
            Leidykla = leidykla;
            Metai = metai;
            PSkaicius = pskaicius;
            Tirazas = tirazas;
        }

        public Knyga(string[] duomenys) : base(duomenys)
        {
            Uzpildymas(duomenys);
        }

        public override void Uzpildymas(string[] duomenys)
        {
            base.Uzpildymas(duomenys);
            ISBN = long.Parse(duomenys[6]);
            Autorius = duomenys[7];
            IsleidimoData = new DateTime(Metai, 1, 1);
        }

        public bool ArSenas()
        {
            //Console.WriteLine(DateTime.Now.Subtract(new DateTime(Metai, 1, 1)).Days / (365.25) + "<<<");
            return DateTime.Now.Subtract(new DateTime(Metai, 1, 1)).Days / (365.25) > 1;
        }

        public override string ToString()
        {
            return base.ToString() + String.Format(" {0, 10} | {1, -15} |", ISBN, Autorius);
        }
    }
}