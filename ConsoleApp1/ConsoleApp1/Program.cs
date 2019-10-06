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

        static int CharToInt(char letter)
        {
            if (letter >= '0' && letter <= '9')
            {
                return letter - '0';
            }
            else
            {
                return letter - 'A' + 10;
            }
        }
        static char IntToChar(int number)
        {
            if (number > 9)
            {
                return (char)(65 + (number - 10));
            }
            else
            {
                return (char)(number + '0');
            }
        }
        static string Translate_RealPart_From10(int r, double realPart)
        {
            int i = 0;
            string returnReal = "";
            while (i != 64)
            {
                realPart *= r;
                if((int)realPart % r == 0)
                {
                    returnReal += "0";
                }
                else
                {
                    realPart -= 1;
                    //realPart = double.Parse("0" + realPart.ToString().Substring(1), System.Globalization.CultureInfo.InvariantCulture);
                    returnReal += "1";
                }

                i++;
            }

            return returnReal;
        }
        static string Translate_IntPart_From10(int r, int Integer)
        {
            Stack<char> stack = new Stack<char>();
            int intPart, residue;
            do
            {
                intPart = Integer / r;
                residue = Integer % r;
                stack.Push(IntToChar(residue));
                Integer = intPart;
            } while (intPart != 0);

            string returnInt = "";

            while (stack.Count != 0)
            {
                returnInt += stack.Pop();
            }

            return returnInt;
        }
        static string TranslateFrom10(int systemBasis, double number)
        {
            int intPart = (int)number;
            int comIndex = number.ToString().IndexOf('.');
            double realPart;
            if (comIndex != -1)
            {
                realPart = double.Parse("0" + number.ToString().Substring(number.ToString().IndexOf('.')), System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {
                realPart = 0;
            }

            string returnValue = Translate_IntPart_From10(systemBasis, intPart);

            if (realPart != 0)
            {
                returnValue += "," + Translate_RealPart_From10(systemBasis, realPart);
            }

            return returnValue;
        }

        static double TranslateTo10(int systemBasis, string fullNumber)
        {
            int commaPosition = fullNumber.IndexOf(',');
            int lowerDegree = 0;
            if (commaPosition != -1)//double
            {
                lowerDegree = commaPosition - fullNumber.Length + 1;
                fullNumber = fullNumber.Remove(commaPosition, 1);
            }

            double resultNumber = 0;
            for (int i = fullNumber.Length - 1, j = lowerDegree; i >= 0; i--, j++)
            {
                resultNumber += CharToInt(fullNumber[i]) * Math.Pow(systemBasis, j);
            }
            return resultNumber;
        }
        static double MemoryRepresToDoule(string repres, int digitCapacity)
        {
            if (digitCapacity != 32 && digitCapacity != 64)
            {
                throw new Exception("NonExistentDigitCapacity");
            }

            string doubleRepres;
            if (digitCapacity == 32)
            {
                doubleRepres = Convert.ToString(Convert.ToInt64(repres, 16), 2).PadLeft(32, '0');
            }
            else
            {
                doubleRepres = Convert.ToString(Convert.ToInt64(repres, 16), 2).PadLeft(64, '0');

            }

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
            if (digitCapacity == 32)
            {
                exponent = Convert.ToInt32(doubleRepres.Substring(1, 8), 2);
                exponent -= 127;
                fraction = TranslateTo10(2, "1" + (doubleRepres.Substring(9)).Insert(exponent, ","));
            }
            else
            {
                exponent = Convert.ToInt32(doubleRepres.Substring(1, 11), 2);
                exponent -= 1023;
                fraction = TranslateTo10(2, "1" + (doubleRepres.Substring(12)).Insert(exponent, ","));
            }

            return sign * fraction;
        }

        static string SumBinary(string b1, string b2)
        {
            int i = 0, rem = 0;
            string sum = "";
            b2 = b2.PadLeft(52, '0');

            while (b1.Length != 0 || b2.Length != 0)
            {
                sum += ((int.Parse(b1[b1.Length - 1].ToString()) + int.Parse(b2[b2.Length - 1].ToString()) + rem) % 2).ToString();
                rem = (int.Parse(b1[b1.Length - 1].ToString()) + int.Parse(b2[b2.Length - 1].ToString()) + rem) / 2;
                b1 = b1.Substring(0, b1.Length - 1);
                b2 = b2.Substring(0, b2.Length - 1);
            }
            if (rem != 0)
                sum += rem;

            return sum;
        }

        static string DoubleToMemoryRepres(double number, int digitCapacity)
        {
            if (digitCapacity != 32 && digitCapacity != 64)
            {
                throw new Exception("NonExistentDigitCapacity");
            }

            int sign;
            long exponent;
            if (number < 0)
            {
                sign = 1;
            }
            else
            {
                sign = 0;
            }
            number = Math.Abs(number);
            string numberAsString = number.ToString();

            string binaryNumber = TranslateFrom10(2, number);
            int decimalexponent = binaryNumber.IndexOf(',') - 1;
            string fullNumber;
            if (binaryNumber.IndexOf(',') != -1)
            {
                fullNumber = binaryNumber.Remove(binaryNumber.IndexOf(','), 1).Substring(1);
            }
            else
            {
                fullNumber = binaryNumber.Substring(1);
            }


            string doubleRepres;
            if (digitCapacity == 32)
            {
                decimalexponent += 127;
                exponent = Convert.ToInt32(Convert.ToString(decimalexponent, 2));

                if (fullNumber.Length >= 23 && fullNumber[23] == '1')
                {
                    fullNumber = SumBinary(fullNumber.Substring(0, 23), "1");
                }
                doubleRepres = sign.ToString() + String.Format("{0:d8}", exponent) + (fullNumber.Length <= 23 ? fullNumber.PadRight(23, '0') : fullNumber.Substring(0, 23));
            }
            else
            {
                decimalexponent += 1023;
                exponent = Convert.ToInt64(Convert.ToString(decimalexponent, 2));

                if (fullNumber.Length >= 52 && fullNumber[52] == '1')
                {
                    fullNumber = SumBinary(fullNumber.Substring(0, 52), "1");
                }
                doubleRepres = sign.ToString() + String.Format("{0:d11}", exponent) + (fullNumber.Length <= 52 ? fullNumber.PadRight(52, '0') : fullNumber.Substring(0, 52));
            }

            return Convert.ToInt64(doubleRepres, 2).ToString("X");
        }
        static void Main(string[] args)
        {
            List<DigitDouble> digitNumbers = ReadfileDigit("Digit.txt");

            foreach (DigitDouble digitNumber in digitNumbers)
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