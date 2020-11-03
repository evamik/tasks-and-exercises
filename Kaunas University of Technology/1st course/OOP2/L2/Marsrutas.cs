using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace L2
{
    /// <summary>
    /// Maršruto duomenų klasė
    /// </summary>
    public class Marsrutas
    {
        public string Kelias { get; private set; }
        public float Atstumas { get; set; }
        

        public Marsrutas(string miestas)
        {
            Kelias = miestas;
            Atstumas = 0;
        }

        public Marsrutas(string kelias, float atstumas)
        {
            Kelias = kelias;
            Atstumas = atstumas;
        }

        public bool Yra(string miestas)
        {
            string[] miestai = Kelias.Replace(", ", ",").Split(',');
            foreach(string m in miestai)
                if (m == miestas)
                    return true;
            return false;
        }

        public void Deti(string miestas, float atstumas)
        {
            Kelias = Kelias + ", " + miestas;
            Atstumas += atstumas;
        }

        public string Paskutinis()
        {
            string[] miestai = Kelias.Replace(", ", ",").Split(',');
            return miestai[miestai.Length - 1];
        }

        public static bool operator <=(Marsrutas kaire, Marsrutas desine)
        {
            return (kaire.Atstumas < desine.Atstumas) ||
                (kaire.Atstumas == desine.Atstumas) &&
                (string.Compare(kaire.Kelias,
                desine.Kelias) >= 0);
        }

        public static bool operator >=(Marsrutas kaire, Marsrutas desine)
        {
            return !(kaire <= desine);
        }

        public override string ToString()
        {
            return string.Format("| {0, -60} | {1, 12} |", Kelias, Atstumas);
        }
    }
}