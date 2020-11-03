using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2
{
    /// <summary>
    /// Kelionės tiklsų sąrašo klasė
    /// </summary>
    public sealed class Tikslai
    {
        private sealed class Mazgas
        {
            public KelionesTikslas Duomenys { get; set; }
            public Mazgas Kitas { get; set; }

            public Mazgas(KelionesTikslas kelionesTikslas, Mazgas adr)
            {
                Duomenys = kelionesTikslas;
                Kitas = Kitas;
            }
        }

        public string PradinisMiestas { get; private set; }
        private Mazgas prad;
        private Mazgas pab;
        private Mazgas d;


        public Tikslai(string miestas)
        {
            PradinisMiestas = miestas;
            prad = null;
            pab = null;
            d = null;
        }

        public void Pradzia()
        {
            d = prad;
        }

        public void Kitas()
        {
            d = d.Kitas;
        }

        public bool Yra()
        {
            return d != null;
        }

        public bool Yra(string miestas)
        {
            for (Pradzia(); Yra(); Kitas())
                if (d.Duomenys.Pavadinimas == miestas)
                    return true;
            return false;
        }

        public void Deti(KelionesTikslas kelionesTikslas)
        {
            var dd = new Mazgas(kelionesTikslas, null);
            if(prad != null)
            {
                pab.Kitas = dd;
                pab = dd;
            }
            else
            {
                prad = dd;
                pab = dd;
            }
        }

        public KelionesTikslas Imti()
        {
            return d.Duomenys;
        }
    }
}