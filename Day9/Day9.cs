using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace Day9
{
    class Day9
    {
        static void Main(string[] args)
        {
            long part1 = 0;
            long part2 = 0;
            List<long> sequence = null;
            Performance.TimeRun("Parse", () =>
            {
                sequence = TextFile.ReadPositiveLongList("input.txt");
            }, 100, 100);
            Performance.TimeRun("Solve part 1", () =>
            {
                part1 = SolvePart1(sequence, 25);
            }, 100, 100);
            Performance.TimeRun("Solve part 2", () =>
            {
                part2 = SolvePart2(sequence, part1);
            }, 1000, 10000);
            Console.WriteLine($"Found failed checksum: {part1}");
            Console.WriteLine($"Found weakness: {part2}");
        }

        public static long SolvePart2(List<long> sequence, long target)
        {
            int head = 1;
            int tail = 0;
            long sum = sequence[head] + sequence[tail];
            while (sum != target)
            {
                if (sum < target)
                {
                    head++;
                    sum += sequence[head];
                }
                if (sum > target)
                {
                    sum -= sequence[tail];
                    tail++;
                }
            }
            //sum found, get smallest and largest in range
            long min = long.MaxValue;
            long max = long.MinValue;
            for(int i = tail; i <= head; i++)
            {
                long num = sequence[i];
                if (num > max) max = num;
                if (num < min) min = num;
            }
            return min + max;
        }

        public static long SolvePart1(List<long> sequence, int preamble)
        {
            List<long> buffer = new List<long>();
            List<Queue<long>> sumQueue = new List<Queue<long>>();
            int seqNo = 0;

            foreach(long input in sequence)
            {
                if (seqNo >= preamble)
                {
                    //Test against existing
                    if (!sumQueue.SelectMany(l => l).Contains(input))
                    {
                        return input;
                    }
                }
                for(int i = 0; i < buffer.Count; i++)
                {
                    sumQueue[i].Enqueue(buffer[i] + input);
                }

                buffer.Add(input);
                sumQueue.Add(new Queue<long>());
                if (seqNo >= preamble)
                {
                    buffer.RemoveAt(0);
                    sumQueue.RemoveAt(0);                    
                }
                seqNo++;
            }
            return -1;
        }
    }
}
