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
        static void Main(string[] args)
        {
            List<double> result = new List<double>();
            result = Readfile("f.txt");
        }
    }
}
