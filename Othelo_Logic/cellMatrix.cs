using System;
using System.Collections.Generic;
using System.Text;

namespace Othelo_Logic
{
    public class CellMatrix
    {
        private int m_Row;
        private int m_Col;

        public CellMatrix(int i_Row, int i_Col)
        {
            m_Row = i_Row;
            m_Col = i_Col;
        }

        public int Row
        {
            get { return m_Row; }
        }

        public int Col
        {
            get { return m_Col; }
        }
    }
}
