using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using TetrisGame;

namespace TetrisGame
{
    public class Figure
    {
        public SquareColor[,] Matrix;

        public int X { get; set; } = 3;
        public int Y { get; set; } = 0;

        public Figure(SquareColor[,] matrix)
        {
            Matrix = matrix;
        }

        public void Redraw()
        {
            DrawManager.Instance.ClearFifure();
            Matrix.ForEach((i, j, c) => DrawManager.Instance.CreateFigureSquare(X + i, Y + j, c));
        }

        public void RedrawInPreview()
        {
            DrawManager.Instance.ClearPreview();
            Matrix.ForEach((i, j, c) => DrawManager.Instance.CreatePreviewSquare(i, j, c));
        }

        public void RotateRight()
        {
            Matrix = Matrix.Select((i, j, c) => Matrix[3 - j, i]);
        }

        public void RotateLeft()
        {
            Matrix = Matrix.Select((i, j, c) => Matrix[j, 3 - i]);
        }
}
}
