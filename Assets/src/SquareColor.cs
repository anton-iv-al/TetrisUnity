using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TetrisGame
{
    public enum SquareColor
    {
        Empty = 0,
        Red = 1,
        Blue = 2,
        Green = 3,
        Yellow = 4,
        Orange = 5,
        Violet = 6,
        LightBlue = 7
    }

    static class SquareColorExtensions
    {
        public delegate void OverElemetAndIndex(int i, int j, SquareColor element);
        public delegate SquareColor ReturnElemetAndIndex(int i, int j, SquareColor element);
        public delegate bool CheckElemetAndIndex(int i, int j, SquareColor element);

        public static void ForEach(this SquareColor[,] matrix, OverElemetAndIndex callback)
        {
            for (int i = 0; i < matrix.GetLength(0); ++i)
            {
                for (int j = 0; j < matrix.GetLength(1); ++j)
                {
                    callback(i, j, matrix[i, j]);
                }
            }
        }

        public static SquareColor[,] Select(this SquareColor[,] matrix, ReturnElemetAndIndex callback)
        {
            var newMatrix = new SquareColor[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); ++i)
            {
                for (int j = 0; j < matrix.GetLength(1); ++j)
                {
                    newMatrix[i, j] = callback(i, j, matrix[i, j]);
                }
            }
            return newMatrix;
        }

        public static bool Any(this SquareColor[,] matrix, CheckElemetAndIndex callback)
        {
            var newMatrix = new SquareColor[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); ++i)
            {
                for (int j = 0; j < matrix.GetLength(1); ++j)
                {
                    if(callback(i, j, matrix[i, j])) return true;
                }
            }
            return false;
        }
    }
}
