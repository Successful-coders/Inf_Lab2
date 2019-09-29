using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {

        static List<double> Readfile(string filename)
        {
            List<double> numbers = new List<double>();
            foreach (var line in File.ReadLines(filename))
            {
                numbers.Add(double.Parse(line, System.Globalization.CultureInfo.InvariantCulture));
            }
            return numbers;
        }

        static int getCountsOfDigits(int number)//count of number
        {
            int count = (number == 0) ? 1 : 0;
            while (number != 0)
            {
                count++;
                number /= 10;
            }
            return count;
        }
        static string FromDec(long n, int p)
        {
            var result = "";
            for (; n > 0; n /= p)
            {
                var x = n % p;
                result = (char)(x < 0 || x > 9 ? x + 'A' - 10 : x + '0') + result;
            }
            return result;
        }
        static void Main(string[] args)
        {
            List<double> result = Readfile("f.txt");
            //List<int> mark = new List<int>();
            //List<int> exponent = new List<int>();
            int[] mark = new int[result.Count];
            int[] exponent = new int[result.Count];
            string[] result2 = new string[result.Count];

            for (int i = 0; i < result.Count; i++)
            {
                if (result[i] < 0)//0
                    {
                        mark[i] = 1;
                    }
                else
                    {
                        mark[i] = 0;
                    }
                result[i] = Math.Abs(result[i]);
                string str = System.Convert.ToString(result[i]);
                string[] parts = str.Split(',');
                exponent[i] = parts[0].Length;//1
                exponent[i] = Convert.ToInt32(FromDec(exponent[i], 2));//2
                result[i] = result[i] * Math.Pow(10, parts[1].Length);//3,4
                result2[i] = FromDec((int)(result[i]), 2);//5

            }
        }
    }
}
