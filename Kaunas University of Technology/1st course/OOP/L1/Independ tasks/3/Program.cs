using System;

namespace S1_3
{
    class Program
    {
        static void Main(string[] args)
        {
            bool repeat = true; // Kintamasis, nurodantis ar kartoti programą po atsakymo
            while (repeat)
            {
                double a, b, ats; // Įvedami skaičių ir atsakymo kintamieji
                char op; // Įvedamas operacijos ženklo kintamasis

                Console.WriteLine("Įveskite pirmąjį skaičių:");
                // Bandoma nuskaityti pirmąjį skaičių
                if (!double.TryParse(Console.ReadLine(), out a))
                {
                    Console.WriteLine("KLAIDA\n\n");
                    continue;
                }

                Console.WriteLine("Įveskite antrąjį skaičių:");
                // Bandoma nuskaityti antrąjį skaičių
                if (!double.TryParse(Console.ReadLine(), out b))
                {
                    Console.WriteLine("KLAIDA");
                    continue;
                }

                Console.WriteLine("Įveskite operaciją:");
                // Nuskaitomas simbolis
                op = Convert.ToChar(Console.Read());
                Console.ReadLine(); // Bufferio išvalymas
                switch (op) // Tikrinamas operacijos simbolis
                {
                    case '+':
                        ats = a + b;
                        break;

                    case '-':
                        ats = a - b;
                        break;

                    case '*':
                        ats = a * b;
                        break;

                    case '/':
                        if (b != 0) // "Daugyba iš nulio negalima" patikrinimas
                            ats = a / b;
                        else
                        {
                            Console.WriteLine("KLAIDA");
                            continue;
                        }
                        break;

                    default:
                        Console.WriteLine("KLAIDA");
                        continue;
                }
                Console.WriteLine("{0} {1} {2} = {3}", a, op, b, ats);
            }
        }
    }
}