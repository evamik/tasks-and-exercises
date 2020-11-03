using System;
using System.Collections.Generic;
using System.IO;

namespace Individuoli_uzduotis
{
    class Program
    {

        private const int KomandosDydis = 5;
        private const int RatoDydis = 2;

        static void Main(string[] args)
        {

            Program p = new Program();

            Ratas[] ratai = p.RatuRadimas(); // 

            Zaidejas AktyviausiasJungle = p.AktyviausiasZaidejasPozicijoje(ratai, "Jungle");
            p.SpausdintiAktyviausiaZaideja(AktyviausiasJungle);

            Ratas[] geriausiosKomandos = p.GeriausiosKomandosRatuose(ratai);
            p.SpausdintiGeriausiasKomandas(geriausiosKomandos);

            Ratas zaidejuPakitimai = p.PakitimuRadimas(ratai);
            p.IssaugotiPakitimus(zaidejuPakitimai, "Pasikeitimai.csv");

            string[] Cempionai = p.PanaudotiCempionai(ratai);
            p.IssaugotiCempionus(Cempionai, "Čempionai.csv");
        }


        /// Išsaugo čempionų vardų sąrašą į failą
        /// <param name="cempionai"> string mąsyvas su čempionų vardais</param>
        /// <param name="failas"> failas į kurį išsaugo </param>
        void IssaugotiCempionus(string[] cempionai, string failas)
        {
            File.Delete(@failas);
            using(var fr = File.AppendText(@failas))
            {
                foreach (string cempionas in cempionai)
                    fr.WriteLine(cempionas);
            }
        }

        /// Suranda visus panaudotus čempionus ratuose
        /// <param name="ratai"> Ratų mąsyvas </param>
        /// <returns> Čempionų pavadinimų mąsyvas </returns>
        string[] PanaudotiCempionai(Ratas[] ratai)
        {
            List<string> cempionai = new List<string>();
            // Eina per visus ratus ir ratuose esančius žaidėjus ir tikrina jų čempionus
            for(int i = 0; i < ratai.Length; i++) {
                for(int j = 0; j < ratai[i].GautiKieki(); j++)
                {
                    string cempionas = ratai[i].Zaidejas(j).Cempionas;
                    if (!cempionai.Contains(cempionas))
                        cempionai.Add(cempionas);
                }
            }
            return cempionai.ToArray();
        }

        /// Išsaugo pakitusius žaidėjus į failą iš rato konteinerio.
        /// <param name="ratas"> Rato konteineris </param>
        /// <param name="failas"> Failo pavadinimas į kurį išsaugo </param>
        void IssaugotiPakitimus(Ratas ratas, string failas)
        {
            File.Delete(@failas); // Ištrina failą, jei jau yra.
            using (var fr = File.AppendText(@failas))
            {
                for (int i = 0; i < ratas.GautiKieki(); i += 2)
                {
                    fr.WriteLine("{0} pakeitė žaidėjas {1}", ratas.Zaidejas(i).ToStringShort()
                        , ratas.Zaidejas(i+1).ToStringShort());
                }
            }
            
        }

        /// Suranda pakitusius žaidėjus ir juos sudeda į rato 
        /// klasę vienas po kito (pakeistas, pakeitėjas, ...)
        /// <param name="ratai"> Ratų mąsyvas </param>
        /// <returns> Rato klasė su pakitimo duomenimis </returns>
        Ratas PakitimuRadimas(Ratas[] ratai)
        {
            Ratas ratas = new Ratas(20, 0);

            for(int i = 0; i < ratai.Length-1; i++) 
                // mąsyvas eina per ratus iki paskutinio (ne imtinai)
            {
                Zaidejas[] zaidejai = ZaidejuMąsyvasRate(ratai[i]); 
                // Žaidėjų mąsyvas tikriname rate

                for(int j = 0; j < ratai[i+1].GautiKieki(); j++) 
                    // mąsyvas eina per sekančio rato žaidėjus
                {
                    Zaidejas zaidejas = ratai[i + 1].Zaidejas(j); 
                    // Žaidėjas iš sekančio rato
                    if (zaidejai[j].Pavadinimas != zaidejas.Pavadinimas) 
                        // Tikrinama ar žaidėjai yra toje pačioje komandoje
                        continue;
                    if(zaidejas != zaidejai[j]) // Jei kitame rate žaidėjas kitoks
                    {
                        ratas.Prideti(zaidejai[j]); // Pridedamas pakeistas zaidėjas
                        ratas.Prideti(zaidejas); // Jo pakeitėjas
                    }
                }
            }

            return ratas;
        }

