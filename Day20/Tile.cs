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
		public readonly int Width;
		public readonly int Height;

		public int GetEdge(Edge edge)
        {
			return orientedEdges[CurrentOrientation][(int)edge];
        }

		public char[] GetTransformedLine(int lineNo, bool horisontal)
        {
			bool rotate, hflip, vflip;
			(rotate, hflip, vflip) = DecodeTransform(CurrentOrientation);
			return GetTransformedLine(lineNo, horisontal, rotate, hflip, vflip);
		}

		public Matrix2D<char> GetTransformedMatrix()
        {
			bool rotate, hflip, vflip;
			(rotate, hflip, vflip) = DecodeTransform(CurrentOrientation);
			List<string> lines = new List<string>();
			for (int y = 0; y < (rotate ? Width : Height); y++)
            {
				lines.Add(new string(GetTransformedLine(y, true)));
            }
			return CharMatrix.Build(lines);
		}

		public (bool rotate, bool hflip, bool vflip) DecodeTransform(int packedTransform)
        {
			var rotate = (packedTransform & 4) > 0;
			var hflip = (packedTransform & 2) > 0;
			var vflip = (packedTransform & 1) > 0;
			return (rotate, hflip, vflip);
		}

		public Tile(int tileNo, List<string> lines, bool calculateEdges = true)
        {
			TileNo = tileNo;
			matrix = CharMatrix.Build(lines);
			Width = matrix.Width;
			Height = matrix.Height;
			if (calculateEdges)
            {
				//cache edges for all orientations:
				for (int i = 0; i < 8; i++)
				{
					bool rotate, hflip, vflip;
					(rotate, hflip, vflip) = DecodeTransform(i);
					int[] edges = new int[4];
					for (int e = 0; e < 4; e++)
					{
						Edge edge = (Edge)e;
						edges[e] = ToInt(GetTransformedEdge(edge, rotate, hflip, vflip));
					}
					orientedEdges[i] = edges;
				}

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

		//Before reading, the matrix is rotated then flipped.
		private char[] GetTransformedLine(int lineNo, bool horisontal, bool rotate90, bool hflip, bool vflip)
		{
			bool reverse = false;

			if (hflip)
			{
				if (!horisontal)
                {
					lineNo = matrix.Height - 1 - lineNo;
                }
				else
				{
					reverse = !reverse;
				}
			}

			if (vflip)
			{
				if (horisontal)
                {
					lineNo = matrix.Height - 1 - lineNo;
                }
				else
				{
					reverse = !reverse;
				}
			}

			if (rotate90)
			{
				if (!horisontal) reverse = !reverse;
				if (horisontal)
				{
					lineNo = matrix.Height - 1 - lineNo;
				}
				horisontal = !horisontal;
			}

			var arr = horisontal ? matrix.ReadRow(lineNo) : matrix.ReadColumn(lineNo);
			return reverse ? arr.Reverse().ToArray() : arr;
		}

		private char[] GetTransformedEdge(Edge edge, bool rotate90, bool hflip, bool vflip)
		{
            switch (edge)
            {
                case Edge.Top:
					return GetTransformedLine(0, true, rotate90, hflip, vflip);
                case Edge.Right:
					return GetTransformedLine(matrix.Width - 1, false, rotate90, hflip, vflip);
				case Edge.Bottom:
					return GetTransformedLine(matrix.Height - 1, true, rotate90, hflip, vflip);
				case Edge.Left:
					return GetTransformedLine(0, false, rotate90, hflip, vflip);
                default:
					throw new ArgumentException();                    
            }

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

			AssertEqual("#.#.#####.", tile.GetTransformedEdge(Edge.Top, false, false, false));
			AssertEqual("..#.###...", tile.GetTransformedEdge(Edge.Bottom, false, false, false));
			AssertEqual("#..##.#...", tile.GetTransformedEdge(Edge.Left, false, false, false));
			AssertEqual(".#....#...", tile.GetTransformedEdge(Edge.Right, false, false, false));

			AssertEqual(".#....#...", tile.GetTransformedEdge(Edge.Top, true, false, false));
			AssertEqual("#..##.#...", tile.GetTransformedEdge(Edge.Bottom, true, false, false));
			AssertEqual(".#####.#.#", tile.GetTransformedEdge(Edge.Left, true, false, false));
			AssertEqual("...###.#..", tile.GetTransformedEdge(Edge.Right, true, false, false));

			AssertEqual(".#####.#.#", tile.GetTransformedEdge(Edge.Top, false, true, false));
			AssertEqual("...###.#..", tile.GetTransformedEdge(Edge.Bottom, false, true, false));
			AssertEqual(".#....#...", tile.GetTransformedEdge(Edge.Left, false, true, false));
			AssertEqual("#..##.#...", tile.GetTransformedEdge(Edge.Right, false, true, false));

			AssertEqual("...#....#.", tile.GetTransformedEdge(Edge.Top, true, true, false));
			AssertEqual("...#.##..#", tile.GetTransformedEdge(Edge.Bottom, true, true, false));
			AssertEqual(".#####.#.#", tile.GetTransformedEdge(Edge.Right, true, true, false));
			AssertEqual("...###.#..", tile.GetTransformedEdge(Edge.Left, true, true, false));

			AssertEqual("..#.###...", tile.GetTransformedEdge(Edge.Top, false, false, true));
			AssertEqual("#.#.#####.", tile.GetTransformedEdge(Edge.Bottom, false, false, true));
			AssertEqual("...#.##..#", tile.GetTransformedEdge(Edge.Left, false, false, true));
			AssertEqual("...#....#.", tile.GetTransformedEdge(Edge.Right, false, false, true));

			AssertEqual(".#....#...", tile.GetTransformedEdge(Edge.Bottom, true, false, true));
			AssertEqual("#..##.#...", tile.GetTransformedEdge(Edge.Top, true, false, true));
			AssertEqual("#.#.#####.", tile.GetTransformedEdge(Edge.Left, true, false, true));
			AssertEqual("..#.###...", tile.GetTransformedEdge(Edge.Right, true, false, true));

			AssertEqual("...###.#..", tile.GetTransformedEdge(Edge.Top, false, true, true));
			AssertEqual(".#####.#.#", tile.GetTransformedEdge(Edge.Bottom, false, true, true));
			AssertEqual("...#....#.", tile.GetTransformedEdge(Edge.Left, false, true, true));
			AssertEqual("...#.##..#", tile.GetTransformedEdge(Edge.Right, false, true, true));

			AssertEqual("...#.##..#", tile.GetTransformedEdge(Edge.Top, true, true, true));
			AssertEqual("...#....#.", tile.GetTransformedEdge(Edge.Bottom, true, true, true));
			AssertEqual("#.#.#####.", tile.GetTransformedEdge(Edge.Right, true, true, true));
			AssertEqual("..#.###...", tile.GetTransformedEdge(Edge.Left, true, true, true));
		}
	}

}