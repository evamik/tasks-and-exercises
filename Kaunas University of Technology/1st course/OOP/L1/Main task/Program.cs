using System;
using System.Collections.Generic;
using System.IO;


/*Nekilnojamo turto agentūra. Turite duomenis apie šiuo metu Kaune parduodamus namus.
Duomenų faile pateikta ši informacija: 
mikrorajonas, gatvė, namo numeris, tipas, pastatymo metai, plotas, kambarių skaičius.
 Raskite ar „Saulėtekio“ mikrorajone parduodamas nors vienas namas?
 Raskite, kokio tipo namų daugiausia, ekrane atspausdinkite namo tipą, ir to tipo namų
skaičių.
 Sudarykite visų mikrorajonų, kuriuose šiuo metu pardavinėjami namai, sąrašą, mikrorajonų
pavadinimus surašykite faile „Mikrorajonai.csv“.
 Sudarykite visų medinių namų sąrašą, į rezultatų failą „Mediniai.csv“ surašykite namo
adresą, plotą ir kambarių skaičių.*/

namespace LD1 {

    class Program {

        static void Main(string[] args) {
            Program p = new Program();
            //Pradiniai duomenys Nr.1:
            string path = "LD1data.csv"; // duomenų failas
            string neighbourhood = "Saulėtekio"; // Pirmo punkto mikrorajonas
            string savePath = "Mikrorajonai.csv";
            string type = "Medinis"; // Ketvirto punkto namų tipas
            string typeSavePath = "Mediniai.csv"; // Ketvirto punkto failo pavadinimas

            List<House> houses = p.ReadFile(path); // Nuskaitomas namų failas

            // Patikrinama ar mikrorajone yra parduodamas bent vienas namas
            bool contains = p.ContainsNeighbourhood(houses, neighbourhood);
            p.PrintContainsNeighbourhood(contains, neighbourhood); // Spausdinamas pirmas punktas

            List<string> mostOccuringType = p.MostOccuringType(houses); // Antro punkto vykdymas
            p.PrintMostOccuringType(mostOccuringType); // Ir spausdinimas

            List<string> neighbourhoods = p.Neighbourhoods(houses); // Trečio punkto vykdymas
            p.SaveNeighbourhoodsToFile(neighbourhoods, savePath); // Ir išsaugojimas į failą

            List<House> housesOfType = p.HousesOfType(houses, type); // Ketvirto punkto vykdymas
            p.SaveHousesToFile(housesOfType, typeSavePath); // Ir išsaugojimas į failą
        }

        private void SaveHousesToFile(List<House> houses, string path) {
            const string header = "----------------------------------------------\r\n" +
                                  "| Adresas           | Plotas(m2) | Kamb. sk. |\r\n" +
                                  "|-------------------|------------|-----------|";
            using(var fr = File.AppendText(@path)) {
                fr.WriteLine(header);
                foreach (House h in houses) {
                    string address = h.Street + " " + h.Address;
                    fr.WriteLine("| {0, -17} | {1, 10} | {2, 9} |", address, h.Space, h.RoomCount);
                }
                fr.WriteLine("----------------------------------------------");
            }
        }

        /// Suranda visus namus iš sąrašo su pasirinktu tipu
        /// <param name="houses"> Namų sąrašas </param>
        /// <param name="type"> Tipas </param>
        /// <returns> Surastų namų sąrašas </returns>
        private List<House> HousesOfType(List<House> houses, string type) {
            List<House> housesOfType = new List<House>();

            foreach(House h in houses)
                if (h.Type == type)
                    housesOfType.Add(h);

            return housesOfType;
        }

        /// Išspausdina visus mikrorajonus į failą
        /// <param name="neighbourhoods"> Mikrorajonų sąrašas </param>
        /// <param name="path"> Failo nuoroda </param>
        private void SaveNeighbourhoodsToFile(List<string> neighbourhoods, string path) {
            using(var fr = File.AppendText(@path)) {
                foreach(string n in neighbourhoods) {
                    fr.WriteLine(n);
                }
            }
        }

        /// Randa visus mikrorajonus esančius namų sąraše
        /// <param name="houses"> Namų sąrašas </param>
        /// <returns> Mikrorajonų sąrašas </returns>
        private List<string> Neighbourhoods(List<House> houses) {
            List<string> neighbourhoods = new List<string>();

            foreach (House h in houses) { // Tikrinamas kiekvienas namas
                string n = h.Neighbourhood;
                if (neighbourhoods.Contains(n)) // Jei mikrorajonas jau yra sąraše
                    continue; // nieko nedaryti ir pradėti kitą ciklo ratą

                // Kodas kuris tęsis tik jei mikrorajono nebuvo sąraše
                neighbourhoods.Add(n); // Pridėti mikrorajoną prie sąrašo
            }

            return neighbourhoods;
        }

