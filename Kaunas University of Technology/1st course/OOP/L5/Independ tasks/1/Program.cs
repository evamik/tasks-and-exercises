using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace savarankiskas2
{
    class Program
    {
        const string Duomenys = "..\\..\\Duomenys.txt";

        static void Main(string[] args)
        {
            Program p = new Program();            string tekstas = File.ReadAllText(Duomenys, Encoding.GetEncoding(1257));            tekstas = p.Apdoroti(tekstas);
            tekstas = p.Apdoroti2(tekstas);
            Console.WriteLine(tekstas);
        }

        string Apdoroti(string tekstas)
        {
            string[] eilutes = Regex.Split(tekstas, "\r\n");
            string naujasTekstas = "";
            for(int i = 0; i < eilutes.Length; i++)
            {
                for (int j = 0; j < eilutes[i].Length-1; j++)
                {
                    if (eilutes[i][j] == '/' && eilutes[i][j + 1] == '/')
                        eilutes[i] = eilutes[i].Remove(j, eilutes[i].Length - j);
                }
                naujasTekstas += eilutes[i] + "\r\n";
            }
            return naujasTekstas;
        }

        string Apdoroti2(string tekstas)
        {
            int indeksas = 0;
            string naujasTekstas = tekstas;
            for(; ; )
            {
                int pradzia = naujasTekstas.IndexOf("/*", indeksas);
                if (pradzia == -1)
                    break;

                int pabaiga = naujasTekstas.IndexOf("*/", pradzia);
                if (pabaiga == -1)
                    pabaiga = naujasTekstas.Length - 1;
                
                naujasTekstas = naujasTekstas.Remove(pradzia, pabaiga - pradzia);
            }
            return naujasTekstas;
        }
    }
}
