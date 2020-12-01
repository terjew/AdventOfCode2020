using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class Performance
    {
        public static void TimeRun(string what, Action a)
        {
            int warmupCount = 10;
            for (int i = 0; i < warmupCount; i++)
            {
                a();
            }
            int runs = 10;
            int loops = 100;
            double total = 0;
            for (int run = 0; run < runs; run++)
            {
                Stopwatch sw = Stopwatch.StartNew();
                for (int loop = 0; loop < loops; loop++)
                {
                    a();
                }
                sw.Stop();
                var ms = sw.ElapsedMilliseconds;
                //var us = sw.ElapsedTicks / ((double)Stopwatch.Frequency / 1000000);
                total += ms;
            }
            Console.WriteLine($"{what}: {total} ms total in {runs * loops} iterations ({runs} runs of {loops} loops), {total * 1000 / (runs * loops)} µs/run");
        }
    }
}
