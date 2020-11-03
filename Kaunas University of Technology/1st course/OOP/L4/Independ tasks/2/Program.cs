using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace savarankiskas4
{
    class Program
    {
        const string CFd = "..\\..\\Duomenys.txt";
        const string CFr = "..\\..\\Rezultatai.txt";
        const bool IrIsDidziosios = true;

        static void Main(string[] args)
        {
            char[] skyrikliai = {' ', '.', ',', '!', '?', ':', ';', '(', ')', '\t' };
            Program p = new Program();

            string tekstas = File.ReadAllText(CFd);

            tekstas = p.Pasalinti(tekstas, skyrikliai);

            File.Delete(CFr);
            File.AppendAllText(CFr, tekstas);
        }

        string Pasalinti(string tekstas, char[] skyrikliai)
        {
            string[] zodziai = IvestiZodzius();
            Spausdinti(zodziai);
            List<string> zodziai2 = new List<string>(Regex.Split(tekstas, " "));
            string skyr = new string(skyrikliai);

            foreach (string zodis in zodziai)
            {
                int indeksas = 0;

                while (tekstas.Substring(indeksas).Contains(zodis)){
                    indeksas = tekstas.IndexOf(zodis, indeksas);
                    int pabaiga = indeksas + zodis.Length;

                    if(indeksas > 0 && !skyr.Contains(tekstas[indeksas-1].ToString()))
                    {
                        indeksas = pabaiga;
                        continue;
                    }

                    while (skyr.Contains(tekstas[pabaiga].ToString()))
                    {
                        if (tekstas.Length == pabaiga + 1)
                            break;
                        pabaiga++;
                    }

                    tekstas = tekstas.Remove(indeksas, pabaiga - indeksas);
                }
            }

            return tekstas;
        }

        void Spausdinti(string[] zodziai)
        {
            Console.Write("Žodžiai \"");
            foreach (string zodis in zodziai)
            {
                Console.Write(zodis + " ");
            }
            Console.Write("\" bus ištrinti iš teksto.\n");
        }

        string[] IvestiZodzius()
        {
            Console.WriteLine("Įveskite žodžius, kuriuos norite, kad ištrintų iš teksto:");
            List<string> zodziai = new List<string>();
            StringBuilder zodis = new StringBuilder();

            for(; ; )
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.Spacebar)
                {
                    zodziai.Add(zodis.ToString());
                    if(IrIsDidziosios) zodziai.Add(zodis[0].ToString().ToUpper() + zodis.ToString().Substring(1));
                    zodis.Clear();
                    continue;
                }

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    zodziai.Add(zodis.ToString());
                    if (IrIsDidziosios) zodziai.Add(zodis[0].ToString().ToUpper() + zodis.ToString().Substring(1));
                    break;
                }
                zodis.Append(keyInfo.KeyChar);
            }

            return zodziai.ToArray();
        }
    }
}
