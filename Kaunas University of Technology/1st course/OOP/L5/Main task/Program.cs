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
        const string LentelesKrastine = "---------------------------------------------------------------------------------------------------------------------------";

        static void Main(string[] args)
        {                     
            Program p = new Program();
            Filialas[] filialai = new Filialas[2];
            AutoriuKonteineris autoriai = new AutoriuKonteineris();
            filialai[0] = p.Skaito("Pirmasfilialas.txt");
            filialai[1] = p.Skaito("Antrasfilialas.txt");

            //pirmas punktas
            int metuKiekis = 2;
            p.SpausdintiSenuKiekiFiliale(filialai[0], p.SenuLeidiniu(filialai[0], metuKiekis));
            p.SpausdintiSenuKiekiFiliale(filialai[1], p.SenuLeidiniu(filialai[1], metuKiekis));

            //antras punktas
            List<Leidinys> moksliniai = p.MoksliniaiLeidiniai(filialai);
            p.SpausdintiMokslinius(moksliniai);

            List<Leidinys> leidiniai = p.VisiLeidiniai(filialai);

            //trečias punktas
            List<Leidinys> nenauji = p.NenaujiLeidiniai(leidiniai);
            p.Rikiuoti(nenauji);
            p.IssaugotiLeidinius(nenauji, @"Nenauji.csv");

            //ketvirtas punktas
            int tirazas = 10000;
            List<Leidinys> populiarus = p.PopuliarusLeidiniai(leidiniai, tirazas);
            p.IssaugotiPavadinimus(populiarus, @"PopuliarūsLeidiniai.csv");
        }

        /// <summary>
        /// Surenka pasenusių leidinių sąrašą iš visų leidinių
        /// </summary>
        /// <param name="leidiniai"> Visi leidiniai </param>
        /// <returns> Nenaujų leidinių sąrašas </returns>
        List<Leidinys> NenaujiLeidiniai(List<Leidinys> leidiniai)
        {
            List<Leidinys> nenauji = new List<Leidinys>();

            foreach(Leidinys leidinys in leidiniai)
            {
                if (   (leidinys is Knyga    && (leidinys as Knyga).ArSenas()) 
                    || (leidinys is Zurnalas && (leidinys as Zurnalas).ArSenas())
                    || (leidinys is Laikrastis && (leidinys as Laikrastis).ArSenas()))
                    nenauji.Add(leidinys);
            }

            return nenauji;
        }

        /// <summary>
        /// Išsaugo leidinių pavadinimus į failą
        /// </summary>
        /// <param name="leidiniai"> Leidinių sąrašas </param>
        /// <param name="failas"> Failo pavadinimas </param>
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

        /// <summary>
        /// Išsaugo leidinius į sąrašą
        /// </summary>
        /// <param name="leidiniai"> Leidinių sąrašas </param>
        /// <param name="failas"> Failo pavadinimas </param>
        void IssaugotiLeidinius(List<Leidinys> leidiniai, string failas)
        {
            File.Delete(failas);
            using(var fr = File.AppendText(failas))
            {
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
                    if (   (leidiniai[j] is Knyga)      && (leidiniai[j] as Knyga) <= leidiniai[i]
                        || (leidiniai[j] is Zurnalas)   && (leidiniai[j] as Zurnalas) <= leidiniai[i]
                        || (leidiniai[j] is Laikrastis) && (leidiniai[j] as Laikrastis) <= leidiniai[i])
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

                string[] duomenys = sarasas[i].Split(';');

                if(DateTime.TryParse(duomenys[6], out var data))
                {
                    filialas.Biblioteka.Deti(new Laikrastis(duomenys));
                }
                else
                {
                    if(int.TryParse(duomenys[7], out var numeris))
                    {
                        filialas.Biblioteka.Deti(new Zurnalas(duomenys));
                    }
                    else
                    {
                        filialas.Biblioteka.Deti(new Knyga(duomenys));
                    }
                }
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