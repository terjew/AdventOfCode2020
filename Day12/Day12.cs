using System;
using System.IO;
using System.Runtime.CompilerServices;
using Utilities;

namespace Day12
{
    class Day12
    {
        static void Main(string[] args)
        {
            int dist1 = 0;
            int dist2 = 0;

            Performance.TimeRun("Read and solve part 1", () =>
            {
                dist1 = Part1("input.txt");
            }, 10, 100);

            Performance.TimeRun("Read and solve part 2", () =>
            {
                dist2 = Part2("input.txt");
            }, 10, 100);

            Performance.TimeRun("Read (twice) and solve both", () =>
            {
                dist1 = Part1("input.txt");
                dist2 = Part2("input.txt");
            }, 100, 1000);

            Console.WriteLine($"Part 1: {dist1}");
            Console.WriteLine($"Part 2: {dist2}");
        }

        public static int Part1(string path)
        {
            int x = 0;
            int y = 0;
            int dir = 0;
            using (StreamReader sr = File.OpenText(path))
            {
                string l = null;
                char c;
                int num;
                while ((l = sr.ReadLine()) != null)
                {
                    c = l[0];
                    num = 0;
                    for (int i = 1; i < l.Length; i++)
                        num = num * 10 + (l[i] - '0');
                    switch (c)
                    {
                        case 'F':
                            switch (dir)
                            {
                                case 0:
                                    x += num;
                                    break;
                                case 90:
                                    y -= num;
                                    break;
                                case 180:
                                    x -= num;
                                    break;
                                case 270:
                                    y += num;
                                    break;
                            }
                            break;
                        case 'E':
                            x += num;
                            break;
                        case 'S':
                            y -= num;
                            break;
                        case 'W':
                            x -= num;
                            break;
                        case 'N':
                            y += num;
                            break;
                        case 'R':
                            dir = (dir + num) % 360;
                            break;
                        case 'L':
                            dir = (360 + dir - num) % 360;
                            break;
                    }
                }
            }
            return Math.Abs(x) + Math.Abs(y);
        }

        public static int Part2(string path)
        {
            int dx = 10;
            int dy = 1;
            int x = 0;
            int y = 0;
            using (StreamReader sr = File.OpenText(path))
            {
                string l = null;
                char c;
                int num;
                while ((l = sr.ReadLine()) != null)
                {
                    c = l[0];
                    num = 0;
                    for (int i = 1; i < l.Length; i++)
                        num = num * 10 + (l[i] - '0');
                    switch (c)
                    {
                        case 'F':
                            x += dx * num;
                            y += dy * num;
                            break;
                        case 'E':
                            dx += num;
                            break;
                        case 'S':
                            dy -= num;
                            break;
                        case 'W':
                            dx -= num;
                            break;
                        case 'N':
                            dy += num;
                            break;
                        case 'R':
                            Rotate(ref dx, ref dy, num);
                            break;
                        case 'L':
                            Rotate(ref dx, ref dy, -num);
                            break;
                    }
                }
            }
            return Math.Abs(x) + Math.Abs(y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Rotate(ref int dx, ref int dy, int degrees)
        {
            int tmp;
            switch ((360 + degrees) % 360)
            {
                case 0:
                    return;
                case 90:
                    tmp = dx;
                    dx = dy;
                    dy = -tmp;
                    return;
                case 180:
                    dx = -dx;
                    dy = -dy;
                    return;
                case 270:
                    tmp = dx;
                    dx = -dy;
                    dy = tmp;
                    return;
            }
        }
    }
}
