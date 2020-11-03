using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace programa
{
    class Laikrastis : Leidinys
    {
        public DateTime Data { get; set; }
        public int Numeris { get; set; }

        public Laikrastis(DateTime data, int numeris, string pavadinimas, string tipas, string leidykla, int metai, int pskaicius, int tirazas) : 
            base(pavadinimas, tipas, leidykla, metai, pskaicius, tirazas)
        {
            Data = data;
            Numeris = numeris;
            Pavadinimas = pavadinimas;
            Tipas = tipas;
            Leidykla = leidykla;
            Metai = metai;
            PSkaicius = pskaicius;
            Tirazas = tirazas;
        }

        public Laikrastis(string[] duomenys) : base(duomenys)
        {
            Uzpildymas(duomenys);
        }

        public override void Uzpildymas(string[] duomenys)
        {
            base.Uzpildymas(duomenys);
            Data = DateTime.Parse(duomenys[6]);
            Numeris = int.Parse(duomenys[7]);
            IsleidimoData = Data;
        }

        public bool ArSenas()
        {
            //Console.WriteLine(DateTime.Now.Subtract(Data).Days+"<<<");
            return DateTime.Now.Subtract(Data).Days > 7;
        }

        public override string ToString()
        {
            return base.ToString() + String.Format(" {0, 10} | {1, 15} |", Data.ToShortDateString(), Numeris);
        }
    }
}