        /// Išrenka žaidėjų mąsyvą iš rato
        /// <param name="ratas"> Rato klasė </param>
        /// <returns> Žaidėjų mąsyvas </returns>
        Zaidejas[] ZaidejuMąsyvasRate(Ratas ratas)
        {
            Zaidejas[] zaidejai = new Zaidejas[ratas.GautiKieki()];
            for(int i = 0; i < ratas.GautiKieki(); i++)
            {
                zaidejai[i] = ratas.Zaidejas(i);
            }
            return zaidejai;
        }

        void SpausdintiGeriausiasKomandas(Ratas[] ratai)
        {
            foreach (Ratas ratas in ratai)
            {
                Console.WriteLine("{0} {1}", ratas.Numeris(), ratas.Zaidejas(0).Pavadinimas);
            }
            Console.WriteLine();
        }

        void SpausdintiAktyviausiaZaideja(Zaidejas zaidejas)
        {
            Console.WriteLine("Aktyviausias {0} zaidejas:\n{1}", zaidejas.Pozicija, zaidejas.ToStringShort());
            Console.WriteLine();
        }

        /// Suranda geriausiai bendradarbiavusią komandą rate
        /// <param name="ratai"> Ratų mąsyvas </param>
        /// <returns> Ratas su vienu žaidėju iš geriausios komandos
        /// ir susumuotais komandos narių taškais </returns>
        Ratas[] GeriausiosKomandosRatuose(Ratas[] ratai)
        {
            Ratas[] geriausiosKomandos = new Ratas[ratai.Length];
            for(int i = 0; i < ratai.Length; i++)
            {
                Zaidejas komanda1 = ratai[i].Zaidejas(0);
                Zaidejas komanda2 = ratai[i].Zaidejas(ratai[i].GautiKieki() - 1);
                for(int j = 1; j < ratai[i].GautiKieki()-1; j++)
                {
                    if (j < KomandosDydis) komanda1 += ratai[i].Zaidejas(j);
                    else komanda2 += ratai[i].Zaidejas(j);
                }

                geriausiosKomandos[i] = new Ratas(1, i + 1);

                if (komanda1 > komanda2)
                    geriausiosKomandos[i].Prideti(komanda1);
                else if (komanda2 > komanda1)
                    geriausiosKomandos[i].Prideti(komanda1);
            }
            return geriausiosKomandos;
        }

        /// Suranda duomenų failus ir užpildo juos ratų duomenimis
        /// <returns> Ratų mąsyvas </returns>
        Ratas[] RatuRadimas()
        {
            string[] failai = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.csv");
            string[] duomenys = RataiIsFailu(failai);
            Ratas[] ratai = new Ratas[duomenys.Length];
            for (int i = 0; i < duomenys.Length; i++)
            {
                Ratas ratas = RatoNuskaitymas(duomenys[i]);
                ratai[ratas.Numeris()-1] = ratas;
            }
            return ratai;
        }

        /// Išrenka failus, kuriuose pirma eilutė yra skaičius
        /// <param name="failai"> Failų nuorodų mąsyvas </param>
        /// <returns> Išrinktas failų nuorodų mąsyvas </returns>
        string[] RataiIsFailu(string[] failai)
        {
            List<string> duomenys = new List<string>();
            foreach(string failas in failai)
            {
                int skaicius;
                string[] eilutes = File.ReadAllLines(failas);
                if (eilutes.Length == 0)
                    continue;

                if (int.TryParse(eilutes[0], out skaicius))
                    duomenys.Add(failas);
            }
            return duomenys.ToArray();
        }

