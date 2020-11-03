using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace programa
{
    class Filialas
    {
        public string Pavadinimas { get; set; }
        public string Adresas { get; set; }
        public int TelefonoNumeris { get; set; }
        public Biblioteka Biblioteka { get; set; }

        public Filialas(string pavadinimas,string adresas,int telefonoNumeris, Biblioteka biblioteka)
        {
            Pavadinimas = pavadinimas;
            Adresas = adresas;
            TelefonoNumeris = telefonoNumeris;
            Biblioteka = biblioteka;
        }
    }
}
