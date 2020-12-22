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
            Tile[] tiledPicture = null;
            int dim = 0;

            Performance.TimeRun("Part1", () => (part1, tiledPicture, dim) = Part1("input.txt"), 5, 10, 5);
            Console.WriteLine($"Part 1: {part1}");

            int part2 = 0;
            Matrix2D<char> chart = null;
            Performance.TimeRun("Part2", () => (part2, chart) = Part2(tiledPicture, dim), 2, 1000, 5);

            Console.WriteLine($"Part 2: {part2}");
            var colormap = CharMatrix.DefaultColorMap();
            colormap['*'] = ConsoleColor.White;
            colormap['.'] = ConsoleColor.DarkBlue;
            colormap['#'] = ConsoleColor.Blue;
            colormap['O'] = ConsoleColor.Green;
            chart.DumpColor(colormap);
        }

        static (int, Matrix2D<char>) Part2(Tile[] tiledPicture, int dim)
        {
            var chart = CombineTiles(tiledPicture, dim);

            var seaMonsterLines = new List<string>()
            {
                "                  # ",
                "#    ##    ##    ###",
                " #  #  #  #  #  #   ",
            };
            var seaMonster = CharMatrix.Build(seaMonsterLines);

            bool found = false;
            Matrix2D<char> transformedChart = null;
            for (int orientation = 0; orientation < 8; orientation++)
            {
                chart.CurrentOrientation = orientation;
                transformedChart = chart.GetTransformedMatrix();

                for (int y = 0; y < transformedChart.Height - seaMonster.Height; y++)
                {
                    for (int x = 0; x < transformedChart.Width - seaMonster.Width; x++)
                    {
                        if (HasAllPixels(transformedChart, seaMonster, x, y))
                        {
                            found = true;
                            StampValue(transformedChart, seaMonster, x, y, 'O');
                        }
                    }
                }
                if (found) break;
            }

            if (!found) throw new ArgumentException("No sea monsters in image!");
            return (transformedChart.Array.Count(c => c == '#'), transformedChart);
        }

        public static bool HasAllPixels(Matrix2D<char> map, Matrix2D<char> sprite, int spriteX, int spriteY)
        {
            for (int y = 0; y < sprite.Height; y++)
            {
                for (int x = 0; x < sprite.Width; x++)
                {
                    if (sprite[x, y] != ' ' && map[spriteX + x, spriteY + y] != sprite[x, y]) return false;
                }
            }
            return true;
        }

        public static bool StampValue(Matrix2D<char> map, Matrix2D<char> sprite, int spriteX, int spriteY, char value)
        {
            for (int y = 0; y < sprite.Height; y++)
            {
                var py = spriteY + y;
                if (py < 0 || py >= map.Height) continue;
                for (int x = 0; x < sprite.Width; x++)
                {
                    var px = spriteX + x;
                    if (px < 0 || px >= map.Width) continue;
                    if ((sprite[x, y] != ' ')) map[px, py] = value;
                }
            }
            return false;
        }

        static Tile CombineTiles(Tile[] tiles, int dim)
        {
            List<string> lines = new List<string>();
            int tileWidth = tiles[0].Width;
            int tileHeight = tiles[0].Height;

            int totalRows = (tileHeight - 2) * dim;
            for (int i = 0; i < totalRows; i++) lines.Add("");

            int rowsPerTile = tileHeight - 2;
            for (int y = 0; y < dim; y++)
            {
                for (int x = 0; x < dim; x++)
                {
                    Tile tile = tiles[y * dim + x];
                    for (int row = 1; row < tileHeight - 1; row++)
                    {
                        var line = new string(tile.GetTransformedLine(row, true));
                        line = line.Substring(1, line.Length - 2);
                        lines[y * rowsPerTile + row - 1] += line;
                    }
                }
            }

            return new Tile(0, lines);
        }

        static Tile CombineTilesWithGap(Tile[] tiles, int dim)
        {
            List<string> lines = new List<string>();
            int tileWidth = tiles[0].Width;
            int tileHeight = tiles[0].Height;

            int totalRows = (tileHeight + 1) * dim - 1;
            for (int i = 0; i < totalRows; i++) lines.Add("");

            int rowsPerTile = tileHeight + 1;
            for (int y = 0; y < dim; y++)
            {
                int nRows = rowsPerTile - 1;
                for (int x = 0; x < dim; x++)
                {
                    Tile tile = tiles[y * dim + x];
                    for (int row = 0; row < nRows; row++)
                    {
                        var line = new string(tile.GetTransformedLine(row, true));
                        if (x != dim - 1)
                        {
                            line += " ";
                        }
                        lines[y * rowsPerTile + row] += line;
                    }
                    if (y != dim - 1)
                    {
                        lines[y * rowsPerTile + nRows] = new string(' ', 32);
                    }
                }
            }

            return new Tile(0, lines);
        }

        static (long, Tile[], int) Part1(string path)
        {
            HashSet<Tile> tiles = new HashSet<Tile>();
            using (StreamReader sr = File.OpenText(path))
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
                throw new ArgumentException("No solution found");                
            }
            else
            {
                long answer = picture[0].TileNo;
                answer *= picture[dim - 1].TileNo;
                answer *= picture[(dim - 1) * dim].TileNo;
                answer *= picture[dim * dim - 1].TileNo;
                return (answer, picture, dim);
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
