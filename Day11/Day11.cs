using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Utilities;

namespace Day11
{
    public class Matrix
    {
        public char this[int x, int y]
        { 
            get {
                return Array[y * Width + x];
            }
            set {
                Array[y * Width + x] = value;
            }
        }

        public readonly char[] Array;
        public readonly int Width;
        public readonly int Height;

        public Matrix(List<string> input)
        {
            Width = input[0].Length;
            Height = input.Count;
            Array = new char[Width * Height];
            int lineptr = 0;
            foreach(var line in input)
            {
                int ptr = lineptr;
                foreach (var c in line)
                {
                    Array[ptr++] = c;
                }
                lineptr += Width;
            }
        }

        private Matrix(Matrix other) {
            Array = (char[])other.Array.Clone();
            Width = other.Width;
            Height = other.Height;
        }

        public int GetIndex(int x, int y)
        {
            return y * Width + x;
        }

        public Matrix Clone()
        {
            return new Matrix(this);
        }

        public void CopyFrom(Matrix other)
        {
            other.Array.CopyTo(Array, 0);
        }

        public void Dump()
        {
            Console.WriteLine("**************************************");
            for (int i = 0; i < Height; i++)
            {
                //FIXME: Use Span<char>
                var str = new string(Array, Width * i, Width);
                Console.WriteLine(str);
            }
        }
    }

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
                var matrix = new Matrix(input);
                var original = matrix.Clone();
                (count1, iter1) = SimulateWithAdjecency(matrix);
                (count2, iter2) = SimulateWithAdjecency(original, adjacencyFunc: FindClosestSeats, crowdedThreshold: 5);
            }, 10, 10);
            Console.WriteLine($"Part1: {count1} seats end up occupied ({iter1} iterations)");
            Console.WriteLine($"Part2: {count2} seats end up occupied ({iter2} iterations)");
        }

        static (int,int) SimulateWithAdjecency(Matrix matrix, Func<int, int, Matrix, int[]> adjacencyFunc = null, int crowdedThreshold = 4, bool output = false)
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

        private static int[] FindImmediateNeighbours(int x, int y, Matrix matrix)
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

        private static int[] FindClosestSeats(int x, int y, Matrix matrix)
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

        private static bool IsCrowded(int[] adjacency, Matrix m, int threshold)
        {
            if (adjacency.Length < threshold) return false;
            int count = 0;
            foreach (var n in adjacency) if (m.Array[n] == '#') count++;
            return count >= threshold;
        }

        private static bool IsClear(int[] adjacency, Matrix m)
        {
            foreach (var n in adjacency) if (m.Array[n] == '#') return false;
            return true;
        }
        
    }
}
