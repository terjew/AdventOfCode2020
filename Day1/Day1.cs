using System;
using System.Collections.Generic;
using Utilities;

namespace Day1
{
    class Day1
    {
        static void Main(string[] args)
        {
            long product1 = 0;
            long product2 = 0;

            List<int> numbers = null;
            Performance.TimeRun("Parse and solve", () =>
            {
                numbers = TextFile.ReadPositiveIntList("puzzle1.txt");

                int[] arr = null;
                arr = numbers.ToArray();
                Array.Sort(arr);

                (int num1, int num2) = FindPair(arr, 2020);
                product1 = num1 * num2;
                (int num3, int num4, int num5) = FindTriplet(arr, 2020);
                product2 = num3 * num4 * num5;
            });
            Console.WriteLine($"Answer 1: {product1}");
            Console.WriteLine($"Answer 2: {product2}");
        }

        public static (int a, int b) FindPair(int[] sorted, int targetSum)
        {
            int head = 0;
            int tail = sorted.Length - 1;
            int iterations = 0;
            while (true)
            {
                iterations++;
                var sum = sorted[head] + sorted[tail];
                if (sum == targetSum)
                {
                    break;
                }
                if (sum < targetSum)
                {
                    head++;
                }
                else
                {
                    tail--;
                }
                if (head >= tail) throw new ArgumentException("Invalid input, no pair sums to target");
            }
            var num1 = sorted[head];
            var num2 = sorted[tail];
            return (num1,num2);
        }

        public static (int a, int b, int c) FindTriplet(int[] sorted, int targetSum)
        {
            int head = 0;
            int mid = 1;
            int tail = sorted.Length - 1;
            int iterations = 0;
            while (true)
            {
                iterations++;
                var sum = sorted[head] + sorted[mid] + sorted[tail];
                if (sum == targetSum)
                {
                    break;
                }

                if (sum < targetSum)
                {
                    if (mid < tail - 1)
                    {
                        if (head < mid - 1)
                        {
                            //find smallest increment:
                            var midIncrement = sorted[mid + 1] - sorted[mid];
                            var headIncrement = sorted[head + 1] - sorted[head];
                            if (midIncrement < headIncrement)
                            {
                                mid++;
                            }
                            else
                            {
                                head++;
                            }
                        }
                        else
                        {
                            //must move mid:
                            mid++;
                        }
                    }
                    else
                    {
                        //must move head and reset mid
                        head++;
                        mid = head + 1;
                    }
                }
                else
                {
                    //move tail and reset mid
                    tail--;
                    mid = head + 1;
                }
                if (head >= tail) throw new ArgumentException("Invalid input, no triplet sums to target");
            }
            var num1 = sorted[head];
            var num2 = sorted[mid];
            var num3 = sorted[tail];
            return (num1, num2, num3);
        }

    }
}
