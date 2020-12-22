using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Utilities;

namespace Day20
{
	public enum Edge
	{
		Top,
		Right,
		Bottom,
		Left
	}

	public class Tile
	{
		public int TileNo;
		public int CurrentOrientation = 0;

		private Matrix2D<char> matrix;
		private int[][] orientedEdges = new int[8][];

		public int GetEdge(Edge edge)
        {
			return orientedEdges[CurrentOrientation][(int)edge];
        }

		public Tile(int tileNo, List<string> lines)
        {
			TileNo = tileNo;
			matrix = MatrixFactory.BuildCharMatrix(lines);

			//build orientations/edges:
			for(int i = 0; i < 8; i++)
            {
				var rotate = (i & 4) > 0;
				var hflip = (i & 2) > 0;
				var vflip = (i & 1) > 0;
				int[] edges = new int[4];
				for (int e = 0; e < 4; e++)
                {
					Edge edge = (Edge)e;
					edges[e] = ToInt(GetOrientedEdge(edge, rotate, hflip, vflip));
				}
				orientedEdges[i] = edges;
			}
        }

		static int ToInt(char[] arr)
		{
			int val = 0;
			for (int i = 0; i < arr.Length; i++)
			{
				int bit = arr[i] == '#' ? 1 : 0;
				val = (val << 1) + bit;
			}
			return val;
		}

		private char[] ReadRawEdge(Edge edge)
		{
			switch (edge)
			{
				case Edge.Top:
					return matrix.ReadRow(0);
				case Edge.Right:
					return matrix.ReadColumn(matrix.Width - 1);
				case Edge.Bottom:
					return matrix.ReadRow(matrix.Height - 1);
				case Edge.Left:
					return matrix.ReadColumn(0);
				default:
					throw new ArgumentException();
			}
		}

		//Before reading, the matrix is rotated then flipped.
		private char[] GetOrientedEdge(Edge edge, bool rotate90, bool hflip, bool vflip)
		{
			bool reverse = false;

			if (hflip)
			{
				if (edge == Edge.Left) edge = Edge.Right;
				else if (edge == Edge.Right) edge = Edge.Left;
				else
				{
					reverse = !reverse;
				}
			}

			if (vflip)
			{
				if (edge == Edge.Top) edge = Edge.Bottom;
				else if (edge == Edge.Bottom) edge = Edge.Top;
				else
				{
					reverse = !reverse;
				}
			}

			if (rotate90)
			{
				if (edge == Edge.Left || edge == Edge.Right) reverse = !reverse;
				edge = (Edge)(((int)edge + 1) % 4);
			}

			var arr = ReadRawEdge(edge);
			return reverse ? arr.Reverse().ToArray() : arr;
		}

		static void AssertEqual(string a, char[] b, [CallerLineNumber] int line = 0)
		{
			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] != b[i])
				{
					Console.WriteLine($"Assert on line {line} failed! \nExpected:\n{a}\nActual:\n{new string(b)}");
					return;
				}
			}
		}

		public static void Test()
		{
			List<string> lines = new List<string>()
			{
				"#.#.#####.",
				".#..######",
				"..#.......",
				"######....",
				"####.#..#.",
				".#...#.##.",
				"#.#####.##",
				"..#.###...",
				"..#.......",
				"..#.###...",
			};

			var tile = new Tile(1337, lines);

			AssertEqual("#.#.#####.", tile.GetOrientedEdge(Edge.Top, false, false, false));
			AssertEqual("..#.###...", tile.GetOrientedEdge(Edge.Bottom, false, false, false));
			AssertEqual("#..##.#...", tile.GetOrientedEdge(Edge.Left, false, false, false));
			AssertEqual(".#....#...", tile.GetOrientedEdge(Edge.Right, false, false, false));

			AssertEqual(".#....#...", tile.GetOrientedEdge(Edge.Top, true, false, false));
			AssertEqual("#..##.#...", tile.GetOrientedEdge(Edge.Bottom, true, false, false));
			AssertEqual(".#####.#.#", tile.GetOrientedEdge(Edge.Left, true, false, false));
			AssertEqual("...###.#..", tile.GetOrientedEdge(Edge.Right, true, false, false));

			AssertEqual(".#####.#.#", tile.GetOrientedEdge(Edge.Top, false, true, false));
			AssertEqual("...###.#..", tile.GetOrientedEdge(Edge.Bottom, false, true, false));
			AssertEqual(".#....#...", tile.GetOrientedEdge(Edge.Left, false, true, false));
			AssertEqual("#..##.#...", tile.GetOrientedEdge(Edge.Right, false, true, false));

			AssertEqual("...#....#.", tile.GetOrientedEdge(Edge.Top, true, true, false));
			AssertEqual("...#.##..#", tile.GetOrientedEdge(Edge.Bottom, true, true, false));
			AssertEqual(".#####.#.#", tile.GetOrientedEdge(Edge.Right, true, true, false));
			AssertEqual("...###.#..", tile.GetOrientedEdge(Edge.Left, true, true, false));

			AssertEqual("..#.###...", tile.GetOrientedEdge(Edge.Top, false, false, true));
			AssertEqual("#.#.#####.", tile.GetOrientedEdge(Edge.Bottom, false, false, true));
			AssertEqual("...#.##..#", tile.GetOrientedEdge(Edge.Left, false, false, true));
			AssertEqual("...#....#.", tile.GetOrientedEdge(Edge.Right, false, false, true));

			AssertEqual(".#....#...", tile.GetOrientedEdge(Edge.Bottom, true, false, true));
			AssertEqual("#..##.#...", tile.GetOrientedEdge(Edge.Top, true, false, true));
			AssertEqual("#.#.#####.", tile.GetOrientedEdge(Edge.Left, true, false, true));
			AssertEqual("..#.###...", tile.GetOrientedEdge(Edge.Right, true, false, true));

			AssertEqual("...###.#..", tile.GetOrientedEdge(Edge.Top, false, true, true));
			AssertEqual(".#####.#.#", tile.GetOrientedEdge(Edge.Bottom, false, true, true));
			AssertEqual("...#....#.", tile.GetOrientedEdge(Edge.Left, false, true, true));
			AssertEqual("...#.##..#", tile.GetOrientedEdge(Edge.Right, false, true, true));

			AssertEqual("...#.##..#", tile.GetOrientedEdge(Edge.Top, true, true, true));
			AssertEqual("...#....#.", tile.GetOrientedEdge(Edge.Bottom, true, true, true));
			AssertEqual("#.#.#####.", tile.GetOrientedEdge(Edge.Right, true, true, true));
			AssertEqual("..#.###...", tile.GetOrientedEdge(Edge.Left, true, true, true));
		}
	}

}