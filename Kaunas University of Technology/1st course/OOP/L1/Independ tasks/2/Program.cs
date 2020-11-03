using System;

namespace S1_2
{
    class Program
    {
        static void Main(string[] args)
        {
            double x; // Įvedamas x kintamasis

            Console.WriteLine("Įveskite x:");
            x = double.Parse(Console.ReadLine());

            double f; // Įvedamas funkcijos rezultato kintamasis

            if(-4 <= x && x < -2) // pritaikoma 1 sąlyga x'ui
            {
                f = 1 / x;
            }
            else if (-2 <= x && x <= 2) // pritaikomas 2 sąlyga x'ui
            {
                f = Math.Cos(Math.PI * x / 180.0);
            }
            else // kitais atvėjais
            {
                f = 2 * x + 4;
            }

            Console.WriteLine("f(x) = {0}", f);
        }
    }
}