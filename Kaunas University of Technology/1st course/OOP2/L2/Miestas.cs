using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2
{
    /// <summary>
    /// Miesto duomenų klasė
    /// </summary>
    public class Miestas
    {
        public string Pavadinimas { get; private set; }
        public int GyventojuSk { get; private set; }
        public Tikslai KelTikslai { get; private set; }

        public Miestas(string pavadinimas, int gyventojuSk)
        {
            Pavadinimas = pavadinimas;
            GyventojuSk = gyventojuSk;
            KelTikslai = new Tikslai(pavadinimas);
        }

        public void Deti(KelionesTikslas tikslas)
        {
            KelTikslai.Deti(tikslas);
        }

        public bool Yra(string miestas, out KelionesTikslas tikslas)
        {
            tikslas = null;
            if (KelTikslai.Yra(miestas))
            {
                tikslas = KelTikslai.Imti();
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return string.Format("| {0, -14} | {1, 13} |", Pavadinimas, GyventojuSk);
        }
    }
}