using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;

namespace mvpa_dotnet
{
    class Program
    {
        static void Main(string[] args)
        {
            var calculator = new MvpaCalculator();

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var totalMvpa = calculator.Calculate();
            sw.Stop();

            Console.WriteLine($"Result is {totalMvpa}, and ellapsed time is {sw.ElapsedMilliseconds} ms.");
        }
    }

    internal class MvpaCalculator
    {
        private const int _Mvpa_Heart_Rate_Threshold = 118;
        private const int _Mvpa_Minimum_Period = 10;

        internal int Calculate()
        {
            return
                File.ReadAllLines("fakereal.txt")
                    .Select(d => int.Parse(d))
                    .Select(heart_rate => heart_rate > _Mvpa_Heart_Rate_Threshold ? 1 : 0)
                    .Aggregate(
                        new List<int> { 0 },
                        (acc, current) =>
                            current == 0 ? new List<int>(acc) { 0 } : new List<int>(acc.Take(acc.Count - 1)) { acc.Last() + 1 })
                    .Where(mvpa_period => mvpa_period >= _Mvpa_Minimum_Period)
                    .Sum();
        }
    }
}
