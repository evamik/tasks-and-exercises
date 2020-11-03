using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace L2
{
    public sealed class MarsrutuSarasas : Sarasas<Marsrutas>
    {
        private void Salinti(Mazgas m)
        {
            if (m == pradinis) pradinis = pradinis.Desine;
            if (m == paskutinis) paskutinis = paskutinis.Kaire;
            if (m.Kaire != null) m.Kaire.Desine = m.Desine;
            if (m.Desine != null) m.Desine.Kaire = m.Kaire;
            m.Desine = null;
            m.Kaire = null;
        }

        /// <summary>
        /// Pašalina iš sąrašo pirmą pasitaikiusį maršrutą atitinkantį nurodytus duomenis
        /// </summary>
        /// <param name="marsrutas"> maršruto duomenys </param>
        public void Pasalinti(Marsrutas marsrutas)
        {
            if (dabartinisEnum.Duomenys.Equals(marsrutas))
            {
                Mazgas kaire = dabartinisEnum.Kaire;
                Salinti(dabartinisEnum);
                if (kaire != null)
                    dabartinisEnum = kaire;
                else dabartinisEnum.Desine = pradinis;
                return;
            }

            for(Mazgas m = pradinis; m != null; m = m.Desine)
                if (m.Duomenys.Equals(marsrutas))
                {
                    Salinti(m);
                    break;
                }
        }

        public void Rikiuoti()
        {
            for(Mazgas m1 = pradinis; m1 != null; m1 = m1.Desine)
            {
                Mazgas max = m1;
                for(Mazgas m2 = m1; m2 != null; m2 = m2.Desine)
                {
                    if (m2.Duomenys.CompareTo(max.Duomenys) < 0)
                        max = m2;
                }
                Marsrutas marsrutas = m1.Duomenys;
                m1.Duomenys = max.Duomenys;
                max.Duomenys = marsrutas;
            }
        }
    }
}