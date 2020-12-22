using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Utilities;

namespace Day20
{
    class Day20
    {
        
        static void Main(string[] args)
        {
            Tile.Test();
            long part1 = 0;
            Performance.TimeRun("Part1", () => part1 = Part1(), 10, 10, 10);
            Console.WriteLine($"Part 1: {part1}");
        }

        static long Part1()
        {
            HashSet<Tile> tiles = new HashSet<Tile>();
            using (StreamReader sr = File.OpenText("input.txt"))
            {
                string s = String.Empty;
                List<string> lines = new List<string>();
                int tileNo = 0;
                while ((s = sr.ReadLine()) != null)
                {
                    if (s.StartsWith("Tile"))
                    {
                        tileNo = int.Parse(s.AsSpan(5, s.Length - 6));
                    }
                    else if (s.StartsWith('.') || s.StartsWith('#'))
                    {
                        lines.Add(s);
                    }
                    else if (s.Length == 0)
                    {
                        tiles.Add(new Tile(tileNo, lines));
                        lines.Clear();
                    }
                }
                if (lines.Count > 0)
                {
                    tiles.Add(new Tile(tileNo, lines));
                }
            }

            int dim = (int)Math.Sqrt(tiles.Count);
            Tile[] picture = new Tile[tiles.Count];
            if (!TryFitTile(picture, dim, tiles, new HashSet<Tile>()))
            {
                Console.WriteLine("No solution found");
                return -1;
            }
            else
            {
                long answer = picture[0].TileNo;
                answer *= picture[dim - 1].TileNo;
                answer *= picture[(dim - 1) * dim].TileNo;
                answer *= picture[dim * dim - 1].TileNo;
                return answer;
            }
        }

        static bool TryFitTile(Tile[] picture, int dim, HashSet<Tile> allTiles, HashSet<Tile> tilesPlaced)
        {
            int next = tilesPlaced.Count;
            int y = next / dim;
            int x = next % dim;
            Tile top = y > 0 ? picture[next - dim] : null;
            Tile left = x > 0 ? picture[next - 1] : null;
            int? matchTop = top?.GetEdge(Edge.Bottom);
            int? matchLeft = left?.GetEdge(Edge.Right);

            var available = allTiles.Except(tilesPlaced);
            foreach (var tile in available)
            {
                tilesPlaced.Add(tile);
                picture[next] = tile;
                for (int i = 0; i < 8; i++)
                {
                    tile.CurrentOrientation = i;
                    if ((matchLeft == null || matchLeft == tile.GetEdge(Edge.Left)) &&
                        (matchTop == null || matchTop == tile.GetEdge(Edge.Top)))
                    {
                        if (next + 1 == dim * dim) return true;
                        if (TryFitTile(picture, dim, allTiles, tilesPlaced)) return true;
                    }
                }
                tilesPlaced.Remove(tile);
            }

            return false;
        }
    }
}
