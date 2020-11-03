using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4
{
    /// <summary>
    /// Konteinerinė klasė skirta saugoti Žodžių duomenis
    /// </summary>
    class Konteineris
    {
        private Zodis[] Zodziai;
        int Kiekis;

        public Konteineris(int dydis)
        {
            Zodziai = new Zodis[dydis];
            Kiekis = 0;
        }

        public int GautiKieki()
        {
            return Kiekis;
        }

        public int Size()
        {
            return Zodziai.Length;
        }

        /// <summary>
        /// Prideda žodį į konteinerį arba padidina žodžio atsikartojimų kiekį, 
        /// jei jis jau yra konteineryje
        /// </summary>
        /// <param name="zodis"></param>
        public void PridetiKiekiui(Zodis zodis)
        {
            int i = IndexOf(zodis);
            if (i != -1)
                Zodziai[i]++;
            else if (Kiekis < Zodziai.Length)
                Zodziai[Kiekis++] = zodis;
        }

        public void Prideti(Zodis zodis)
        {
            Zodziai[Kiekis++] = zodis;
        }

        public Zodis Imti(int i)
        {
            return Zodziai[i];
        }

        int IndexOf(Zodis zodis)
        {
            for (int i = 0; i < Kiekis; i++)
            {
                if (Zodziai[i].Pavadinimas == zodis.Pavadinimas)
                    return i;
            }
            return -1;
        }
    }
}
