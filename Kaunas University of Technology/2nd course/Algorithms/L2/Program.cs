using System;
using System.Diagnostics;

namespace Program
{
    class Program
    {
        public static void Main()
        {
            first();

            second();
        }

        static void first()
        {
            int[] n_array = { 5, 10, 15, 20, 25 };
            int[] W_array = { 20, 40, 60, 80, 100 };

            int[] w = randomIntArray(n_array[4], 1, 2);
            int[] v = randomIntArray(W_array[4], 1, 2);
            F(W_array[4], w, v, n_array[4]);
            F(W_array[4], w, v, n_array[4]);
            F(W_array[1], w, v, n_array[1]);
            dp_F(W_array[4], w, v, n_array[4]);
            dp_F(W_array[4], w, v, n_array[4]);
            dp_F(W_array[1], w, v, n_array[1]);
            Console.WriteLine("Rekurentinio atsakymas: " + F(W_array[4], w, v, n_array[4]));
            Console.WriteLine("Dinaminio atsakymas: " + dp_F(W_array[4], w, v, n_array[4]));

            Console.Write("{0, -15}", "W\\n");
            for (int i = 0; i < 5; i++)
            {
                Console.Write("{0, -20}", n_array[i]);
            }
            Console.WriteLine();
            Console.Write("".PadLeft(10));
            for (int i = 0; i < 5; i++)
            {
                Console.Write("{0, -10}", "r");
                Console.Write("{0, -10}", "dp");
            }
            Console.WriteLine();
            for (int i = 0; i < 5; i++)
            {
                Console.Write("{0, -5}", W_array[i]);
                for (int j = 0; j < 5; j++)
                {
                    test(n_array[j], W_array[i], 1, 2, 1, 2);
                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        static void test(int n, int W, int min_v, int max_v, int min_w, int max_w)
        {
            int[] w = randomIntArray(n, min_w, max_w);
            int[] v = randomIntArray(n, min_v, max_v);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            F(W, w, v, n);
            sw.Stop();
            Console.Write("{0, 10}", sw.Elapsed.TotalMilliseconds);

            sw.Reset();
            sw.Start();
            dp_F(W, w, v, n);
            sw.Stop();
            Console.Write("{0, 10}", sw.Elapsed.TotalMilliseconds);
        }

        static int max(int a, int b)
        {
            return (a > b) ? a : b;
        }

        static int[] randomIntArray(int n, int min, int max)
        {
            int[] array = new int[n];

            Random rand = new Random(1);
            for (int i = 0; i < n; i++)
            {
                array[i] = rand.Next(min, max);
            }

            return array;
        }

        static int F(int W, int[] w, int[] v, int n)
        {
            if (n == 0 || W == 0)
                return 0;

            if (w[n - 1] > W)
                return F(W, w, v, n - 1);

            else return max(
                    v[n - 1] + F(W - w[n - 1], w, v, n - 1),
                    F(W, w, v, n - 1)
                    );
        }

        static int dp_F(int W, int[] w, int[] v, int n)
        {
            int[,] K = new int[n + 1, W + 1];

            for (int i = 0; i <= n; i++)
            {
                for (int j = 0; j <= W; j++)
                {
                    if (i == 0 || j == 0)
                        K[i, j] = 0;
                    else if (w[i - 1] <= j)
                        K[i, j] = Math.Max(v[i - 1] + K[i - 1, j - w[i - 1]], K[i - 1, j]);
                    else K[i, j] = K[i - 1, j];
                }
            }

            return K[n, W];
        }

        static void second()
        {
            int[] n_array = { 10, 15, 20, 25, 30 };

            char[] chars = randomCharArray(n_array[4]);
            minimum(chars, 0, chars.Length - 1);
            dp_minimum(chars, chars.Length);
            Console.WriteLine("Rekurentinio atsakymas: " + minimum(chars, 0, chars.Length - 1));
            Console.WriteLine("Dinaminio atsakymas: " + dp_minimum(chars, chars.Length));

            Console.Write("{0, -15}", "n");
            for (int i = 0; i < 5; i++)
            {
                Console.Write("{0, -20}", n_array[i]);
            }
            Console.WriteLine();
            Console.Write("".PadLeft(10));
            for (int i = 0; i < 5; i++)
            {
                Console.Write("{0, -10}", "r");
                Console.Write("{0, -10}", "dp");
            }
            Console.WriteLine();
            Console.Write("{0, -5}", "");
            for (int i = 0; i < 5; i++)
            {
                test2(n_array[i]);
            }
            Console.WriteLine();
        }

        static void test2(int n)
        {
            char[] chars = randomCharArray(n);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            minimum(chars, 0, chars.Length - 1);
            sw.Stop();
            Console.Write("{0, 10}", sw.Elapsed.TotalMilliseconds);

            sw.Reset();
            sw.Start();
            dp_minimum(chars, chars.Length);
            sw.Stop();
            Console.Write("{0, 10}", sw.Elapsed.TotalMilliseconds);
        }

        static char[] randomCharArray(int n)
        {
            String chars = "abcdefghijklmnopqrstuvwxyz";
            char[] array = new char[n];

            Random rand = new Random(1);
            for (int i = 0; i < n; i++)
            {
                array[i] = chars[rand.Next(0, chars.Length)];
            }

            return array;
        }

        static int minimum(char[] chars, int i, int j)
        {
            if (i > j) return int.MaxValue;
            if (i == j) return 0;
            if (i == j - 1)
                return (chars[i] == chars[j]) ? 0 : 1;

            if (chars[i] == chars[j])
                return minimum(chars, i + 1, j - 1);
            else {
                int a = minimum(chars, i, j - 1);
                int b = minimum(chars, i + 1, j);
                return Math.Min(a, b) + 1; 
            }
        }

        static int dp_minimum(char[] chars, int n)
        {
            int[,] table = new int[n, n];
            int i, j, row;

            for (row = 1; row < n; row++)
                for (i = 0, j = row; j < n; i++, j++)
                {
                    if (chars[i] == chars[j])
                        table[i, j] = table[i + 1, j - 1];
                    else table[i, j] = Math.Min(table[i, j - 1], table[i + 1, j]) + 1;
                }

            return table[0, n - 1];
        }
    }
}
