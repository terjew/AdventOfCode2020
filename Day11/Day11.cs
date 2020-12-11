using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Utilities;

namespace Day11
{
    

    class Day11
    {
        static void Main(string[] args)
        {
            int count1 = 0;
            int iter1 = 0;
            int count2 = 0;
            int iter2 = 0;

            Performance.TimeRun("part1 + 2 (with IO)", () =>
            {
                var input = TextFile.ReadStringList("input.txt");
                var matrix = Matrix<char>.BuildCharMatrix(input);
                var original = matrix.Clone();
                (count1, iter1) = SimulateWithAdjecency(matrix);
                (count2, iter2) = SimulateWithAdjecency(original, adjacencyFunc: FindClosestSeats, crowdedThreshold: 5);
            }, 10, 10);
            Console.WriteLine($"Part1: {count1} seats end up occupied ({iter1} iterations)");
            Console.WriteLine($"Part2: {count2} seats end up occupied ({iter2} iterations)");
        }

        static (int,int) SimulateWithAdjecency(Matrix<char> matrix, Func<int, int, Matrix<char>, int[]> adjacencyFunc = null, int crowdedThreshold = 4, bool output = false)
        {
            if (adjacencyFunc == null) adjacencyFunc = FindImmediateNeighbours;
            int[] indices = new int[128*128];             //max 128*128
            int[][] adjacencyLists = new int[128*128][];  //max 128*128

            for (int y = 0; y < matrix.Height; y++)
            {
                for (int x = 0; x < matrix.Width; x++)
                {
                    int index = (y << 7) + x;
                    var ptr = matrix.GetIndex(x, y);
                    indices[index] = ptr;
                    adjacencyLists[index] = adjacencyFunc(x, y, matrix);
                }
            }

            var current = matrix;
            var next = matrix.Clone();
            int numFlipped = 1;
            int iterations = 0;
            while (numFlipped > 0)
            {
                numFlipped = 0;
                for (int y = 0; y < matrix.Height; y++)
                {
                    for (int x = 0; x < matrix.Width; x++)
                    {
                        int index = (y << 7) + x;
                        var adjacency = adjacencyLists[index];
                        if (adjacency == null) continue; //null or floor
                        var ptr = indices[index];
                        var c = current.Array[ptr];
                        if (c == 'L' && IsClear(adjacency, current))
                        {
                            next.Array[ptr] = '#';
                            numFlipped++;
                        }
                        else if (c == '#' && IsCrowded(adjacency, current, crowdedThreshold))
                        {
                            next.Array[ptr] = 'L';
                            numFlipped++;
                        }
                        else
                        {
                            next.Array[ptr] = current.Array[ptr];
                        }
                    }
                }
                if (output)
                {
                    Console.CursorLeft = 0;
                    Console.CursorTop = 0;
                    next.Dump();
                    Thread.Sleep(50);
                }
                //bufferswap
                var tmp = current;
                current = next;
                next = tmp;
                iterations++;
            }

            return (current.Array.Count(c => c == '#'), iterations);
        }

        static readonly (int, int)[] offsets = new[]
        {
            (-1, -1),
            ( 0, -1),
            ( 1, -1),
            (-1,  0),
            ( 1,  0),
            (-1,  1),
            ( 0,  1),
            ( 1,  1),
        };

        private static int[] FindImmediateNeighbours(int x, int y, Matrix<char> matrix)
        {
            var c = matrix[x, y];
            if (c == '.' || c == '\0') return null;

            var list = new List<int>();
            foreach (var offset in offsets)
            {
                var dx = x + offset.Item1;
                var dy = y + offset.Item2;
                if (dx >= 0 && dx < matrix.Width && dy >= 0 && dy < matrix.Height)
                {
                    var ptr = matrix.GetIndex(dx, dy);
                    c = matrix.Array[ptr];
                    if (c == 'L' || c == '#') list.Add(ptr);
                }
            }
            return list.ToArray();
        }

        private static int[] FindClosestSeats(int x, int y, Matrix<char> matrix)
        {
            var c = matrix[x, y];
            if (c == '.' || c == '\0') return null;

            var list = new List<int>();
            foreach (var offset in offsets)
            {
                var dx = x + offset.Item1;
                var dy = y + offset.Item2;
                while (dx >= 0 && dx < matrix.Width && dy >= 0 && dy < matrix.Height)
                {
                    var ptr = matrix.GetIndex(dx, dy);
                    c = matrix.Array[ptr];
                    if (c == 'L' || c == '#')
                    {
                        list.Add(ptr);
                        break;
                    }
                    dx += offset.Item1;
                    dy += offset.Item2;
                }
            }
            return list.ToArray();
        }

        private static bool IsCrowded(int[] adjacency, Matrix<char> m, int threshold)
        {
            if (adjacency.Length < threshold) return false;
            int count = 0;
            foreach (var n in adjacency) if (m.Array[n] == '#') count++;
            return count >= threshold;
        }

        private static bool IsClear(int[] adjacency, Matrix<char> m)
        {
            foreach (var n in adjacency) if (m.Array[n] == '#') return false;
            return true;
        }
        
    }
}
