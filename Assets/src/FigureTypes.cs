using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TetrisGame
{
    public class FigureTypes
    {
        private List<SquareColor[,]> _types = new List<SquareColor[,]>();

        Random _random = new Random();

        private void ReadTypes()
        {
            string[] lines = File.ReadAllLines("Figures.txt");

            int j = 0;
            SquareColor[,] matrix = new SquareColor[4, 4]; ;
            foreach (var line in lines)
            {
                string[] elements = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (elements.Length == 0) continue;
                if (elements.Length != 4) throw new Exception("Wrong format of Figures.txt");


                for(int i=0; i< elements.Length; ++i)
                {
                    int color = 0;
                    if (!int.TryParse(elements[i], out color)) throw new Exception("Wrong format of Figures.txt");
                    matrix[i, j] = (SquareColor)color;
                }

                j++;
                if (j > 3)
                {
                    _types.Add(matrix);
                    j = 0;
                    matrix = new SquareColor[4, 4];
                }
            }

            if(j != 0) throw new Exception("Wrong format of Figures.txt");
            if (_types.Count == 0) throw new Exception("Wrong format of Figures.txt");
        }

        public FigureTypes()
        {
            ReadTypes();
        }

        public SquareColor[,] GetRandomMatrix()
        {
            int number = _random.Next(0, _types.Count);
            return _types[number].Clone() as SquareColor[,];
        }
    }
}
