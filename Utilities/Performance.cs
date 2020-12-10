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
        public static void TimeRun(string what, Action a, int runs = 10, int loops = 100)
        {
            int warmupCount = 10;
            for (int i = 0; i < warmupCount; i++)
            {
                a();
            }
            long totalTicks = 0;
            for (int run = 0; run < runs; run++)
            {
                Stopwatch sw = Stopwatch.StartNew();
                for (int loop = 0; loop < loops; loop++)
                {
                    a();
                }
                sw.Stop();
                totalTicks += sw.ElapsedTicks;
            }
            double ticksPerSecond = (double)Stopwatch.Frequency;

            Console.WriteLine($"{what}: {totalTicks} ticks total in {runs * loops} iterations ({runs} runs of {loops} loops), {totalTicks * 1000000 / ticksPerSecond  / (runs * loops)} µs/run");
        }
    }
}
