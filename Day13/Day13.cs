using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Utilities;

namespace Day13
{
    class Day13
    {
        static void Main(string[] args)
        {
            //string sample = "939\n7,13,x,x,59,x,31,19";
            string input = "1002392\n23,x,x,x,x,x,x,x,x,x,x,x,x,41,x,x,x,37,x,x,x,x,x,421,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,17,x,19,x,x,x,x,x,x,x,x,x,29,x,487,x,x,x,x,x,x,x,x,x,x,x,x,13";

            var lines = input.Split('\n');
            int earliest = int.Parse(lines[0]);
            int[] arr = lines[1].Split(',').Select(str => str == "x" ? -1: int.Parse(str)).ToArray();

            int part1 = 0;
            long part2 = 0;

            Performance.TimeRun("Part 1 and 2", () =>
            {
                part1 = Part1(earliest, arr);
                part2 = Part2(arr);
            }, 1000, 1000);

            Console.WriteLine($"Part 1: {part1}");
            Console.WriteLine($"Part 2: {part2}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Part1(int earliest, int[] arr)
        {
            var possible = arr.Where(i => i > 0).ToArray();
            var first = possible.Select(d => new { line = d, wait = d - (earliest % d) }).OrderBy(p => p.wait).First();
            return first.line * first.wait;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static long GetOffset(long t, long line)
        {
            var offset = line - (t % line);
            if (offset == line) offset = 0;
            return offset;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static long Part2(int[] arr)
        {
            int[] lines = arr.Where(i => i > 0).ToArray();
            long[] desiredOffsets = new long[lines.Length];
            desiredOffsets[0] = 0;
            int lineNo = 1;
            for (int pos = 1; pos < arr.Length; pos++)
            {
                int line = arr[pos];
                if (line == -1) continue;
                desiredOffsets[lineNo++] = pos % line;
            }

            long t = 0;
            long period = arr[0];

            for (int i = 1; i < lines.Length; i++)
            {
                int line = lines[i];
                long desiredOffset = desiredOffsets[i];
                var offset = GetOffset(t, line);
                while (offset != desiredOffset)
                {
                    t += period;
                    offset = GetOffset(t, line);
                }
                period *= line;
            }
            return t;
        }

    }
}
