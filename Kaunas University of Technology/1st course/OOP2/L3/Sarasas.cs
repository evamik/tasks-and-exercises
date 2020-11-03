using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2
{
    public abstract class Sarasas<Tipas> : IEnumerable<Tipas>
    {
        protected sealed class Mazgas
        {
            public Tipas Duomenys { get; set; }
            public Mazgas Kaire { get; set; }
            public Mazgas Desine { get; set; }

            public Mazgas(Tipas tipas, Mazgas kaire, Mazgas desine)
            {
                Duomenys = tipas;
                Kaire = kaire;
                Desine = desine;
            }
        }

        /// <summary>
        /// Pradinis sąrašo elementas
        /// </summary>
        protected Mazgas pradinis;
        /// <summary>
        /// Paskutinis sąrašo elementas
        /// </summary>
        protected Mazgas paskutinis;
        /// <summary>
        /// Dabartinis parinktas sąrašo elementas
        /// </summary>
        protected Mazgas dabartinis;
        /// <summary>
        /// Dabartinis parinktas sąrašo enumerator elementas
        /// </summary>
        protected Mazgas dabartinisEnum;

        private Mazgas Pirmas()
        {
            return pradinis;
        }

        private Mazgas Kitas()
        {
            if(dabartinis.Desine == null)
                return null;

            dabartinis = dabartinis.Desine;
            return dabartinis;
        }

        public bool Yra()
        {
            return dabartinis != null;
        }

        public void Deti(Tipas duomenys)
        {
            var mazgas = new Mazgas(duomenys);
            if(pradinis == null)
            {
                pradinis = mazgas;
                paskutinis = mazgas;
            }
            else
            {
                paskutinis.Desine = mazgas;
                mazgas.Kaire = paskutinis;
                paskutinis = mazgas;
            }
        }

        public void DetiA(Tipas duomenys)
        {
            if (pradinis == null)
            {
                pradinis = new Mazgas(duomenys, null, null);
                paskutinis = pradinis;
            }
            else pradinis = new Mazgas(duomenys, null, pradinis);
        }

        public Tipas Imti()
        {
            Tipas duomenys = dabartinis.Duomenys;
            Desine();
            return duomenys;
        }

        public void Pradzia()
        {
            dabartinis = pradinis;
        }

        public void Desine()
        {
            dabartinis = dabartinis.Desine;
        }

        public void Kaire()
        {
            dabartinis = dabartinis.Kaire;
        }

        public IEnumerator<Tipas> GetEnumerator()
        {
            for (dabartinisEnum = pradinis; dabartinisEnum != null; dabartinisEnum = dabartinisEnum.Desine)
            {
                yield return dabartinisEnum.Duomenys;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}