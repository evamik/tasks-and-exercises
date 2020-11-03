using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2
{
    /// <summary>
    /// Miestų sąrašo klasė
    /// </summary>
    public sealed class Miestai
    {
        private sealed class Mazgas
        {
            public Miestas Duomenys { get; set; }
            public Mazgas Kitas { get; set; }

            public Mazgas(Miestas miestas, Mazgas adr)
            {
                Duomenys = miestas;
                Kitas = Kitas;
            }
        }

        private Mazgas prad;
        private Mazgas pab;
        private Mazgas d;
        private Mazgas paieska;


        public Miestai()
        {
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
            for (paieska = prad; paieska != null; paieska = paieska.Kitas)
                if (paieska.Duomenys.Pavadinimas == miestas)
                    return true;
            return false;
        }

        public void Deti(Miestas miestas)
        {
            var dd = new Mazgas(miestas, null);
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

        public Miestas Imti()
        {
            return d.Duomenys;
        }

        public Miestas Rastas()
        {
            return paieska.Duomenys;
        }
    }
}