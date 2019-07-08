using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Othelo_Logic;

namespace Windows_UI
{
    public class GameForm : Form
    {
        public readonly bool r_AgainstComputer;
        private readonly int r_SizeOfBoard;
        private int m_YellowPlayerWins;
        private int m_RedPlayerWins;
        private BoardMatrix m_BoardMatrix;
        public Game m_GameLogic;
        private List<CellMatrix> m_UserPossibleMove;

        public GameForm(int i_BoardSize, bool i_AgainstComputer)
        {
            r_SizeOfBoard = i_BoardSize;
            r_AgainstComputer = i_AgainstComputer;
            initializeComponent(r_SizeOfBoard, r_AgainstComputer);
        }

        public GameForm(int i_BoardSize, bool i_AgainstComputer, int i_YellowPlayerWins, int i_RedPlayerWins) : this(i_BoardSize, i_AgainstComputer)
        {
            m_YellowPlayerWins = i_YellowPlayerWins;
            m_RedPlayerWins = i_RedPlayerWins;
        }

        private void initializeComponent(int i_BoardSize, bool i_AgainstComputer)
        {
            SuspendLayout();
            ForeColor = SystemColors.ButtonHighlight;
            Name = "Game";
            Text = "Otello - Yellow's turn";
            m_BoardMatrix = new BoardMatrix(i_BoardSize, this);
            for (int i = 0; i < r_SizeOfBoard; i++)
            {
                for (int j = 0; j < r_SizeOfBoard; j++)
                {
                    this.Controls.Add(m_BoardMatrix.Get_Button(i, j));
                }
            }

            CenterToScreen();
            m_GameLogic = new Game(i_BoardSize, i_AgainstComputer);
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size((i_BoardSize * 40) + 40, (i_BoardSize * 40) + 40);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            m_UserPossibleMove = ShowUserLegalMoves();
        }

        public void CheckIfGameOver()
        {
            m_GameLogic.ScanAndUpdatePossiblMovesList(eCellValue.yellow, ref m_GameLogic.m_User1PossibleMove);
            if (m_GameLogic.m_User1PossibleMove.Count == 0)
            {
                m_GameLogic.ScanAndUpdatePossiblMovesList(eCellValue.red, ref m_GameLogic.m_User2PossibleMove);
                if (m_GameLogic.m_User2PossibleMove.Count == 0)
                {
                    FinishedGameMessage();
                }
            }
        }

        public bool CheckIfAvailableMoves()
        {
            bool check = false;
            if (this.Text == "Otello - Yellow's turn")
            {
                m_GameLogic.ScanAndUpdatePossiblMovesList(eCellValue.yellow, ref m_GameLogic.m_User1PossibleMove);
                check = m_GameLogic.m_User1PossibleMove.Count > 0;
            }
            else
            {
                m_GameLogic.ScanAndUpdatePossiblMovesList(eCellValue.red, ref m_GameLogic.m_User2PossibleMove);
                check = m_GameLogic.m_User2PossibleMove.Count > 0;
            }

            return check;
        }

        public List<CellMatrix> UserPossibleMove
        {
            get { return m_UserPossibleMove; }
            set { m_UserPossibleMove = value; }
        }

        private void FinishedGameMessage()
        {
            DialogResult result = MessageBox.Show(GetFinishedMessage(), "Othello Game",
                  MessageBoxButtons.YesNo, MessageBoxIcon.Information,
                  MessageBoxDefaultButton.Button1);

            if (result == DialogResult.OK)
            {
                Application.Exit();
            }

            if (result == DialogResult.Yes)
            {
                StartForm.ResetGame(this);
            }
            else
            {
                Application.Exit();
            }
        }

        public bool IsAgainstComputer
        {
            get { return r_AgainstComputer; }
        }

        public int RedWins
        {
            get { return m_RedPlayerWins; }
        }

