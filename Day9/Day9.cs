using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace Day9
{
    class RingBuffer2D<T>
    {
        public readonly T[] Array;
        public int XOffset = 0;
        public int YOffset = 0;
        public readonly int Width;
        public readonly int Height;

        public RingBuffer2D(int width, int height)
        {
            Array = new T[width * height];
            Width = width;
            Height = height;
        }

        private int mod(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }

        public T Read(int x, int y)
        {
            int dx = mod(x + XOffset, Width);
            int dy = mod(y + YOffset, Height);
            return Array[dy * Width + dx];
        }

        public void Write(int x, int y, T value)
        {
            int dx = mod(x + XOffset, Width);
            int dy = mod(y + YOffset, Height);
            Array[dy * Width + dx] = value;
        }

        public void SetRow(int y, T value)
        {
            for (int x = 0; x < Width; x++) Write(x, y, value);
        }

        public void Dump()
        {
            Console.WriteLine($"********************");
            for(int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Console.Write($"{Read(x, y)}\t");
                }
                Console.WriteLine();
            }
        }
    }

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
            Performance.TimeRun("Solve part 1 (naive)", () =>
            {
                part1 = SolvePart1Naive(sequence, 25);
            }, 100, 100);
            Performance.TimeRun("Solve part 1 (ringbuffer)", () =>
            {
                part1 = SolvePart1Ringbuffer(sequence, 25);
            }, 100, 100);
            Performance.TimeRun("Solve part 1 (queues)", () =>
            {
                part1 = SolvePart1(sequence, 25);
            }, 100, 100);
            Performance.TimeRun("Solve part 2", () =>
            {
                part2 = SolvePart2(sequence, part1);
            }, 100, 10000);
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

        public static long SolvePart1Ringbuffer(List<long> sequence, int preamble)
        {
            var window = new RingBuffer2D<long>(preamble, 1);
            var sums = new RingBuffer2D<long>(preamble, preamble);
            int seqNo = 0;
            foreach (long input in sequence)
            {
                int num = Math.Min(seqNo, preamble);

                if (seqNo >= preamble)
                {
                    //Test against existing
                    if (!sums.Array.Contains(input))
                    {
                        return input;
                    }
                }

                window.XOffset++;
                sums.XOffset++;
                sums.YOffset++;

                //push right column:
                if (seqNo >= preamble)
                {
                    sums.SetRow(-1, 0);
                }
                for (int i = 0; i < num; i++)
                {
                    long sum = input + window.Read(-i -2, 0);
                    sums.Write(-1, -i-1, sum);
                }
                window.Write(-1, 0, input);
                seqNo++;
            }
            return -1;
        }

        private static bool HasPairSummingTo(long[] arr, long target)
        {
            int num = arr.Length;
            for (int a = 0; a < num; a++)
            {
                for (int b = a + 1; b < num; b++)
                {
                    if (arr[a] + arr[b] == target) return true;
                }
            }
            return false;
        }

        public static long SolvePart1Naive(List<long> sequence, int preamble)
        {

            var window = new RingBuffer2D<long>(preamble, 1);
            int seqNo = 0;
            foreach (long input in sequence)
            {
                if (seqNo >= preamble)
                {
                    if (!HasPairSummingTo(window.Array, input)) return input;
                }

                window.Write(0, 0, input);
                window.XOffset++;
                seqNo++;
            }
            return -1;
        }
    }
}
