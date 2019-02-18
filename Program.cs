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
            var calculator = new MvpaCalculator("mock_heart_rate.txt");

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var totalMvpa = calculator.Calculate();
            sw.Stop();

            Console.WriteLine($"Result is {totalMvpa}, and ellapsed time is {sw.Elapsed.TotalMilliseconds} ms.");
        }
    }

    internal class MvpaCalculator
    {
        private const int _Mvpa_Heart_Rate_Threshold = 118;
        private const int _Mvpa_Minimum_Period = 10;
        
        private readonly List<int> _HeartRates;

        internal MvpaCalculator(string filePath)
        {
            _HeartRates = new List<int>(File.ReadAllLines(filePath).Select(d => int.Parse(d)));
        }

        internal int Calculate()
        {
            return
                _HeartRates
                    .Select(heart_rate => heart_rate > _Mvpa_Heart_Rate_Threshold ? 1 : 0)
                    .Aggregate(
                        new List<int> { 0 },
                        AccumulateMutable)
                    .Where(mvpa_period => mvpa_period >= _Mvpa_Minimum_Period)
                    .Sum();
        }

        private List<int> AccumulateImmutable(List<int> acc, int current)
        {
            return current == 0 ? new List<int>(acc) { 0 } : new List<int>(acc.Take(acc.Count - 1)) { acc.Last() + 1 };
        }

        private List<int> AccumulateMutable(List<int> acc, int current)
        {
            if(current == 0)
            {
                acc.Add(0);
                return acc;
            }
            else
            {
                acc[acc.Count - 1] = acc.Last() + 1;
                return acc;
            }
        }
    }
}
