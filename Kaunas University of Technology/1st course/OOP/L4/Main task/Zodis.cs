using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4
{
    /// <summary>
    /// Klasė skirta laikyti Žodžio duomenis ir juos manipuliuoti
    /// </summary>
    class Zodis
    {
        public string Pavadinimas { get; private set; }
        private int Kiekis;
        public int Eilute { get; private set;}
        public string Skyriklis { get; private set; }

        public Zodis(string zodis)
        {
            Pavadinimas = zodis;
            Kiekis = 1;
        }

        public Zodis(string zodis, string skyriklis, int eilute)
        {
            Pavadinimas = zodis;
            Skyriklis = skyriklis;
            Eilute = eilute;
        }

        /// <summary>
        /// Pakeičia skyriklį
        /// </summary>
        /// <param name="skyriklis"></param>
        public void Iterpimas(string skyriklis)
        {
            Skyriklis = skyriklis;
        }

        /// <summary>
        /// Užpildo žodį tarpais
        /// </summary>
        /// <param name="p"></param>
        public void Iterpimas(int p)
        {
            Pavadinimas = Pavadinimas.PadLeft(Pavadinimas.Length + p);
        }

        static public Zodis operator ++(Zodis zodis)
        {
            zodis.Kiekis++;
            return zodis;
        }

        static public Zodis operator --(Zodis zodis)
        {
            zodis.Kiekis--;
            return zodis;
        }

        public override string ToString()
        {
            return string.Format("{0, -15} {1, 2}", Pavadinimas, Kiekis);
        }

        public string ToString2()
        {
            return string.Format("{0}{1}", Pavadinimas, Skyriklis);
        }
    }
}
