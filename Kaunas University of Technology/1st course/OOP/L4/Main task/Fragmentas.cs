using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4
{
    /// <summary>
    /// Klasė skirta laikyti fragmento duomenis
    /// </summary>
    class Fragmentas
    {
        public string Fragment { get; private set; }
        public int Pradzia { get; private set; }
        public int Pabaiga { get; private set; }

        public Fragmentas(string fragment, int pradzia, int pabaiga)
        {
            Fragment = fragment;
            Pradzia = pradzia;
            Pabaiga = pabaiga;
        }
    }
}
