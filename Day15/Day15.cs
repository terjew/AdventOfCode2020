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
            int[] roundSpoken = new int[count];
            for (int i = 0; i < start.Length - 1; i++)
            {
                var number = start[i];
                roundSpoken[number] = i + 1;
            }

            int lastSpoken = start[start.Length - 1];
            for (int lastTurn = start.Length ; lastTurn <= count -1; lastTurn++)
            {
                var round = roundSpoken[lastSpoken];
                int speak = (round > 0) ? lastTurn - round : 0;
                roundSpoken[lastSpoken] = lastTurn;
                lastSpoken = speak;
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
            },5, 3, 3);
            Console.WriteLine(part1);
            Console.WriteLine(part2);
        }
    }
}
