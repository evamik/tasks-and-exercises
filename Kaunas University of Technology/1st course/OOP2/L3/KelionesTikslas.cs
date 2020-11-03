using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2
{
    public class KelionesTikslas
    {
        public string Pavadinimas { get; private set; }
        public int GyventojuSk { get; private set; }
        public float Atstumas { get; private set; }

        public KelionesTikslas(string pavadinimas, int gyventojuSk, float atstumas)
        {
            Pavadinimas = pavadinimas;
            GyventojuSk = gyventojuSk;
            Atstumas = atstumas;
        }
    }
}