using System;
using System.Linq;
using Utilities;

namespace Day17
{
    class Day17
    {

        static int Solve3D(string[] initialState)
        {
            int w = 21;
            int h = 21;
            int d = 21;

            var current = new Matrix3D<char>(w, h, d, Enumerable.Repeat('.', w * h * d).ToArray());
            int cx = 6;
            int cy = 6;
            int cz = 10;
            for (int y = 0; y < initialState.Length; y++)
            {
                for (int x = 0; x < initialState[0].Length; x++)
                {
                    current[cx + x, cy + y, cz] = initialState[y][x];
                }
            }

            var next = current.Clone();
            for (int i = 1; i <= 6; i++)
            {
                for (int z = 0; z < d; z++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        for (int x = 0; x < w; x++)
                        {
                            var state = current[x, y, z];
                            int activeNeighbors = CountNeighbors(current, x, y, z);
                            if (state == '#')
                            {
                                next[x, y, z] = (activeNeighbors == 2 || activeNeighbors == 3) ? '#' : '.';
                            }
                            else
                            {
                                next[x, y, z] = activeNeighbors == 3 ? '#' : '.';
                            }
                        }
                    }
                }
                var tmp = current;
                current = next;
                next = tmp;
            }
            return current.Array.Count(c => c == '#');
        }

        static int Solve4D(string[] initialState)
        {
            int width = 21;
            int height = 21;
            int depth = 21;
            int wdim = 21;

            var current = new Matrix4D<char>(width, height, depth, wdim, Enumerable.Repeat('.', width * height * depth * wdim).ToArray());
            int cx = 6;
            int cy = 6;
            int cz = 10;
            int cw = 10;
            for (int y = 0; y < initialState.Length; y++)
            {
                for (int x = 0; x < initialState[0].Length; x++)
                {
                    current[cx + x, cy + y, cz, cw] = initialState[y][x];
                }
            }

            var next = current.Clone();
            for (int i = 1; i <= 6; i++)
            {
                for (int w = 0; w < wdim; w++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            for (int x = 0; x < width; x++)
                            {
                                var state = current[x, y, z, w];
                                int activeNeighbors = CountNeighbors(current, x, y, z, w);
                                if (state == '#')
                                {
                                    next[x, y, z, w] = (activeNeighbors == 2 || activeNeighbors == 3) ? '#' : '.';
                                }
                                else
                                {
                                    next[x, y, z, w] = activeNeighbors == 3 ? '#' : '.';
                                }
                            }
                        }
                    }
                }
                var tmp = current;
                current = next;
                next = tmp;
            }
            return current.Array.Count(c => c == '#');
        }

        static void Main(string[] args)
        {

            var initialState = new string[]
            {
                "...###.#",
                "#.#.##..",
                ".##.##..",
                "..##...#",
                ".###.##.",
                ".#..##..",
                ".....###",
                ".####..#",
            }; //8x8

            int part1 = 0;
            int part2 = 0;

            Performance.TimeRun("solve part1+2", () =>
            {
                part1 = Solve3D(initialState);
                part2 = Solve4D(initialState);
            },10,1);

            Console.WriteLine($"Part1: {part1} cubes are active");
            Console.WriteLine($"Part3: {part2} cubes are active");
        }

        static int CountNeighbors(Matrix4D<char> map, int x, int y, int z, int w)
        {
            int count = 0;
            foreach (var n in Matrix4D<char>.Neighborhood)
            {
                int px = x + n.Item1;
                int py = y + n.Item2;
                int pz = z + n.Item3;
                int pw = w + n.Item4;
                if (px < 0 || py < 0 || pz < 0 || pw < 0) continue;
                if (px >= map.Width || py >= map.Height || pz >= map.Depth || pw >= map.Wdim) continue;
                if (map[px, py, pz, pw] == '#') count++;
            }
            return count;
        }

        static int CountNeighbors(Matrix3D<char> map, int x, int y, int z)
        {
            int count = 0;
            foreach(var n in Matrix3D<char>.Neighborhood)
            {
                int px = x + n.Item1;
                int py = y + n.Item2;
                int pz = z + n.Item3;
                if (px < 0 || py < 0 || pz < 0) continue;
                if (px >= map.Width || py >= map.Height || pz >= map.Depth) continue;
                if (map[px, py, pz] == '#') count++;
            }
            return count;
        }
    }
}
