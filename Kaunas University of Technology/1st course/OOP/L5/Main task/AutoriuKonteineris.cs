using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace programa
{
    class AutoriuKonteineris
    {
        public const int Max = 10000;
        public int AutoriuKiekis { get; private set; }
        public Autorius[] autoriai;

        public AutoriuKonteineris()
        {
            AutoriuKiekis = 0;
            autoriai = new Autorius[Max];
        }
        public Autorius Imti(int index)
        {
            return autoriai[index];
        }
        public void Deti(Autorius a1)
        {
            autoriai[AutoriuKiekis++] = a1;
        }
        public void Deti(int index, Autorius a1)
        {
            autoriai[index] = a1;
        }
        public int GetCount3()
        {
            return AutoriuKiekis;
        }
        public int IndexOf(string autorius, string pavadinimas)
        {
            int index = -1;
            for (int i = 0; i < AutoriuKiekis; i++)
                if (autorius == autoriai[i].VardasPavarde)
                    index = i;
            return index;
        }

    }
}
