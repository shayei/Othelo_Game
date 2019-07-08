using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Othelo_Logic;

namespace Windows_UI
{
    public class BoardMatrix : Form
    {
        private readonly ButtonHandler[,] m_ButtonMatrix;
        private GameForm m_Game;

        public BoardMatrix(int SizeOfBoard, GameForm i_Game)
        {
            m_Game = i_Game;
            m_ButtonMatrix = new ButtonHandler[SizeOfBoard, SizeOfBoard];
            int celNr = 1;

            for (int i = 0; i < SizeOfBoard; i++)
            {
                for (int j = 0; j < SizeOfBoard; j++)
                {
                    m_ButtonMatrix[i, j] = new ButtonHandler(this);
                    m_ButtonMatrix[i, j].SetLocation((i * 40) + 15, (j * 40) + 15);
                    m_ButtonMatrix[i, j].Tag = celNr++;
                }
            }

            m_ButtonMatrix[(SizeOfBoard / 2) - 1, (SizeOfBoard / 2) - 1].CellOfRedPlayer();
            m_ButtonMatrix[SizeOfBoard / 2, SizeOfBoard / 2].CellOfRedPlayer();
            m_ButtonMatrix[SizeOfBoard / 2, (SizeOfBoard / 2) - 1].CellOfYellowPlayer();
            m_ButtonMatrix[(SizeOfBoard / 2) - 1, SizeOfBoard / 2].CellOfYellowPlayer();
        }

        public GameForm GetGameForm
        {
            get { return m_Game; }
        }

        public void FindButton(ref int io_row, ref int io_col, PictureBox i_Button)
        {
            for (int i = 0; i < m_Game.SizeOfBoard; i++)
            {
                for (int j = 0; j < m_Game.SizeOfBoard; j++)
                {
                    if (Get_Button(i, j) == i_Button)
                    {
                        io_row = i;
                        io_col = j;
                        break;
                    }
                }
            }
        }

        public PictureBox Get_Button(int i, int j)
        {
            return m_ButtonMatrix[i, j].GetButton;
        }

        public ButtonHandler GetButtonHandler(int i, int j)
        {
            return m_ButtonMatrix[i, j];
        }
    }
}
