using System;
using System.Collections.Generic;
using Utilities;

namespace Day10
{
    class GraphNode
    {
        public GraphNode(bool isGoal = false) { 
            Children = new List<GraphNode>();
            Paths = null;
            if (isGoal) Paths = 1;
        }

        public readonly List<GraphNode> Children;
        private long? Paths;
        public long CountPaths()
        {
            if (!Paths.HasValue)
            {
                long num = 0;
                foreach (var child in Children)
                {
                    num += child.CountPaths();
                }
                Paths = num;
            }

            return Paths.Value;
        }
    }

    class Day10
    {
        static int Part1(List<int> numbers)
        {
            numbers.Sort();
            int prev = 0;
            int n1 = 0;
            int n3 = 1;
            foreach (var num in numbers)
            {
                var diff = num - prev;
                prev = num;
                if (diff == 1) n1++;
                if (diff == 3) n3++;
            }
            return n1 * n3;
        }

        static long Part2Sloper(List<int> numbers)
        {
            int len = numbers.Count + 1;

            int[] x = new int[len];
            x[0] = 0;

            for (int i = 1; i < len; i++) x[i] = numbers[i-1];

            long[] res = new long[len];
            res[len - 1] = 1;
            res[len - 2] = 1;
            res[len - 3] = x[len - 1] - x[len - 3] <= 3 ? 2 : 1;
            for (int i = len - 4; i >= 0; i--)
            {
                res[i] = res[i + 1];
                if (x[i+2] - x[i] <= 3)
                {
                    res[i] += res[i + 2];
                    if (x[i+3] - x[i] <= 3)
                    {
                        res[i] += res[i + 3];
                    }
                }
            }
            return res[0];
        }

        static long Part2Reksten(List<int> numbersList)
        {
            var num = numbersList.ToArray();
            int len = num.Length;
            var count = new long[len];
            for (int i = 0; i <= 2; i++)
            {
                if (num[i] < 4) count[i] = 1;
            }

            for (int i = 0; i < len; i++)
            {
                var to = i;
                var end = num[i] + 3;
                while (++to < len && num[to] <= end)
                {
                    count[to] += count[i];
                }
            }
            return count[len - 1];
        }

        static long Part2Graph(List<int> numbers)
        {
            Dictionary<int, GraphNode> nodes = new Dictionary<int, GraphNode>();
            var max = numbers[numbers.Count - 1];
            nodes.Add(max, new GraphNode(true)); //goal
            for (int i = numbers.Count - 2; i >= 0; i--)
            {
                var num = numbers[i];
                var node = new GraphNode();
                for (int j = i + 1; j < numbers.Count; j++)
                {
                    var larger = numbers[j];
                    if ((larger - num) <= 3) node.Children.Add(nodes[larger]);
                    else break;
                }
                nodes.Add(num, node);
            }
            var start = new GraphNode();
            nodes.Add(0, start); //start
            for (int j = 0; j < 3; j++)
            {
                var larger = numbers[j];
                if (larger <= 3) start.Children.Add(nodes[larger]);
            }
            return start.CountPaths();
        }

        static void Main(string[] args)
        {
            int part1 = 0;
            long part2 = 0;
            List<int> numbers = null;


            numbers = TextFile.ReadPositiveIntList("input.txt");

            Performance.TimeRun("Copy and solve pt2 (array)", () =>
            {
                var list = new List<int>(numbers);
                list.Sort();
                Part2Sloper(list);
            }, 1000, 10000);

            Performance.TimeRun("Copy and solve pt2 (reksten)", () =>
            {
                var list = new List<int>(numbers);
                list.Sort();
                Part2Reksten(list);
            }, 1000, 10000);

            Performance.TimeRun("Copy and solve pt2 (graph)", () =>
            {
                var list = new List<int>(numbers);
                list.Sort();
                Part2Graph(list);
            }, 1000, 10000);

            Performance.TimeRun("Read and solve part1+2", () =>
            {
                numbers = TextFile.ReadPositiveIntList("input.txt");
                part1 = Part1(numbers);
                part2 = Part2Graph(numbers);
            }, 100, 1000);

            Console.WriteLine($"Part 1: {part1}");
            Console.WriteLine($"Part 2: {part2}");
        }
    }
}
