using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2
{
    public sealed class Miestai : Sarasas<Miestas>
    {
        private Mazgas paieska;

        public bool Yra(string miestas)
        {
            for (paieska = pradinis; paieska != null; paieska = paieska.Desine)
                if (paieska.Duomenys.Pavadinimas == miestas)
                    return true;
            return false;
        }

        public Miestas Rastas()
        {
            return paieska.Duomenys;
        }
    }
}