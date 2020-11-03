using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2
{
    /// <summary>
    /// Maršrutų sąrašo klasė
    /// </summary>
    public sealed class Marsrutai
    {
        private sealed class Mazgas
        {
            public Marsrutas Duomenys { get; set; }
            public Mazgas Kitas { get; set; }
            
            public Mazgas(Marsrutas reiksme, Mazgas adresas)
            {
                Duomenys = reiksme;
                Kitas = adresas;
            }
        }
        
        private Mazgas prad;
        private Mazgas pab;
        private Mazgas d;
        private Mazgas v;

        public Marsrutai()
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
            if(!(d is null))
                d = d.Kitas;
        }

        public bool Yra()
        {
            return d != null;
        }

        public void Deti(Marsrutas marsrutas)
        {
            var mazgas = new Mazgas(marsrutas, null);
            if(prad != null)
            {
                pab.Kitas = mazgas;
                pab = mazgas;
            }
            else
            {
                prad = mazgas;
                pab = mazgas;
            }
        }

        public void IterptiUž(Marsrutas marsrutas)
        {
            Mazgas m = new Mazgas(marsrutas, d.Kitas);
            if (d == pab)
                pab = m;
            d.Kitas = m;

            for(Mazgas ma = prad; ma != null; ma = ma.Kitas)
            {
                if (ma.Duomenys > marsrutas)
                    IterptiUž(marsrutas);
            }
        }

        public Marsrutas Imti()
        {
            return d.Duomenys;
        }

        public void Salinti()
        {
            if (d == null)
                return;
            if(prad == pab)
            {
                prad = null;
                pab = null;
                d = null;
                return;
            }

            if(d == prad)
            {
                prad = prad.Kitas;
                d = prad;
            }
            else if(d == pab)
            {
                for (v = prad; v.Kitas != pab; v = v.Kitas) ;
                v.Kitas = null;
                pab = v;
                d = pab;
            }
            else
            {
                for (v = prad; v.Kitas != d; v = v.Kitas) ;
                v.Kitas = d.Kitas;
                d = v.Kitas;
            }
        }

        public void Rikiuoti()
        {
            for(Mazgas m1 = prad; m1 != null; m1 = m1.Kitas)
            {
                Mazgas max = m1;
                for(Mazgas m2 = m1; m2 != null; m2 = m2.Kitas)
                {
                    if (m2.Duomenys <= max.Duomenys)
                        max = m2;
                }
                Marsrutas marsrutas = m1.Duomenys;
                m1.Duomenys = max.Duomenys;
                max.Duomenys = marsrutas;
            }
        }

        public Mazgas Rasti(Duomenys duom)
        {
            for(Mazgas ma = prad; ma != null; ma = ma.Kitas)
            {
                if (ma.Duomenys == duom1)
                    return ma;
            }
        }

        public void Sukeisti(Duomenys duom1, Duomenys duom2)
        {
            Mazgas d1 = Rasti(duom1);
            Mazgas d2 = Rasti(duom2);
            if(d1 != null && d2 != null)
            {
                Duomenys duom = d1.Duomenys;
                d1.Duomenys = d2.Duomenys;
                d2.Duomenys = duom;
            }
        }
    }
}