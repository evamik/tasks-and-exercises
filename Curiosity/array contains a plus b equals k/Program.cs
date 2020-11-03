using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleQuestion
{
    public class Program
    {
        public static void Main()
        {
            Random random = new Random();
            List<int> numbers = new List<int>(new int[] { 5, 10, 9, 2, 3, 7 });
            int k = 17;
            Console.WriteLine(Found(numbers, k));
        }

        static public bool Found(List<int> numbers, int k)
        {
            numbers.Sort();

            for (int i = 0, j = numbers.Count - 1; i < j;)
            {
                int sum = numbers[j] + numbers[i];
                if (sum == k) return true;
                else if (sum < k) i++;
                else j--;
            }
            return false;
        }
    }
}