        public int YellowWins
        {
            get { return m_YellowPlayerWins; }
        }

        private string GetFinishedMessage()
        {
            string msg;
            bool winner = true;
            bool loser = false;
            if (m_GameLogic.FindWinnerOrLoser(winner) == null)
            {
                msg =
@"It's a tie
Would you like to play another round?";
            }
            else
            {
                if (this.m_GameLogic.FindWinnerOrLoser(winner).Color == eCellValue.yellow)
                {
                    this.m_YellowPlayerWins++;
                }
                else
                {
                    this.m_RedPlayerWins++;
                }

                msg = string.Format(
@"{0} won!! ({1}/{2}) ({3}/{4})
Would you like another round?",
                    this.m_GameLogic.FindWinnerOrLoser(winner).Color == eCellValue.yellow ? "Yellow" : "Red",
                    this.m_GameLogic.FindWinnerOrLoser(winner).Points,
                    this.m_GameLogic.FindWinnerOrLoser(loser).Points,
                    this.m_GameLogic.FindWinnerOrLoser(winner).Color == eCellValue.yellow ? this.m_YellowPlayerWins : this.m_RedPlayerWins,
                    this.m_GameLogic.FindWinnerOrLoser(loser).Color == eCellValue.yellow ? this.m_YellowPlayerWins : this.m_RedPlayerWins);
            }

            return msg;
        }

        public int SizeOfBoard
        {
            get { return r_SizeOfBoard; }
        }

        public string GetTitle()
        {
            return Text;
        }

        public void ChangeTitle()
        {
            if (this.Text == "Otello - Yellow's turn")
            {
                Text = "Otello - Red's turn";
            }
            else
            {
                Text = "Otello - Yellow's turn";
            }
        }

        public List<CellMatrix> ShowUserLegalMoves()
        {
            int col = 0, row = 0;
            List<CellMatrix> m_UserPossibleMove = null;
            if (this.Text == "Otello - Yellow's turn")
            {
                m_GameLogic.ScanAndUpdatePossiblMovesList(eCellValue.yellow, ref m_GameLogic.m_User1PossibleMove);
                m_UserPossibleMove = m_GameLogic.m_User1PossibleMove;
            }
            else
            {
                m_GameLogic.ScanAndUpdatePossiblMovesList(eCellValue.red, ref m_GameLogic.m_User2PossibleMove);
                m_UserPossibleMove = m_GameLogic.m_User2PossibleMove;
            }

            for (int i = 0; i < m_UserPossibleMove.Count; i++)
            {
                col = m_UserPossibleMove[i].Col;
                row = m_UserPossibleMove[i].Row;
                m_BoardMatrix.GetButtonHandler(row, col).ButtonCanBePressed();
            }

            return m_UserPossibleMove;
        }

        public void HidePreviousValidMoves()
        {
            int col = 0, row = 0;
            for (int i = 0; i < m_UserPossibleMove.Count; i++)
            {
                col = m_UserPossibleMove[i].Col;
                row = m_UserPossibleMove[i].Row;
                m_BoardMatrix.GetButtonHandler(row, col).ButtonCanNotBePressed();
            }

            m_UserPossibleMove = null;
        }

        public void UpdateCurrentBoard()
        {
            for (int i = 0; i < r_SizeOfBoard; i++)
            {
                for (int j = 0; j < r_SizeOfBoard; j++)
                {
                    eCellValue requiredSign = m_GameLogic.m_Board.GetCellValue(i, j);
                    switch (requiredSign)
                    {
                        case eCellValue.empty:
                            m_BoardMatrix.GetButtonHandler(i, j).ButtonCanNotBePressed();
                            break;
                        case eCellValue.yellow:
                            m_BoardMatrix.GetButtonHandler(i, j).CellOfYellowPlayer();
                            break;
                        case eCellValue.red:
                            m_BoardMatrix.GetButtonHandler(i, j).CellOfRedPlayer();
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
