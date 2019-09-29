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
        static string FromDec(long n, int p)//перевод в двоичную
        {
            var result = "";
            for (; n > 0; n /= p)
            {
                var x = n % p;
                result = (char)(x < 0 || x > 9 ? x + 'A' - 10 : x + '0') + result;
            }
            return result;
        }

        static string[] FromEVM_64 (int mark, int exponent, string result, int n)
        {
            string[] resultEvm = new string[32];
            resultEvm[0] = resultEvm[12] = System.Convert.ToString(mark);
            
            return resultEvm;
        }

        static string[] FromEVM_32(int mark, int exponent, string result, int n)
        {
            string[] resultEvm = new string[32];
            resultEvm[0] = resultEvm[9] = System.Convert.ToString(mark);

            return resultEvm;
        }
        static void Main(string[] args)
        {
            List<double> result = Readfile("f.txt");
            //List<int> mark = new List<int>();
            //List<int> exponent = new List<int>();
            int[] mark = new int[result.Count];//массив знаков
            int[] exponent = new int[result.Count];//массив порядка
            string[] result2 = new string[result.Count];//массив результата представленного в двоичном виде

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

                string str = System.Convert.ToString(result[i]);//разделяю на целую и дробную часть, чтобы узнать колличество цифр
                string[] parts = str.Split(',');// пробовала через Math.Truncate, но там проблемы с дробной частью были
                exponent[i] = parts[0].Length;//1 порядок

                exponent[i] = Convert.ToInt32(FromDec(exponent[i], 2));//2 порядок в 2-ичной
                result[i] = result[i] * Math.Pow(10, parts[1].Length);//3,4 - полное число без дробной части
                result2[i] = FromDec((int)(result[i]), 2);//5 - это число в 2-ичной

                FromEVM_32(mark[i], exponent[i], result2[i], result.Count);
                FromEVM_64(mark[i], exponent[i], result2[i], result.Count);
            }
        }
    }
}
