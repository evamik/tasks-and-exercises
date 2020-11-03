using System;

namespace S1_4
{
    class Program
    {
        static void Main(string[] args)
        {
            bool repeat = true; // Kartoti programą?
            while (repeat)
            {
                string name = ""; // Vardo kintamasis
                int length = 0; // Vardo ilgis

                Console.WriteLine("Įveskite savo vardą:");
                while (length == 0) // Kol neįvestas vardas, tol kartoti
                {
                    name = Console.ReadLine(); // Nuskaitomas vardas
                    length = name.Length; // Paimamas vardo ilgis
                }

                switch (name[length - 1]) // Tikrinama paskutinė vardo raidė
                {
                    case 'ė': // "-ė"
                        name = changeEnding(name, 1, "e");
                        break;
                    case 's': 
                        // Jei vardas baigiasi raide 's' tikrinama antra nuo galo raidė
                        switch (name[length - 2])
                        {
                            case 'a': // "-as"
                                name = changeEnding(name, 2, "ai");
                                break;
                            case 'i': // "-is"
                                name = changeEnding(name, 2, "i");
                                break;
                            case 'y': // "-ys"
                                name = changeEnding(name, 2, "y");
                                break;
                        }
                        break;
                }

                Console.WriteLine("Labas, {0}!\n\n", name);
            }
        }

        // Metodas pakeičiantis string'o galūnę
        // s - keičiamas string'as
        // i - keičiamas raidžių skaičius
        // end - norima papildoma galūnė
        static string changeEnding(string s, int i, string end)
        {
            return s.Remove(s.Length - i, i) + end;
        }
    }
}