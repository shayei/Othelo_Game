using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Windows;
using Othelo_Logic;

namespace Windows_UI
{
    public class StartForm : Form
    {
        private Button m_BoardSize;
        private Button m_PlayAgainstComputer;
        private Button m_PlayAgainstFriend;
        private int m_CurrentGameSize = 6;

        public StartForm()
        {
            initializeComponent();
        }

        private void initializeComponent()
        {
            m_BoardSize = new Button();
            m_PlayAgainstComputer = new Button();
            m_PlayAgainstFriend = new Button();
            Text = "Othello - Game Settings";

            m_BoardSize.Text = string.Format("Board size: {0}x{0} (click to increase)", m_CurrentGameSize);
            m_PlayAgainstComputer.Text = "Play against the computer";
            m_PlayAgainstFriend.Text = "Play against your friend";

            m_BoardSize.Location = new Point(12, 12);
            m_BoardSize.Size = new Size(273, 41);
            m_PlayAgainstComputer.Location = new Point(12, 70);
            m_PlayAgainstComputer.Size = new Size(135, 36);
            m_PlayAgainstFriend.Location = new Point(153, 70);
            m_PlayAgainstFriend.Size = new Size(132, 36);

            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(309, 128);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;

            m_BoardSize.Click += new EventHandler(BoardSizeButtonWasClick);
            m_PlayAgainstComputer.Click += new EventHandler(GameButtonWasClicked);
            m_PlayAgainstFriend.Click += new EventHandler(GameButtonWasClicked);

            Controls.Add(m_BoardSize);
            Controls.Add(m_PlayAgainstComputer);
            Controls.Add(m_PlayAgainstFriend);
            CenterToScreen();
        }

        private void GameButtonWasClicked(object sender, EventArgs e)
        {
            Dispose();
            GameForm gameForm = new GameForm(m_CurrentGameSize, sender == m_PlayAgainstComputer);
            gameForm.ShowDialog();
        }

        private void BoardSizeButtonWasClick(object sender, EventArgs e)
        {
            if (m_CurrentGameSize == 12)
            {
                m_CurrentGameSize = 6;
                m_BoardSize.Text = string.Format("Board size: {0}x{0} (click to increase)", m_CurrentGameSize);
            }
            else
            {
                m_CurrentGameSize += 2;
                m_BoardSize.Text = string.Format("Board size: {0}x{0} (click to increase)", m_CurrentGameSize);
            }
        }

        public static void ResetGame(GameForm i_OldGameForm)
        {
            i_OldGameForm.Dispose();
            GameForm gameForm = new GameForm(i_OldGameForm.SizeOfBoard, i_OldGameForm.IsAgainstComputer, i_OldGameForm.YellowWins, i_OldGameForm.RedWins);
            gameForm.ShowDialog();
            Application.Exit();
        }
    }
}
