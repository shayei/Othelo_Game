using System;
using System.Collections.Generic;
using System.Text;

namespace Othelo_Logic
{
    public class Player
    {
        private readonly eCellValue m_Color;
        public int m_Points;

        public Player(eCellValue i_Color)
        {
            m_Color = i_Color;
            m_Points = 2;
        }

        public eCellValue Color
        {
            get { return m_Color; }
        }

        public int Points
        {
            get { return m_Points; }
            set { m_Points = value; }
        }
    }
}