        /// Nuskaito duomenų failą
        /// <param name="fd"> duomenų failas </param>
        /// <returns> Ratą ir informacija apie komandas rate </returns>
        Ratas RatoNuskaitymas(string fd)
        {
            string[] lines = File.ReadAllLines(fd);
            int ratoNr = int.Parse(lines[0]);
            string data = lines[1];
            
            Ratas ratas = new Ratas(lines.Length-2, ratoNr);

            KomandosNuskaitymas(ratas, lines, 2); // Pirmos komandos nuskaitymas
            KomandosNuskaitymas(ratas, lines, 7); // Antros komandos nuskaitymas

            return ratas;
        }

        /// Nuskaito komandos sudetį iš masyvo ir papildo ratą
        /// <param name="ratas"> Nuskaitomas ratas </param>
        /// <param name="lines"> Nuskaitomas mąsyvas </param>
        /// <param name="pradziosIndeksas"> Nuo kurios vietos mąsyve skaityti </param>
        /// <param name="ratoNr"> Rato numeris </param>
        void KomandosNuskaitymas(Ratas ratas, string[] lines, int pradziosIndeksas)
        {
            for (int i = pradziosIndeksas; i < pradziosIndeksas+KomandosDydis; i++)
            {
                string line = lines[i];
                string[] values = line.Split(';');
                string vardas = values[0];
                string pavarde = values[1];
                string pavadinimas = values[2];
                string pozicija = values[3];
                string cempionas = values[4];
                int sunaikinimai = int.Parse(values[5]);
                int asistai = int.Parse(values[6]);

                Zaidejas zaidejas = new Zaidejas(vardas, pavarde, pavadinimas
                    , pozicija, cempionas, sunaikinimai, asistai);
                ratas.Prideti(zaidejas);
            }
        }

        /// Randa daugiausiai sunaikinimu+asistu turintį žaidėja
        /// iš visų ratų, žaidžiantį pasirinktoje pozicijoje
        /// <param name="ratai"> Ratų mąsyvas </param>
        /// <param name="pozicija"> Ieškoma pozicija </param>
        /// <returns> Aktyviausias žaidėjas </returns>
        Zaidejas AktyviausiasZaidejasPozicijoje(Ratas[] ratai, string pozicija)
        {
            List<Zaidejas> zaidejai = ZaidejaiPozicijoje(ratai, pozicija);
            Zaidejas aktyviausias = new Zaidejas();
            foreach (Zaidejas zaidejas in zaidejai)
            {
                if (zaidejas > aktyviausias)
                    aktyviausias = zaidejas;
            }
            return aktyviausias;
        }

        /// Surenka žaidėjus iš visų ratų pagal poziciją
        /// <param name="ratai"> Ratų mąsyvas </param>
        /// <param name="pozicija"> Ieškoma pozicija </param>
        /// <returns> Žaidėjų sąrašas </returns>
        List<Zaidejas> ZaidejaiPozicijoje(Ratas[] ratai, string pozicija)
        {
            List<Zaidejas> zaidejai = new List<Zaidejas>();
            foreach (Ratas ratas in ratai)
            {
                for (int i = 0; i < ratas.GautiKieki(); i++)
                {
                    Zaidejas zaidejas = ratas.Zaidejas(i);
                    if (zaidejas.Pozicija == pozicija)
                    {
                        int indeksas = ZaidejoIndeksas(zaidejai.ToArray(), zaidejas);
                        if (indeksas == -1) zaidejai.Add(zaidejas);
                        else zaidejai[indeksas] += zaidejas;
                    }
                }
            }
            return zaidejai;
        }

        /// Suranda žaidėjo indeksą mąsyve
        /// <param name="zaidejai"> Žaidėjų mąsyvas </param>
        /// <param name="zaidejas"> Ieškomas žaidėjas </param>
        /// <returns> Žaidėjo indeksas mąsyve arba -1, jei nėra </returns>
        public int ZaidejoIndeksas(Zaidejas[] zaidejai, Zaidejas zaidejas)
        {
            for (int i = 0; i < zaidejai.Length; i++)
            {
                if (zaidejai[i] == zaidejas)
                    return i;
            }
            return -1;
        }
    }
}

