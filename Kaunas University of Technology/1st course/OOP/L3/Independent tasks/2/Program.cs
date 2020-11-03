using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savarankiskas_2
{
    class Program
    {
        const string Krepsininkai = @"Krepsininkai.csv";
        const string Futbolininkai = @"Futbolininkai.csv";
        const string Komandos = @"Komandos.csv";

        const int MaxZaideju = 40;
        const int MaxKomandu = 10;

        static void Main(string[] args)
        {
            Program p = new Program();

            ZaidejuKont krepsininkai = new ZaidejuKont(MaxZaideju);
            p.NuskaitytiZaidejus(krepsininkai, Krepsininkai);

            ZaidejuKont futbolininkai = new ZaidejuKont(MaxZaideju);
            p.NuskaitytiZaidejus(futbolininkai, Futbolininkai);

            List<Komanda> komandos = new List<Komanda>();
            p.NuskaitytiKomandas(komandos, Komandos);

            Console.WriteLine("Įveskite norimą miestą: ");
            string miestas = Console.ReadLine();
            Console.WriteLine();

            p.RastiVidurki(krepsininkai, komandos);
            p.RastiVidurki(futbolininkai, komandos);

            ZaidejuKont atrinkti = new ZaidejuKont(MaxZaideju);
            p.AtrinktiZaidejus(atrinkti, krepsininkai, komandos, miestas);
            p.AtrinktiZaidejus(atrinkti, futbolininkai, komandos, miestas);

            Console.WriteLine("Žaidėjai žaidę mieste " + miestas
                + " visuose komandos rungtynėse ir surinkę ne mažiau vidutiniškai taškų:\n");
            p.SpausdintiZaidejus(atrinkti);
            Console.WriteLine();
        }

        private void SpausdintiZaidejus(ZaidejuKont zaidejai)
        {
            for(int i = 0; i < zaidejai.GautiKieki(); i++)
            {
                Console.WriteLine(zaidejai.Imti(i).ToString());
            }
        }

        private void AtrinktiZaidejus(ZaidejuKont atrinkti, ZaidejuKont zaidejai, List<Komanda> komandos, string miestas)
        {
            for(int i = 0; i < zaidejai.GautiKieki(); i++)
            {
                Zaidejas zaidejas = zaidejai.Imti(i);

                foreach(Komanda komanda in komandos)
                {
                    if(komanda.Miestas == miestas && zaidejas >= komanda)
                    {
                        atrinkti.Prideti(zaidejas);
                        break;
                    }
                }
            }
        }

        private void RastiVidurki(ZaidejuKont zaidejai, List<Komanda> komandos)
        {
            foreach (Komanda komanda in komandos)
            {
                int bendras = 0;

                for (int i = 0; i < zaidejai.GautiKieki(); i++)
                {
                    if(zaidejai.Imti(i) == komanda)
                        bendras += zaidejai.Imti(i).Taskai;
                }

                komanda.NustatytiVidurki(bendras / zaidejai.GautiKieki());

            }
        }

        private void NuskaitytiZaidejus(ZaidejuKont zaidejai, string failas)
        {
            string[] linijos = File.ReadAllLines(failas);

            foreach (string linija in linijos)
            {
                string[] duomenys = linija.Split(',');
                string komanda = duomenys[0];
                string vardas = duomenys[1];
                string pavarde = duomenys[2];
                DateTime gimimoData = DateTime.Parse(duomenys[3]);
                int rungtynes = int.Parse(duomenys[4]);
                int taskai = int.Parse(duomenys[5]);

                switch (duomenys.Length)
                {
                    case 7:
                        int geltonos = int.Parse(duomenys[6]);
                        Futbolininkas f = new Futbolininkas(komanda, vardas, pavarde, gimimoData, rungtynes, taskai, geltonos);
                        zaidejai.Prideti(f);
                        break;

                    case 8:
                        int atkovoti = int.Parse(duomenys[6]);
                        int perdavimai = int.Parse(duomenys[7]);
                        Krepsininkas k = new Krepsininkas(komanda, vardas, pavarde, gimimoData, rungtynes, taskai, atkovoti, perdavimai);
                        zaidejai.Prideti(k);
                        break;
                }
            }
        }

        private void NuskaitytiKomandas(List<Komanda> komandos, string failas)
        {
            string[] linijos = File.ReadAllLines(failas);

            foreach (string linija in linijos)
            {
                //komandos pavadinimas, miestas, komandos treneris, žaistų rungtynių skaičius
                string[] duomenys = linija.Split(',');
                string pavadinimas = duomenys[0];
                string miestas = duomenys[1];
                string treneris = duomenys[2];
                int rungtynes = int.Parse(duomenys[3]);

                Komanda komanda = new Komanda(pavadinimas, miestas, treneris, rungtynes);
                komandos.Add(komanda);
            }
        }

        class ZaidejuKont
        {
            private Zaidejas[] Zaidejai;
            private int Kiekis;

            public ZaidejuKont(int dydis)
            {
                Zaidejai = new Zaidejas[dydis];
                Kiekis = 0;
            }

            public int GautiKieki()
            {
                return Kiekis;
            }

            public void Prideti(Zaidejas zaidejas)
            {
                Zaidejai[Kiekis++] = zaidejas;
            }

            public Zaidejas Imti(int i)
            {
                return Zaidejai[i];
            }
        }

        class Komanda
        {
            //komandos pavadinimas, miestas, komandos treneris, žaistų rungtynių skaičius
            public string Pavadinimas { get; private set; }
            public string Miestas { get; private set; }
            public string Treneris { get; private set; }
            public int Rungtynes { get; private set; }
            public int Vidurkis { get; private set; }

            public Komanda(string pavadinimas, string miestas, string treneris, int rungtynes)
            {
                Pavadinimas = pavadinimas;
                Miestas = miestas;
                Treneris = treneris;
                Rungtynes = rungtynes;
            }

            public void NustatytiVidurki(int vidurkis)
            {
                Vidurkis = vidurkis;
            }
        }

        class Futbolininkas : Zaidejas
        {
            public int Geltonos { get; private set; }

            public Futbolininkas(string komanda, string vardas, string pavarde, DateTime gimimoData, int rungtynes,
                int taskai, int geltonos)
                : base(komanda, vardas, pavarde, gimimoData, rungtynes, taskai)
            {
                Geltonos = geltonos;
            }
        }

        class Krepsininkas : Zaidejas
        {
            public int Atkovoti { get; private set; }
            public int Perdavimai { get; private set; }

            public Krepsininkas(string komanda, string vardas, string pavarde, DateTime gimimoData, int rungtynes,
                int taskai, int atkovoti, int perdavimai)
                : base(komanda, vardas, pavarde, gimimoData, rungtynes, taskai)
            {
                Atkovoti = atkovoti;
                Perdavimai = perdavimai;
            }
        }

        abstract class Zaidejas
        {
            public string Komanda { get; private set; }
            public string Vardas { get; private set; }
            public string Pavarde { get; private set; }
            public DateTime GimimoData { get; private set; }
            public int Rungtynes { get; private set; }
            public int Taskai { get; private set; }

            public Zaidejas(string komanda, string vardas, string pavarde, DateTime gimimoData, int rungtynes, int taskai)
            {
                Komanda = komanda;
                Vardas = vardas;
                Pavarde = pavarde;
                GimimoData = gimimoData;
                Rungtynes = rungtynes;
                Taskai = taskai;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                return base.Equals(obj);
            }

            public static bool operator ==(Zaidejas lhs, Komanda rhs)
            {
                return (lhs.Komanda == rhs.Pavadinimas);
            }

            public static bool operator !=(Zaidejas lhs, Komanda rhs)
            {
                return (lhs.Komanda != rhs.Pavadinimas);
            }

            public static bool operator >=(Zaidejas lhs, Komanda rhs)
            {
                return (lhs.Komanda == rhs.Pavadinimas) && (lhs.Rungtynes == rhs.Rungtynes) && (lhs.Taskai >= rhs.Vidurkis);
            }

            public static bool operator <=(Zaidejas lhs, Komanda rhs)
            {
                return (lhs.Komanda != rhs.Pavadinimas) || (lhs.Rungtynes != rhs.Rungtynes) || (lhs.Taskai <= rhs.Vidurkis);
            }

            public override string ToString()
            {
                return String.Format("{0, -20}   {1, -25}   {2:yyyy - MM - dd}   Pelnė taškų: {3, -3}",
                    Komanda, Vardas + " " + Pavarde, GimimoData, Taskai);
            }
        }
    }
}
