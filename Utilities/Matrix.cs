using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class Matrix<T>
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
       
        public Matrix(int width, int height, T[] array)
        {
            Width = width;
            Height = height;
            Array = array;
        }

        public Matrix(int width, int height) : this(width, height, new T[width * height])
        {            
        }

        public Matrix(Matrix<T> other)
        {
            Array = (T[])other.Array.Clone();
            Width = other.Width;
            Height = other.Height;
        }

        public int GetIndex(int x, int y)
        {
            return y * Width + x;
        }

        public Matrix<T> Clone()
        {
            return new Matrix<T>(this);
        }

        public void CopyFrom(Matrix<T> other)
        {
            other.Array.CopyTo(Array, 0);
        }

        public static Matrix<char> BuildCharMatrix(List<string> input)
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
            return new Matrix<char>(width, height, array);
        }

        
    }

    public static class MatrixExtensions
    {        
        public static void Dump(this Matrix<char> charMatrix)
        {
            Console.WriteLine("**************************************");
            for (int i = 0; i < charMatrix.Height; i++)
            {
                //FIXME: Use Span<char>
                var str = new string(charMatrix.Array, charMatrix.Width * i, charMatrix.Width);
                Console.WriteLine(str);
            }
        }
    }
}
