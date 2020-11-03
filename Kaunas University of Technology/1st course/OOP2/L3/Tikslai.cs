using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2
{
    public sealed class Tikslai : Sarasas<KelionesTikslas>
    {
        public string PradinisMiestas { get; set; }

        public bool Yra(string miestas)
        {
            for (Pradzia(); Yra(); Desine())
                if (dabartinis.Duomenys.Pavadinimas == miestas)
                    return true;
            return false;
        }
    }
}