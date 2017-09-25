using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TetrisGame
{
    public class Field
    {
        private const int _matrixSizeX = 10;
        private const int _matrixSizeY = 24;    // первые 4 выше границы
        private SquareColor[,] _matrix = new SquareColor[_matrixSizeX, _matrixSizeY];

        public Field()
        {
            DrawManager.Instance.MatrixSizeX = _matrixSizeX;
            DrawManager.Instance.MatrixSizeY = _matrixSizeY;
        }

        public void Redraw()
        {
            DrawManager.Instance.ClearField();
            _matrix.ForEach((i,j,c) => DrawManager.Instance.CreateFieldSquare(i, j, c));
        }

        public bool CheckFigureIntersection(Figure figure)
        {
            bool isIntersect = figure.Matrix.Any((i, j, c) =>
            {
                if (c > 0)
                {
                    bool isLowerThanField = figure.Y + j >= _matrixSizeY;
                    bool isLefterThanField = figure.X + i < 0;
                    bool isRighterThanField = figure.X + i >= _matrixSizeX;
                    bool isHigherThanField = figure.Y + j < 0;
                    bool isInField = !isLowerThanField && !isHigherThanField && !isLefterThanField && !isRighterThanField;
                    bool isSquareIntersects = false;
                    if (isInField) isSquareIntersects = _matrix[figure.X + i, figure.Y + j] > 0;
                    if (isLowerThanField || isLefterThanField || isRighterThanField || isSquareIntersects)
                    {
                        return true;
                    }
                }
                return false;
            });
            return !isIntersect;
        }

        public void AddFigure(Figure figure)
        {
            figure.Matrix.ForEach((i, j, c) =>
            {
                if (c > 0) _matrix[figure.X + i, figure.Y + j] = c;
            });
        }

        public int RemoveLines()
        {
            int linesCount = 0;
            for (int j = 0; j < _matrixSizeY; ++j)
            {
                bool isFullLine = true;
                for (int i = 0; i < _matrixSizeX; ++i)
                {
                    if (_matrix[i,j] <= 0)
                    {
                        isFullLine = false;
                        break;
                    }
                }
                if(isFullLine)
                {
                    RemoveLine(j);
                    linesCount++;
                }
            }
            return linesCount;
        }

        private void RemoveLine(int y)
        {
            for (int j = y - 1; j >= 0; --j)
            {
                for (int i = 0; i < _matrixSizeX; ++i)
                {
                    _matrix[i, j + 1] = _matrix[i, j];
                }
            }
        }

        public bool CheckGamoOver()
        {
            for (int i = 0; i < _matrixSizeX; ++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    if (_matrix[i,j] > 0)
                          return true;
                }
            }
            return false;
        }

        public void Clear()
        {
            _matrix = new SquareColor[_matrixSizeX, _matrixSizeY];
        }
    }
}
