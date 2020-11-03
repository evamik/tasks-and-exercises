using System;
using System.Collections.Generic;
using System.IO;

namespace savarankiskas1
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "flats.csv";

            Program p = new Program();
            FlatContainer flats = p.ReadData(path);
            int rooms = p.InputRooms();
            int[] floors = p.InputFloors();
            double cost = p.InputCost();
            FlatContainer searchResults = p.SearchFlats(flats, rooms, floors, cost, 27, 3);
            p.PrintResults(searchResults);
        }

        private void PrintResults(FlatContainer flats)
        {
            Console.WriteLine("-----------------------------------------------------------------\n" +
                              "| Numeris | Plotas | Kambariai |    Kaina    | Telefono numeris |\n" +
                              "-----------------------------------------------------------------");
            for(int i = 0; i < flats.GetCount(); i++)
            {
                Flat flat = flats.GetFlat(i);
                Console.WriteLine("| {0, 7} | {1, 6} | {2, 9} | {3, 11} | {4, 16} |", flat.Number, flat.Area + "m²", flat.Rooms,
                    flat.Cost + " eur", flat.Phone);
                Console.WriteLine("-----------------------------------------------------------------");
            }
        }

        /// Ieško butų iš flats sąrašo, kurie turi rooms kambarių, yra floors aukštuose ir kainuoja ne daugiau cost
        /// <param name="flats"> Butų sąrašas </param>
        /// <param name="rooms"> Kambarių skaičius </param>
        /// <param name="floors"> Aukštų intervalas </param>
        /// <param name="cost"> Kaina </param>
        /// <param name="flatsInStairway"> Butų kiekis laiptinėje </param>
        /// <param name="flatsPerFloor"> Butų kiekis aukšte </param>
        /// <returns> Surastų butų sąrašas </returns>
        private FlatContainer SearchFlats(FlatContainer flats, int rooms, int[] floors, double cost, int flatsInStairway, int flatsPerFloor)
        {
            FlatContainer searchResults = new FlatContainer(flats.GetCount());

            for (int i = 0; i < flats.GetCount(); i++)
            {
                Flat flat = flats.GetFlat(i);
                // Eiti į kitą ciklą jei:
                // Butas neturi tiek kambarių
                if (flat.Rooms != rooms)
                    continue;
                // Buto aukštas nepatenka į intervalą
                int flatNumberInStairway = getFlatNumberInStairway(flat.Number, flatsInStairway);
                int floor = getFloor(flatNumberInStairway, flatsPerFloor);
                if (floor < floors[0] && floor > floors[1])
                    continue;

                // Buto kaina per didelė
                if (flat.Cost > cost)
                    continue;
                // Atitikus visus reikalavimus pridėti butą prie rezultatų
                searchResults.Add(flat);
            }

            return searchResults;
        }

        /// Gražina buto numerį laiptinėje [1, flatsInStairway]
        /// <param name="number"> Tikras buto numeris </param>
        /// <param name="flatsInStairway"> Butų kiekis laiptinėje </param>
        /// <returns> Numeris nuo 1 iki flatsInStairway </returns>
        private static int getFlatNumberInStairway(int number, int flatsInStairway)
        {
            // Paskaičiuoja kiek laiptinių ir atima laiptinių skaičių,
            // padaugintą iš kiek butų būna laiptinėje
            return number - flatsInStairway * ((number - 1) / flatsInStairway);
        }

        /// Parašo konsolėje įvedimo žinutę ir priima įvestą butų kainą
        /// <returns> Butų kaina </returns>
        private double InputCost()
        {
            double cost;
            Console.WriteLine("Įveskite butų kainą:");
            while (!double.TryParse(Console.ReadLine(), out cost))
                continue;
            return cost;
        }

        /// Parašo konsolėje įvedimo žinutę ir priima įvestą aukštų intervalą
        /// <returns> Aukštų intervalas </returns>
        private int[] InputFloors()
        {
            Console.Write("Įveskite ieškomų butų aukštų intervalą:\n[");
            int arrayStart;
            int arrayEnd = 0;
            // Kol nepavyksta, tikrina ar paspaustas mygtukas yra skaičius
            while (!int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out arrayStart))
                continue;
            Console.Write("{0}, ", arrayStart);
            // Kol antras įvestas skaičius mažesnis už pirmą
            while (arrayEnd < arrayStart)
                // Kol nepavyksta, tikrina ar paspaustas mygtukas yra skaičius
                while (!int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out arrayEnd))
                    continue;
            Console.Write("{0}]\n", arrayEnd);
            int[] floors = { arrayStart, arrayEnd };
            return floors;
        }

        /// Parašo konsolėje įvedimo žinutę ir priima įvestą kambarių skaičių
        /// <returns> Kambarių skaičius </returns>
        private int InputRooms()
        {
            Console.WriteLine("Įveskite ieškomų butų kambarių skaičių:");
            int rooms;
            while (!int.TryParse(Console.ReadLine(), out rooms))
                Console.WriteLine("Blogai įvestas būtų skaičius");
            return rooms;
        }


        /// Nuskaito butų duomenis iš failo ir grąžina būtų konteinerį
        /// <param name="path"> failo pavadinimas </param>
        /// <returns> Butų konteineris </returns>
        private FlatContainer ReadData(string path)
        {
            string[] lines = File.ReadAllLines(@path);

            FlatContainer flatContainer = new FlatContainer(lines.Length);
            foreach (string line in lines)
            {
                string[] values = line.Split(';');
                int number = int.Parse(values[0]);
                double area = double.Parse(values[1]);
                int rooms = int.Parse(values[2]);
                int cost = int.Parse(values[3]);
                string phone = values[4];

                Flat f = new Flat(number, area, rooms, cost, phone);
                flatContainer.Add(f);
            }

            return flatContainer;
        }

        /// Grąžina aukšto skaičių
        /// <param name="number"> Buto numeris pačioje laiptinėje </param>
        /// <param name="flatsPerFloor"> Butų kiekis aukšte </param>
        /// <returns> Aukštas </returns>
        private static int getFloor(int number, int flatsPerFloor)
        {
            return (int)Math.Ceiling((double)(number) / 3);
        }
    }

    /// Buto klasė
    class Flat
    {
        public int Number { get; private set; }
        public double Area { get; private set; }
        public int Rooms { get; private set; }
        public int Cost { get; private set; }
        public string Phone { get; private set; }

        public Flat(int number, double area, int rooms, int cost, string phone)
        {
            Number = number;
            Area = area;
            Rooms = rooms;
            Cost = cost;
            Phone = phone;
        }
    }


    /// Butų konteinerio klasė, skirta laikyti butų sąrašą
    class FlatContainer
    {
        private Flat[] Flats;
        private int Count;

        public FlatContainer(int size)
        {
            Count = 0;
            Flats = new Flat[size];
        }

        public void Add(Flat flat)
        {
            Flats[Count] = flat;
            Count++;
        }

        public Flat GetFlat(int i)
        {
            return Flats[i];
        }

        public int GetCount()
        {
            return Count;
        }
    }
}
