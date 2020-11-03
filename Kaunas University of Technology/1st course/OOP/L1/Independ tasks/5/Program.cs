using System;
using System.Collections.Generic;
using System.IO;

namespace S2._1
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();

            List<Tourist> tourists = p.ReadTouristData();

            int conSum = p.ContributionSum(tourists);

            int biggestCon = p.BiggestContribution(tourists);

            List<Tourist> mostConTourists = p.MostContributedTourists(tourists, biggestCon);

            p.PrintData(conSum, biggestCon, mostConTourists);
        }

        /// <summary>
        /// Grąžina turistų surinktą sumą
        /// </summary>
        /// <param name="tourists"> Turistai, iš kurių renkama suma </param>
        /// <returns> Suma kurią surinko (centais) </returns>
        int ContributionSum(List<Tourist> tourists)
        {
            int sum = 0;
            foreach(Tourist t in tourists)
            {
                // prie sumos pridedama kiek prisidėjo turistas
                sum += t.Con;
            }
            return sum;
        }

        /// <summary>
        /// Suranda didžiausią surinktą kiekį 
        /// </summary>
        /// <param name="tourists"> Turistai, iš kurių renkama suma </param>
        /// <returns> Didžiausias surinktas kiekis (centais) </returns>
        int BiggestContribution(List<Tourist> tourists)
        {
            int biggestCon = 0;
            foreach (Tourist t in tourists)
            {
                if (t.Con > biggestCon)
                    biggestCon = t.Con; // Priskiriamas didžiausias surinktas kiekis
            }
            return biggestCon;
        }
        /// <summary>
        /// Suranda visus turistus, kurie surinko pasirinktą sumą (centais)
        /// </summary>
        /// <param name="tourists"> Turistai, iš kurių renkama suma </param>
        /// <param name="con"> Suma, kurios ieškoma (centais) </param>
        /// <returns> Turistų sarašas, kurie surinko pasirinktą sumą </returns>
        List<Tourist> MostContributedTourists(List<Tourist> tourists, int con)
        {
            List<Tourist> mcTourists = new List<Tourist>();

            foreach (Tourist t in tourists)
            {
                // Pridedamas turistas prie daugiausiai prisidėjusių išlaidoms turistų sarašo,
                // jei tikrinamas turistas prisidėjo tinkamą kiekį sumos
                if (t.Con == con)
                    mcTourists.Add(t);
            }

            return mcTourists;
        }

        static int SumToEur(int sum)
        {
            return (int)Math.Floor((double)sum / 100);
        }

        static int SumToCt(int sum)
        {
            return sum % 100;
        }

        /// <summary>
        /// Turistų duomenų nuskaitymas iš failo
        /// </summary>
        /// <returns> Užpildytas turistų sąrašas </returns>
        List<Tourist> ReadTouristData()
        {
            List<Tourist> tourists = new List<Tourist>();

            string[] lines = File.ReadAllLines(@"S2_1Data.csv");
            foreach (string line in lines)
            {
                string[] values = line.Split(';');
                string name = values[0];
                int eur = int.Parse(values[1]);
                int ct = int.Parse(values[2]);

                Tourist t = new Tourist(name, eur, ct);
                tourists.Add(t);
            }

            return tourists;
        }
        /// <summary>
        /// Ekrane spausdina iš viso surinktą sumą, didžiausią sumą 
        /// ir turistus surinkusius didžiausią sumą
        /// </summary>
        /// <param name="con"> iš visu surinkta suma </param>
        /// <param name="max"> didžiausia surinkta suma </param>
        /// <param name="mcTourists"> daugiausiai surinkę turistai </param>
        void PrintData(int con, int max, List<Tourist> mcTourists)
        {
            Console.WriteLine("Iš viso skirta {0}eu ir {1}ct", SumToEur(con), SumToCt(con));
            Console.WriteLine("Daugiausiai skirta {0}eu ir {1}ct, kuriuos skyrė:", SumToEur(max), SumToCt(max));
            foreach(Tourist t in mcTourists)
            {
                Console.WriteLine(t.Name);
            }
        }
    }

    /// <summary>
    /// Turisto klasė, laikantį jo vardą, eurus, centus ir pasidalintą kiekį
    /// </summary>
    class Tourist
    {
        public string Name { get; set; }
        public int Eur { get; set; }
        public int Ct { get; set; }
        public int Con { get; set; }

        public Tourist(string name, int eur, int ct)
        {
            Name = name;
            Eur = eur;
            Ct = ct;
            Con = (eur * 100 + ct) / 4; // ketvirtadalis pridėtos liešos
        }
    }
}
