using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace L4
{
    class Program
    {
        static void Main(string[] args)
        {
            const string duomenys = "..\\..\\Knyga.txt";
            const string rodikliai = "..\\..\\Rodikliai.txt";
            const string manoKnyga = "..\\..\\ManoKnyga.txt";
            const string fragmentoFailas = "..\\..\\Fragmentas.txt";
            char[] skyrikliai = { ' ', '.', ',', '!', '?', ':', ';', '(', ')', '\t', '–', '"', '-' };
            Program p = new Program();

            string[] eilutes = File.ReadAllLines(duomenys, Encoding.GetEncoding(1257));

            // Pirmas punktas
            Konteineris zodziai = p.Atrinkti(eilutes, skyrikliai, 10);
            File.Delete(rodikliai);
            p.IssaugotiKonteineri(rodikliai, zodziai);

            // Antras punktas
            Regex zodziams = new Regex("(?:)["+new string(skyrikliai)+"]+");
            Regex skyrikliams = new Regex(@"[a-zA-ZĄąČčĘęĖėĮįŠšŲųŪūŽž]+");
            int maxZodziuEiluteje = 0;
            Zodis[] visiZodziai = p.VisiZodziai(eilutes, zodziams, skyrikliams, out maxZodziuEiluteje);

            Fragmentas fragmentas = p.RastiFragmenta(eilutes, visiZodziai);
            File.AppendAllText(rodikliai, String.Format("Fragmentas \"{0}\" prasidedantis eilutėje {1} ir pasibaigiantis {2}",
                fragmentas.Fragment, fragmentas.Pradzia, fragmentas.Pabaiga));

            // Sulygiavimas
            int p1 = 1;
            Konteineris[] stulpeliai = p.ZodziaiStulpeliais(visiZodziai, maxZodziuEiluteje);
            int eiluciuKiekis = visiZodziai[visiZodziai.Length - 1].Eilute;
            p.Sulygiavimas(stulpeliai, p1, maxZodziuEiluteje, eiluciuKiekis);
            p.IssaugotiTeksta(visiZodziai, manoKnyga);
        }

        /// <summary>
        /// Išsaugo tekstą į failą
        /// </summary>
        /// <param name="zodziai"> Žodžių masyvas </param>
        /// <param name="failas"></param>
        void IssaugotiTeksta(Zodis[] zodziai, string failas)
        {
            int eilute = 1;
            File.Delete(failas);
            using (var fr = File.AppendText(failas))
            {
                for (int i = 0; i < zodziai.Length-1; i++)
                {
                    Zodis zodis = zodziai[i];
                    fr.Write(zodis.ToString2());
                    for(;eilute < zodziai[i+1].Eilute; eilute++)
                    {
                        fr.Write("\r\n");
                    }
                }
            }
        }

        /// <summary>
        /// Sulygiouja visus žodžius
        /// </summary>
        /// <param name="stulpeliai"> žodžių stulpelių masyvas </param>
        /// <param name="p1"> pirmo stulpelio poslynkis </param>
        /// <param name="maxZodziuEiluteje"></param>
        /// <param name="eiluciuKiekis"></param>
        void Sulygiavimas(Konteineris[] stulpeliai, int p1, int maxZodziuEiluteje, int eiluciuKiekis)
        {
            int[] p = new int[maxZodziuEiluteje];
            p[0] = p1;

            for (int i = 0; i < maxZodziuEiluteje - 1; i++)
            {
                p[i+1] = Pozicija(stulpeliai[i], maxZodziuEiluteje, eiluciuKiekis);
            }

            Iterpimas(stulpeliai, p, maxZodziuEiluteje, eiluciuKiekis);
        }

        /// <summary>
        /// Suranda stulpelio reikiamą minimalų poslynkį
        /// </summary>
        /// <param name="stulpelis"> Žodžių stulpelis </param>
        /// <param name="maxZodziuEiluteje"></param>
        /// <param name="eiluciuKiekis"></param>
        /// <returns></returns>
        int Pozicija(Konteineris stulpelis, int maxZodziuEiluteje, int eiluciuKiekis)
        {
            int p = 0;
            int[] ilgiai = new int[eiluciuKiekis];
            for (int j = 0; j < eiluciuKiekis; j++) // stulpelyje žodžių ciklas
            {
                Zodis zodis = stulpelis.Imti(j);
                ilgiai[j] = zodis.Pavadinimas.Length + zodis.Skyriklis.Length + 1;
                if (ilgiai[j] > p)
                    p = ilgiai[j];
            }
            return p;
        }

        /// <summary>
        /// Įterpia tarpus į žodžius, kad lygiuotos jų pabaiga su ilgiausio žodžio pabaiga tame pačiame stulpelyje
        /// </summary>
        /// <param name="stulpeliai"> Žodžių stulpelių masyvas </param>
        /// <param name="p"> Stulpelių poslynkiai </param>
        /// <param name="maxZodziuEiluteje"></param>
        /// <param name="eiluciuKiekis"></param>
        void Iterpimas(Konteineris[] stulpeliai, int[] p, int maxZodziuEiluteje, int eiluciuKiekis)
        {
            for(int i = 0; i < maxZodziuEiluteje-1; i++)
            {
                for (int j = 0; j < eiluciuKiekis; j++)
                {
                    Zodis zodis = stulpeliai[i].Imti(j);
                    zodis.Iterpimas(zodis.Skyriklis.PadRight(p[i + 1] - zodis.Pavadinimas.Length));
                    if (i == 0)
                    {
                        zodis.Iterpimas(p[0]);
                    }
                }
            }
        }

        /// <summary>
        /// Sudeda žodžius į konteinerių masyvą pagal žodžių stulpelius išskyrus paskutinį žodį
        /// </summary>
        /// <param name="zodziai"> Žodžių masyvas </param>
        /// <returns> Konteinerių masyvas </returns>
        Konteineris[] ZodziaiStulpeliais(Zodis[] zodziai, int maxZodziuEiluteje)
        {
            List<Konteineris> stulpeliai = new List<Konteineris>();

            int zodis = 0;
            for(int i = 0; i < zodziai[zodziai.Length - 1].Eilute; i++) // ciklas keliaujantis pro eilutes
            {
                for(int j = 0; j < maxZodziuEiluteje; j++) // ciklas keliaujantis pro stulpelius
                {
                    if(i==0) // pirmoje eilutėje sukurti stulpelių konteinerius
                        stulpeliai.Add(new Konteineris(maxZodziuEiluteje));
                    if (zodis > zodziai.Length - 1) // jei žodžių daugiau nebėra, likusius pridėti tuščius
                        stulpeliai[j].Prideti(new Zodis(" ", " ", i + 1));
                    else if (zodziai[zodis].Eilute == i + 1) // jei sutampa žodžio eilutė su tikrinama eilute
                    {
                        stulpeliai[j].Prideti(zodziai[zodis]);
                        zodis++;
                    }
                    // jei eilutėje tame stulpelyje nėra žodžio, tai sukurti "tuščią" žodį
                    else stulpeliai[j].Prideti(new Zodis(" ", " ", i + 1));
                }
            }

            return stulpeliai.ToArray();
        }

        /// <summary>
        /// Randa ilgiausią teksto fragmentą sudarytą iš žodžių
        /// </summary>
        /// <param name="eilutes"> teksto eilučių string masyvas </param>
        /// <param name="zodziams"> Regex žodžiams išrinkti </param>
        /// <param name="skyrikliams"> Regex skyrikliams išrinkti </param>
        /// <returns> fragmento duomenys </returns>
        public Fragmentas RastiFragmenta(string[] eilutes, Zodis[] zodziai)
        {
            StringBuilder fragmentas = new StringBuilder();
            int kiekis = 0;
            int pradzia = 0;
            int pabaiga = 0;
            string paskutineRaide = "";
            string skyriklisTarp = "";

            string ilgiausias = "";
            int ilgiausioKiekis = 0;
            int ilgiausioPradzia = 0;
            int ilgiausioPabaiga = 0;

            foreach(Zodis zodisKon in zodziai)
            {
                string zodis = zodisKon.Pavadinimas;
                string skyriklis = zodisKon.Skyriklis;
                int eilute = zodisKon.Eilute; 

                if (fragmentas.Length == 0 || zodis[0].ToString().ToLower() == paskutineRaide)
                {
                    if (fragmentas.Length == 0)
                    {
                        pradzia = eilute;
                        pabaiga = eilute;
                    }
                    if (eilute > pabaiga)
                        skyriklisTarp += " ";
                    pabaiga = eilute;
                    fragmentas.Append(skyriklisTarp);
                    fragmentas.Append(zodis);
                    kiekis++;
                    paskutineRaide = zodis[zodis.Length - 1].ToString().ToLower();
                    skyriklisTarp = skyriklis;
                }
                else
                {
                    if (kiekis > ilgiausioKiekis)
                    {
                        ilgiausias = fragmentas.ToString();
                        ilgiausioPradzia = pradzia;
                        ilgiausioPabaiga = pabaiga;
                        ilgiausioKiekis = kiekis;
                    }

                    kiekis = 0;
                    fragmentas.Clear();
                    skyriklisTarp = "";
                }
            }

            return new Fragmentas(ilgiausias.ToString(), ilgiausioPradzia, ilgiausioPabaiga);
        }

        /// <summary>
        /// Konvertuoja tekstą į Zodis klasės masyvą
        /// </summary>
        /// <param name="eilutes"> teksto eilučių string masyvas </param>
        /// <param name="zodziams"> Regex žodžiams išrinkti </param>
        /// <param name="skyrikliams"> Regex skyrikliams išrinkti </param>
        /// <returns> Zodis konstruktoriaus masyvas </returns>
        Zodis[] VisiZodziai(string[] eilutes, Regex zodziams, Regex skyrikliams, out int maxZodziuEiluteje)
        {
            List<Zodis> visiZodziai = new List<Zodis>();
            maxZodziuEiluteje = 0;

            for(int i = 0; i < eilutes.Length; i++)
            {
                string[] zodziai = zodziams.Split(eilutes[i]);
                // jei paskutinis žodis yra tuščias sumažinti žodžių masyvą
                if (zodziai[zodziai.Length - 1] == "")
                    Array.Resize(ref zodziai, zodziai.Length - 1);

                string[] skyrikliai = skyrikliams.Split(eilutes[i]);

                for(int j = 0; j < zodziai.Length; j++)
                {
                    string skyriklis = "";
                    if (j < skyrikliai.Length)
                        skyriklis = skyrikliai[j + 1];
                    Zodis zodis = new Zodis(zodziai[j], skyriklis, i+1);
                    visiZodziai.Add(zodis);
                    if (maxZodziuEiluteje < j + 1)
                        maxZodziuEiluteje = j + 1;
                }
            }

            return visiZodziai.ToArray();
        }

        /// <summary>
        /// Išsaugo konteinerio duomenis į failą
        /// </summary>
        /// <param name="failas"> Failo pavadinimas </param>
        /// <param name="kont"> Konteineris </param>
        public void IssaugotiKonteineri(string failas, Konteineris kont)
        {
            using(var fr = File.AppendText(failas))
            {
                for(int i = 0; i < kont.GautiKieki(); i++)
                    fr.WriteLine(kont.Imti(i).ToString());
            }
        }

        /// <summary>
        /// Atrenka žodžius, kurie prasideda iš mažosios raidės ir baigiasi raide 's' ir sudeda jų kiekį
        /// </summary>
        /// <param name="eilutes"> Teksto eilutės </param>
        /// <param name="skyrikliai"> Skiriamieji ženklai </param>
        /// <param name="kiekis"> Skirtingų žodžių maksimalus kiekis </param>
        /// <returns> Žodžių konteineris </returns>
        Konteineris Atrinkti(string[] eilutes, char[] skyrikliai, int kiekis)
        {
            Konteineris zodziai = new Konteineris(kiekis);

            for(int i = 0; i < eilutes.Length; i++)
            {
                string[] eilZodziai = eilutes[i].Split(skyrikliai, StringSplitOptions.RemoveEmptyEntries);
                foreach (string zodis in eilZodziai)
                {
                    if (char.IsLower(zodis[0]) && zodis[zodis.Length - 1] == 's') {
                        Zodis rastas = new Zodis(zodis);
                        zodziai.PridetiKiekiui(rastas);
                     }
                }
            }
            return zodziai;
        }
    }
}