        /// Nuskaito namų duomenis iš failo ir sukelia juos į sąrašą
        /// <param name="path"> Failo pavadinimas </param>
        /// <returns> Namų sąrašas </returns>
        private List<House> ReadFile(string path) {
            List<House> houses = new List<House>();

            string[] lines = File.ReadAllLines(@path);
            foreach (string line in lines) {
                string[] values = line.Split(';');
                string nh = values[0];
                string st = values[1];
                string ad = values[2];
                string tp = values[3];
                int yr = int.Parse(values[4]);
                double sp = double.Parse(values[5]);
                int rc = int.Parse(values[6]);

                House h = new House(nh, st, ad, tp, yr, sp, rc);
                houses.Add(h);
            }

            return houses;
        }

        /// Spausdina tipo sąraše nurodytą tipą ir sąrašo ilgį
        /// <param name="types"> Tipo sąrašas </param>
        private void PrintMostOccuringType(List<string> types) {
            Console.WriteLine("Dažniausias parduodamų namų tipas yra {0}. Iš viso {1}",
                types[0], types.Count);
        }

        /// Suranda dažniausiai atsikartojantį namo tipą
        /// <param name="houses"> Namų sarašas </param>
        /// <returns> Namų sąrašas su daugiausiai atsikartojančiu tipu </returns>
        private List<string> MostOccuringType(List<House> houses) {
            List<string> mostType = new List<string>(); // Dažniausiai atsikartojantis tipas

            List<string> types = new List<string>(); // Sukuriamas tipų sąrašas
            List<int> counts = new List<int>(); // Sukuriamas pasikartojimams skaičiuoti sąrašas

            foreach (House h in houses) { // Tikrinamas kiekvienas namas
                string t = h.Type;
                if (types.Contains(t)) // Jei namo tipas jau yra sąraše
                    for (int i = 0; i < types.Count; i++) {
                        if (types[i] == t) {
                            counts[i]++; // Padidinti to tipo pasikartojimų skaičių sąraše
                            continue;
                        }
                    }
                // Kodas kuris tęsis tik jei namo tipo nebuvo sąraše
                types.Add(t); // Pridėti tipą prie sąrašo
                counts.Add(1); // Užfiksuoti, kad jis pasikartojo vieną kartą
            }

            int mostCount = 0;
            int mostID = 0;

            for (int i = 0; i < types.Count; i++) // Ciklas skirtas surasti daugiausiai pasikartojusį
                if (counts[i] > mostCount) {
                    mostCount = counts[i]; // Kiek kartų pasikartojo
                    mostID = i; // Koks pasikartojusio tipo indeksas sąraše
                }

            string type = types[mostID]; // Dažniausiai pasikartojančio tipo kintamasis
            for (int i = 0; i < mostCount; i++) {
                mostType.Add(type); // Užpildomas sąrašas to pačio kintamojo,
                //vėliau naudojimui paimti string ir .Count
            }

            return mostType;
        }

        /// Patikrina ar bent vienas namas priklauso pasirinktam mikrorajonui
        /// <param name="houses"> Tikrinamų namų sąrašas </param>
        /// <param name="neighbourhood"> Tikrinamas mikrorajonas </param>
        /// <returns> true/false </returns>
        private bool ContainsNeighbourhood(List<House> houses, string neighbourhood) {

            foreach (House h in houses) {
                if (h.Neighbourhood == neighbourhood)
                    return true;
            }

            return false;
        }

        /// Spausdina užduoties pirmo punkto sąlygą
        /// <param name="contains"> Ar rajone yra parduodamų namų kintamasis </param>
        /// <param name="neighbourhood"> Tikrinamas mikrorajonas </param>
        private void PrintContainsNeighbourhood(bool contains, string neighbourhood) {
            if (contains == false) {
                Console.WriteLine("{0} mikrorajone nėra parduodamų namų", neighbourhood);
            }
            else Console.WriteLine("{0} mikrorajone yra parduodamų namų", neighbourhood);
        }
    }

    /// Namo klase
    class House {

        public string Neighbourhood { get; set; }
        public string Street { get; set; }
        public string Address { get; set; }
        public string Type { get; set; }
        public int Year { get; set; }
        public double Space { get; set; }
        public int RoomCount { get; set; }

        public House(string neighbourhood, string street, string address,
            string type, int year, double space, int roomcount) {
            Neighbourhood = neighbourhood;
            Street = street;
            Address = address;
            Type = type;
            Year = year;
            Space = space;
            RoomCount = roomcount;
        }
    }
}
