using System;
using System.Collections.Generic;
using System.Text;

namespace Othelo_Logic
{
    public class Board
    {
        private eCellValue[,] r_BoardGame;
        private int r_Size;

        public Board(int size)
        {
            r_BoardGame = new eCellValue[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    r_BoardGame[i, j] = eCellValue.empty;
                }
            }

            r_Size = size;
            r_BoardGame[(size / 2) - 1, (size / 2) - 1] = r_BoardGame[size / 2, size / 2] = eCellValue.red;
            r_BoardGame[size / 2, (size / 2) - 1] = r_BoardGame[(size / 2) - 1, size / 2] = eCellValue.yellow;
        }

        public int Size
        {
            get { return r_Size; }
        }

        public eCellValue GetCellValue(int i_Row, int i_Col)
        {
            return r_BoardGame[i_Row, i_Col];
        }

        public void SetCellValue(eCellValue i_Symbol, int i_Row, int i_Col)
        {
            r_BoardGame[i_Row, i_Col] = i_Symbol;
        }
    }
}
