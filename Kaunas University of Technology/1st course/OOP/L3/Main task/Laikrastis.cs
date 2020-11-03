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
    }
}