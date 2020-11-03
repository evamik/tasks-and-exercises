using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace programa

{
    class Program
    {
        const string LentelesKrastine = "--------------------------------------------------------------------------------------------";
        const string LentelesAntraste = "| Pavadinimas      | Tipas        | Leidykla             | Metai      | Lapai    | Tirazas |";

        static void Main(string[] args)
        {                     
            Program p = new Program();
            Filialas[] filialai = new Filialas[2];
            AutoriuKonteineris autoriai = new AutoriuKonteineris();
            filialai[0] = p.Skaito("Pirmasfilialas.txt");
            filialai[1] = p.Skaito("Antrasfilialas.txt");

            //pirmas punktas
            p.SpausdintiSenuKiekiFiliale(filialai[0], p.SenuLeidiniu(filialai[0], 2));
            p.SpausdintiSenuKiekiFiliale(filialai[1], p.SenuLeidiniu(filialai[1], 2));

            //antras punktas
            List<Leidinys> moksliniai = p.MoksliniaiLeidiniai(filialai);
            p.SpausdintiMokslinius(moksliniai);

            //tračias punktas
            List<Leidinys> leidiniai = p.VisiLeidiniai(filialai);
            p.Rikiuoti(leidiniai);
            p.IssaugotiLeidinius(leidiniai, @"Leidiniai.csv");

            //ketvirtas punktas
            List<Leidinys> populiarus = p.PopuliarusLeidiniai(leidiniai, 10000);
            p.IssaugotiPavadinimus(populiarus, @"PopuliarūsLeidiniai.csv");
        }

        void IssaugotiPavadinimus(List<Leidinys> leidiniai, string failas)
        {
            File.Delete(failas);
            using(var fr = File.AppendText(failas))
            {
                foreach(Leidinys leidinys in leidiniai)
                    fr.WriteLine(leidinys.Pavadinimas);
            }
        }

        /// <summary>
        /// Atrenka leidinius, kurių tiražas viršija nustatytą
        /// </summary>
        /// <param name="leidiniai"> leidinių sąrašas </param>
        /// <param name="tirazas"> tiražas </param>
        /// <returns> leidinių sąrašas </returns>
        List<Leidinys> PopuliarusLeidiniai(List<Leidinys> leidiniai, int tirazas)
        {
            List<Leidinys> populiarus = new List<Leidinys>();

            foreach(Leidinys leidinys in leidiniai)
            {
                if (leidinys.Tirazas > tirazas)
                    populiarus.Add(leidinys);
            }

            return populiarus;
        }

        void IssaugotiLeidinius(List<Leidinys> leidiniai, string failas)
        {
            File.Delete(failas);
            using(var fr = File.AppendText(failas))
            {
                fr.WriteLine(LentelesKrastine);
                fr.WriteLine(LentelesAntraste);
                fr.WriteLine(LentelesKrastine);

                foreach (Leidinys leidinys in leidiniai)
                {
                    fr.WriteLine(leidinys.ToString());
                    fr.WriteLine(LentelesKrastine);
                }
            }
        }

        /// <summary>
        /// Surikiuoja leidinius mažėjimo tvarka naudojant >= operatorių
        /// </summary>
        /// <param name="leidiniai"> leidinių sąrašas </param>
        void Rikiuoti(List<Leidinys> leidiniai)
        {
            for(int i = 0; i < leidiniai.Count-1; i++)
            {
                for (int j = i + 1; j < leidiniai.Count; j++)
                {
                    if (leidiniai[j] <= leidiniai[i])
                    {
                        Leidinys temp = leidiniai[j];
                        leidiniai[j] = leidiniai[i];
                        leidiniai[i] = temp;
                    }
                }
            }
        }

        /// <summary>
        /// Išrenka visus leidinius iš filialų
        /// </summary>
        /// <param name="filialai"> filialų masyvas </param>
        /// <returns> Leidinių sąrašas </returns>
        List<Leidinys> VisiLeidiniai(Filialas[] filialai)
        {
            List<Leidinys> leidiniai = new List<Leidinys>();
            
            foreach(Filialas filialas in filialai)
            {
                Biblioteka bibl = filialas.Biblioteka;
                for(int i = 0; i < bibl.Kiekis; i++)
                {
                    leidiniai.Add(bibl.Imti(i));
                }
            }

            return leidiniai;
        }

        void SpausdintiMokslinius(List<Leidinys> leidiniai)
        {
            Console.WriteLine();
            Console.WriteLine("Moksliniai leidiniai:");

            Console.WriteLine(LentelesKrastine);
            Console.WriteLine(LentelesAntraste);
            Console.WriteLine(LentelesKrastine);

            foreach (Leidinys leidinys in leidiniai)
            {
                Console.WriteLine(leidinys.ToString());
                Console.WriteLine(LentelesKrastine);
            }
        }
        
        /// <summary>
        /// Iš visų filialų suranda leidinius, kurių tipas yra "Mokslinis"
        /// </summary>
        /// <param name="filialai"> filialų masyvas </param>
        /// <returns> Mokslinių leidinių sąrašas </returns>
        List<Leidinys> MoksliniaiLeidiniai(Filialas[] filialai)
        {
            List<Leidinys> leidiniai = new List<Leidinys>();
            foreach(Filialas filialas in filialai)
            {
                Biblioteka bibl = filialas.Biblioteka;
                for(int i = 0; i < bibl.Kiekis; i++)
                {
                    Leidinys leidinys = bibl.Imti(i);
                    if (leidinys.Tipas == "Mokslinis")
                        leidiniai.Add(leidinys);
                }
            }
            return leidiniai;
        }

        /// <summary>
        /// Nuskaito duomenu failus
        /// </summary>
        /// <param name="file"> duomenu failas </param>
        /// <returns> Filialo duomenys </returns>
        Filialas Skaito(string file)
        {

            string[] sarasas = File.ReadAllLines(@file);
            int length = sarasas.Length;

            string filialoPavadinimas = sarasas[0];
            string adresas = sarasas[1];
            int telefonoNumeris = int.Parse(sarasas[2]);

            Filialas filialas = new Filialas(filialoPavadinimas, adresas, telefonoNumeris, new Biblioteka());

            for (int i = 3; i < length; i++)
            {
                string[] values = sarasas[i].Split(';');
                string pavadinimas = values[0];
                string tipas = values[1];
                string leidykla = values[2];
                int metai = int.Parse(values[3]);
                int pskaicius = int.Parse(values[4]);
                int tirazas = int.Parse(values[5]);

                Leidinys leidinys;
                DateTime data;
                if(DateTime.TryParse(values[6], out data))
                {
                    int numeris = int.Parse(values[7]);
                    leidinys = new Laikrastis(data, numeris, pavadinimas
                        , tipas, leidykla, metai, pskaicius, tirazas);
                }
                else
                {
                    long isbn = long.Parse(values[6]);
                    int numeris;
                    if(int.TryParse(values[7], out numeris))
                    {
                        leidinys = new Zurnalas(isbn, numeris, pavadinimas
                            , tipas, leidykla, metai, pskaicius, tirazas);
                    }
                    else
                    {
                        string autorius = values[7];
                        leidinys = new Knyga(isbn, autorius, pavadinimas, tipas
                            , leidykla, metai, pskaicius, tirazas);
                    }
                }
                filialas.Biblioteka.Deti(leidinys);
            }
            return filialas;
        }

        /// <summary>
        /// Suskaičiuoja kiek leidinių yra išleistų prieš nurodytą metų kiekį
        /// </summary>
        /// <param name="filialas"> filialo duomenys </param>
        /// <param name="metai"> metų kiekis </param>
        /// <returns> Leidinių kiekis </returns>
        int SenuLeidiniu(Filialas filialas, int metai)
        {
            int kiekis = 0;
            Biblioteka bibl = filialas.Biblioteka;
            for (int i = 0; i < bibl.Kiekis; i++)
            {
                if (DateTime.Now.Year - bibl.Imti(i).Metai >= metai)
                    kiekis++;
            }
            return kiekis;
        }

        /// <summary>
        /// Spausdina kiek filiale yra senų leidinių
        /// </summary>
        /// <param name="filialas"> filialo duomenys </param>
        /// <param name="kiekis"> leidinių kiekis </param>
        void SpausdintiSenuKiekiFiliale(Filialas filialas, int kiekis)
        {
            Console.WriteLine("Filiale {0} yra {1} senų(-i) leidinių(-iai)", filialas.Pavadinimas, kiekis);
        }
    }
}