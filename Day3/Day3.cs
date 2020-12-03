using System;
using System.Collections.Generic;
using System.Drawing;
using Utilities;

namespace Day3
{
    public class Slope
    {
        private List<string> lines;
        public int Width { get; }
        public int Height { get; }

        public Slope(List<string> lines)
        {
            this.lines = lines;
            Width = lines[0].Length;
            Height = lines.Count;
        }

        public char GetChar(Point p)
        {
            return lines[p.Y][p.X % Width];
        }
    }

    class Day3
    {
        static void Main(string[] args)
        {
            int numTrees = 0;
            long product = 0;
            Performance.TimeRun("Both stars", () => {
                var slope = new Slope(TextFile.ReadStringList("input.txt"));
                numTrees = CountTrees(slope, new Size(3, 1));
                List<Size> vectors = new List<Size>()
                {
                    new Size(1, 1),
                    new Size(5, 1),
                    new Size(7, 1),
                    new Size(1, 2),
                };
                product = numTrees;
                for (int i = 0; i < vectors.Count; i++)
                {
                    product *= CountTrees(slope, vectors[i]);
                }
            });
            Console.WriteLine($"Crashed into {numTrees} trees");
            Console.WriteLine($"Product is {product}");
        }

        public static int CountTrees(Slope slope, Size vector)
        {
            Point pos = new Point(0, 0);
            int numTrees = 0;
            while (pos.Y < slope.Height)
            {
                if (slope.GetChar(pos) == '#') numTrees++;
                pos += vector;
            }
            return numTrees;
        }
    }
}
