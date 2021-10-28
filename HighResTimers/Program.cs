using System;
using System.Diagnostics;

namespace HighResTimers
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();

            Console.WriteLine($"Frequency: {Stopwatch.Frequency}");
            Console.WriteLine($"IsHighResolution: {Stopwatch.IsHighResolution}");
        }
    }
}
