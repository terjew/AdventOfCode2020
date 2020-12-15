using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace Day15
{
    class Day15
    {
        private static int CalculateSequence(int[] start, int count)
        {
            (int, int)[] turnsSpoken = Enumerable.Repeat((-1, -1), count).ToArray();
            for (int i = 0; i < start.Length; i++)
            {
                var number = start[i];
                var info = turnsSpoken[number];
                if (info.Item1 == -1)
                {
                    info.Item1 = i;
                }
                else
                {
                    info.Item2 = i - info.Item1;
                    info.Item1 = i;
                }
                turnsSpoken[number] = info;
            }

            int lastSpoken = start[start.Length - 1];
            for (int i = start.Length; i < count; i++)
            {
                var info = turnsSpoken[lastSpoken];
                int diff = info.Item2;
                if (diff > 0)
                {
                    lastSpoken = diff;
                }
                else
                {
                    lastSpoken = 0;
                }
                info = turnsSpoken[lastSpoken];
                if (info.Item1 == -1)
                {
                    info.Item1 = i;
                }
                else
                {
                    info.Item2 = i - info.Item1;
                    info.Item1 = i;
                }
                turnsSpoken[lastSpoken] = info;

            }
            return lastSpoken;
        }

        static void Main(string[] args)
        {
            int[] start = { 11, 18, 0, 20, 1, 7, 16 };
            int part1 = 0;
            int part2 = 0;
            Performance.TimeRun("Part 1 and 2", () =>
            {
                part1 = CalculateSequence(start, 2020);
                part2 = CalculateSequence(start, 30000000);
            },5, 1, 3);
            Console.WriteLine(part1);
            Console.WriteLine(part2);
        }
    }
}
