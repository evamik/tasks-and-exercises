using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace programa
{
    class Biblioteka
    {
        public const int Max = 10000;
        public int Kiekis { get; private set; }
        public Leidinys[] leidiniai;
        public Biblioteka()
        {
            Kiekis = 0;
            leidiniai = new Leidinys[Max];
        }
        public Leidinys Imti(int index)
        {
            return leidiniai[index];
        }
        public void Deti(Leidinys l)
        {
            leidiniai[Kiekis++] = l;
        }
        public void Deti(int index, Leidinys l)
        {
            leidiniai[index] = l;
        }
        public int GetCount()
        {
            return Kiekis;
        }
    }
}
