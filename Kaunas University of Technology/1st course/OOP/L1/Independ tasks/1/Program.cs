using System;

namespace S1_1
{
    class Program
    {
        static void Main(string[] args)
        {
            char character; // Simbolis
            double s_kiekis; // Simbolių kiekis
            int e_kiekis; // Eilučių kiekis

            Console.WriteLine("Įveskite spausdinamą simbolį:");
            character = (char)Console.Read(); // Nuskaitomas simbolis

            Console.ReadLine(); // Išvalomas bufferis

            Console.WriteLine("Įveskite simbolių kiekį:");
            s_kiekis = (double)int.Parse(Console.ReadLine()); // Nuskaitomas simbolių kiekis

            Console.WriteLine("Įveskite simbolių kiekį eilutėje:");
            e_kiekis = int.Parse(Console.ReadLine()); // Nuskaitomas eilučių kiekis

            int eilutes = (int) Math.Ceiling(s_kiekis / e_kiekis); // Suskaiciuojamas būsimų eilučių skaičius

            for (int i = 1; i <= eilutes; i++)
            {
                int max = e_kiekis; // ciklo didziausia reiksme
                if(i*e_kiekis > s_kiekis) max = (int)s_kiekis%e_kiekis; // jei kita pilna eilutė turės perdaug simbolių,
                //pakeisti ciklo didžiausią reikšmę į liekaną

                for (int j = 0; j < max; j++) // Ciklas keliaujantis pro simbolius eilutėje
                {
                    Console.Write(character); // Spausdinamas simbolis
                }
                Console.WriteLine(""); // Pereinama į kitą eilutę
            }
        }
    }
}
