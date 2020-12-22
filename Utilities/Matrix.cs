using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class Matrix4D<T>
    {
        public readonly T[] Array;
        public readonly int Width;
        public readonly int Height;
        public readonly int Depth;
        public readonly int Wdim;

        public T this[int x, int y, int z, int w]
        {
            get
            {
                return Array[w * Width * Height * Depth + z * Width * Height + y * Width + x];
            }
            set
            {
                Array[w * Width * Height * Depth + z * Width * Height + y * Width + x] = value;
            }
        }

        public Matrix4D(int width, int height, int depth, int wdim, T[] array)
        {
            Width = width;
            Height = height;
            Depth = depth;
            Wdim = wdim;
            Array = array;
        }

        public Matrix4D(int width, int height, int depth, int wdim) : this(width, height, depth, wdim, new T[width * height * depth * wdim])
        {
        }

        public Matrix4D(Matrix4D<T> other)
        {
            Array = (T[])other.Array.Clone();
            Width = other.Width;
            Height = other.Height;
            Depth = other.Depth;
            Wdim = other.Wdim;
        }

        public int GetIndex(int x, int y, int z, int w)
        {
            return w * Width * Height * Depth + z * Width * Height + y * Width + x;
        }

        public Matrix4D<T> Clone()
        {
            return new Matrix4D<T>(this);
        }

        public void CopyFrom(Matrix4D<T> other)
        {
            other.Array.CopyTo(Array, 0);
        }

        static Matrix4D()
        {
            Neighborhood = new (int, int, int, int)[80];
            int i = 0;
            for (int w = -1; w <= 1; w++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        for (int x = -1; x <= 1; x++)
                        {
                            if (x == 0 && y == 0 && z == 0 && w == 0) continue;
                            Neighborhood[i++] = (x, y, z, w);
                        }
                    }
                }
            }
        }

        public static (int, int, int, int)[] Neighborhood;
    }


    public class Matrix3D<T>
    {
        public readonly T[] Array;
        public readonly int Width;
        public readonly int Height;
        public readonly int Depth;

        public T this[int x, int y, int z]
        {
            get
            {
                return Array[z * Width * Height + y * Width + x];
            }
            set
            {
                Array[z * Width * Height + y * Width + x] = value;
            }
        }

        public Matrix3D(int width, int height, int depth, T[] array)
        {
            Width = width;
            Height = height;
            Depth = depth;
            Array = array;
        }

        public Matrix3D(int width, int height, int depth) : this(width, height, depth, new T[width * height * depth])
        {
        }

        public Matrix3D(Matrix3D<T> other)
        {
            Array = (T[])other.Array.Clone();
            Width = other.Width;
            Height = other.Height;
            Depth = other.Depth;
        }

        public int GetIndex(int x, int y, int z)
        {
            return z * Width * Height + y * Width + x;
        }

        public Matrix3D<T> Clone()
        {
            return new Matrix3D<T>(this);
        }

        public void CopyFrom(Matrix3D<T> other)
        {
            other.Array.CopyTo(Array, 0);
        }

        public static (int, int, int)[] Neighborhood = new (int, int, int)[]
        {
            (-1, -1, -1),
            ( 0, -1, -1),
            ( 1, -1, -1),
            (-1,  0, -1),
            ( 0,  0, -1),
            ( 1,  0, -1),
            (-1,  1, -1),
            ( 0,  1, -1),
            ( 1,  1, -1),

            (-1, -1,  0),
            ( 0, -1,  0),
            ( 1, -1,  0),
            (-1,  0,  0),

            ( 1,  0,  0),
            (-1,  1,  0),
            ( 0,  1,  0),
            ( 1,  1,  0),

            (-1, -1,  1),
            ( 0, -1,  1),
            ( 1, -1,  1),
            (-1,  0,  1),
            ( 0,  0,  1),
            ( 1,  0,  1),
            (-1,  1,  1),
            ( 0,  1,  1),
            ( 1,  1,  1),
        };
    }

    public class Matrix2D<T>
    {
        public readonly T[] Array;
        public readonly int Width;
        public readonly int Height;

        public T this[int x, int y]
        {
            get
            {
                return Array[y * Width + x];
            }
            set
            {
                Array[y * Width + x] = value;
            }
        }

        public Matrix2D(int width, int height, T[] array)
        {
            Width = width;
            Height = height;
            Array = array;
        }

        public Matrix2D(int width, int height) : this(width, height, new T[width * height])
        {
        }

        public Matrix2D(Matrix2D<T> other)
        {
            Array = (T[])other.Array.Clone();
            Width = other.Width;
            Height = other.Height;
        }

        public int GetIndex(int x, int y)
        {
            return y * Width + x;
        }

        public Matrix2D<T> Clone()
        {
            return new Matrix2D<T>(this);
        }

        public void CopyFrom(Matrix2D<T> other)
        {
            other.Array.CopyTo(Array, 0);
        }

        public T[] ReadRow(int y)
        {
            int index = GetIndex(0, y);
            var arr = new T[Width];
            System.Array.Copy(Array, index, arr, 0, Width);
            return arr;
        }

        public T[] ReadColumn(int x)
        {
            int index = GetIndex(x, 0);
            var arr = new T[Height];
            for (int y = 0; y < Height; y++)
            {
                arr[y] = Array[index + y * Width];
            }
            return arr;
        }

    }

    public static class CharMatrix
    {
        public static Matrix2D<char> Build(List<string> input)
        {
            var width = input[0].Length;
            var height = input.Count;
            var array = new char[width * height];
            int lineptr = 0;
            foreach (var line in input)
            {
                int ptr = lineptr;
                foreach (var c in line)
                {
                    array[ptr++] = c;
                }
                lineptr += width;
            }
            return new Matrix2D<char>(width, height, array);
        }

        public static ConsoleColor[] DefaultColorMap()
        {
            return Enumerable.Repeat(ConsoleColor.Gray, 256).ToArray();
        }

        public static void DumpColor(this Matrix2D<char> charMatrix, ConsoleColor[] foregroundMap)
        {
            Console.WriteLine(new string('*', charMatrix.Width + 2));
            var defaultColor = Console.ForegroundColor;
            for (int i = 0; i < charMatrix.Height; i++)
            {
                Console.Write("*");
                var str = new string(charMatrix.Array, charMatrix.Width * i, charMatrix.Width);
                foreach (var c in str)
                {
                    Console.ForegroundColor = foregroundMap[c];
                    Console.Write(c);
                }
                Console.ForegroundColor = defaultColor;
                Console.WriteLine("*");
            }
            Console.WriteLine(new string('*', charMatrix.Width + 2));
        }


        public static void Dump(this Matrix2D<char> charMatrix)
        {
            Console.WriteLine(new string('*', charMatrix.Width + 2));
            for (int i = 0; i < charMatrix.Height; i++)
            {
                var str = new string(charMatrix.Array, charMatrix.Width * i, charMatrix.Width);
                Console.WriteLine("*" + str + "*");
            }
            Console.WriteLine(new string('*', charMatrix.Width + 2));
        }

        public static void DumpSlice(this Matrix3D<char> charMatrix, int z)
        {
            for (int i = 0; i < charMatrix.Height; i++)
            {
                var str = new string(charMatrix.Array, z * charMatrix.Height * charMatrix.Width + charMatrix.Width * i, charMatrix.Width);
                Console.WriteLine(str);
            }
        }

        public static void DumpSlice(this Matrix4D<char> charMatrix, int z, int w)
        {
            for (int i = 0; i < charMatrix.Height; i++)
            {
                var str = new string(charMatrix.Array, w * charMatrix.Depth * charMatrix.Height * charMatrix.Width + z * charMatrix.Height * charMatrix.Width + charMatrix.Width * i, charMatrix.Width);
                Console.WriteLine(str);
            }
        }

    }
}
