using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Othelo_Logic;

namespace Windows_UI
{
    public class ButtonHandler : Form
    {
        private PictureBox m_PictureBox = new PictureBox();

        private BoardMatrix m_BoardMatrix;
        private GameForm m_Game;
        private eCellValue m_eCellValue = eCellValue.empty;
        public const int k_NoPossibleMoves = 0;

        public ButtonHandler(BoardMatrix i_BoardMatrix)
        {
            m_BoardMatrix = i_BoardMatrix;
            m_Game = m_BoardMatrix.GetGameForm;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            m_PictureBox.ForeColor = SystemColors.ButtonHighlight;
            m_PictureBox.Name = "m_PictureBox";
            m_PictureBox.Size = new Size(40, 40);
            m_PictureBox.TabIndex = 0;
            m_PictureBox.Text = string.Empty;
            m_PictureBox.Enabled = false;
            m_PictureBox.Click += new EventHandler(Button_Click);
            m_PictureBox.BorderStyle = BorderStyle.Fixed3D;
        }

        public PictureBox GetButton
        {
            get { return m_PictureBox; }
        }

        public void ButtonCanBePressed()
        {
            m_PictureBox.BackColor = Color.LawnGreen;
            m_PictureBox.Enabled = true;
        }

        public void ButtonCanNotBePressed()
        {
            m_PictureBox.BackColor = Color.Empty;
            m_PictureBox.Enabled = false;
        }

        public void CellOfRedPlayer()
        {
            m_PictureBox.Image = Properties.Resources.CoinRed;
            m_PictureBox.BackColor = Color.Snow;
            m_eCellValue = eCellValue.red;
            m_PictureBox.Enabled = false;
            m_PictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        public void CellOfYellowPlayer()
        {
            m_PictureBox.Image = Properties.Resources.CoinYellow;
            m_PictureBox.BackColor = Color.Black;
            m_eCellValue = eCellValue.yellow;
            m_PictureBox.Enabled = false;
            m_PictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        public eCellValue CellValue
        {
            get { return m_eCellValue; }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            int row = 0, col = 0;
            m_BoardMatrix.FindButton(ref row, ref col, this.GetButton);
            m_Game.CheckIfGameOver();

            if (m_Game.GetTitle() == "Otello - Yellow's turn")
            {
                if (m_Game.CheckIfAvailableMoves())
                {
                    m_Game.m_GameLogic.GameRunning(row, col, eCellValue.yellow);
                }
            }
            else
            {
                if (m_Game.CheckIfAvailableMoves())
                {
                    m_Game.m_GameLogic.GameRunning(row, col, eCellValue.red);
                }
            }

            m_Game.HidePreviousValidMoves();
            m_Game.UpdateCurrentBoard();
            m_Game.ChangeTitle();
            m_Game.UserPossibleMove = m_Game.ShowUserLegalMoves();
            m_Game.CheckIfGameOver();

            if (m_Game.IsAgainstComputer)
            {
                row = col = -1;
                m_Game.m_GameLogic.GameRunning(row, col, eCellValue.red);
                m_Game.HidePreviousValidMoves();
                m_Game.UpdateCurrentBoard();
                m_Game.ChangeTitle();
            }

            if (!m_Game.CheckIfAvailableMoves())
            {
                m_Game.ChangeTitle();
                m_Game.UserPossibleMove = m_Game.ShowUserLegalMoves();
                m_Game.Refresh();
            }

            m_Game.CheckIfGameOver();
            m_Game.UserPossibleMove = m_Game.ShowUserLegalMoves();
        }

        public void SetLocation(int i_X, int i_Y)
        {
            m_PictureBox.Location = new Point(i_X, i_Y);
        }
    }
}
