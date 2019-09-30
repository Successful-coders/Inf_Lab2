using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        private struct DigitDouble
        {
            public double number;
            public int digitCapacity;
        }
        private struct DigitHex
        {
            public string number;
            public int digitCapacity;
        }

        static List<DigitDouble> ReadfileDigit(string filename)
        {
            List<DigitDouble> numbers = new List<DigitDouble>();
            foreach (var line in File.ReadLines(filename))
            {
                string[] data = line.Split(' ');
                DigitDouble digitNumber;
                digitNumber.number = double.Parse(data[0], System.Globalization.CultureInfo.InvariantCulture);
                digitNumber.digitCapacity = Convert.ToInt32(data[1]);

                numbers.Add(digitNumber);
            }
            return numbers;
        }
        static List<DigitHex> ReadfileHex(string filename)
        {
            List<DigitHex> numbers = new List<DigitHex>();
            foreach (var line in File.ReadLines(filename))
            {
                string[] data = line.Split(' ');
                DigitHex digitNumber;
                digitNumber.number = data[0];
                digitNumber.digitCapacity = Convert.ToInt32(data[1]);

                numbers.Add(digitNumber);
            }
            return numbers;
        }

        static double MemoryRepresToDoule(string repres, int digitCapacity)
        {
            if (digitCapacity != 32 && digitCapacity != 64)
            {
                throw new Exception("NonExistentDigitCapacity");
            }

            string doubleRepres = Convert.ToString(Convert.ToInt64(repres, 16), 2);

            int sign;
            if (doubleRepres[0] == '0')
            {
                sign = 1;
            }
            else
            {
                sign = -1;
            }

            int exponent;
            double fraction;
            if(digitCapacity == 32)
            {
                exponent = Convert.ToInt32(doubleRepres.Substring(1, 8), 2);
                fraction = Convert.ToInt64(doubleRepres.Substring(9), 2);
                fraction /= Math.Pow(10, fraction.ToString().Length);
            }
            else
            {
                exponent = Convert.ToInt32(doubleRepres.Substring(1, 11), 2);
                fraction = Convert.ToInt32(doubleRepres.Substring(12), 2);
                fraction /= Math.Pow(10, fraction.ToString().Length);
            }

            return sign * (fraction * Math.Pow(10, exponent));
        }
        static string DoubleToMemoryRepres(double number, int digitCapacity)
        {
            if(digitCapacity != 32 && digitCapacity != 64)
            {
                throw new Exception("NonExistentDigitCapacity");
            }

            int sign;
            int exponent;
            if (number < 0)
            {
                sign = 1;
            }
            else
            {
                sign = 0;
            }
            number = Math.Abs(number);

            string numberAsString = number.ToString();//разделяю на целую и дробную часть, чтобы узнать колличество цифр
            string[] parts = new string[2];
            if (numberAsString.Contains(','))
            {
                parts = numberAsString.Split(',');
            }
            else
            {
                parts[0] = numberAsString;
                parts[1] = "";
            }

            exponent = Convert.ToInt32(Convert.ToString(parts[0].Length, 2));//2 порядок в 2-ичной
            number *= Math.Pow(10, parts[1].Length);//3,4 - полное число без дробной части
            string fraction = Convert.ToString((int)number, 2);//5 - это число в 2-ичной

            string doubleRepres;
            if (digitCapacity == 32)
            {
                doubleRepres = sign.ToString() + String.Format("{0:d8}", exponent) + String.Format("{0:d23}", fraction);
            }
            else
            {
                doubleRepres = sign.ToString() + String.Format("{0:d11}", exponent) + String.Format("{0:d42}", fraction);
            }

            return Convert.ToInt64(doubleRepres, 2).ToString("X");
        }
        static void Main(string[] args)
        {
            List<DigitDouble> digitNumbers = ReadfileDigit("Digit.txt");

            foreach(DigitDouble digitNumber in digitNumbers)
            {
                string numberHex = DoubleToMemoryRepres(digitNumber.number, digitNumber.digitCapacity);
                Console.WriteLine(numberHex);
            }

            List<DigitHex> digitHexes = ReadfileHex("Hex.txt");

            foreach (DigitHex digitHex in digitHexes)
            {
                double numberDouble = MemoryRepresToDoule(digitHex.number, digitHex.digitCapacity);
                Console.WriteLine(numberDouble);
            }
        }
    }
}
