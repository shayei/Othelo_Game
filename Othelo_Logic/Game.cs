using System;
using System.Collections.Generic;
using System.Text;

namespace Othelo_Logic
{
    public class Game
    {
        private int r_GameMode;
        private readonly int r_SizeBoard;
        public Board m_Board;
        public Player m_User1;
        public Player m_User2;
        public List<CellMatrix> m_User1PossibleMove = new List<CellMatrix>();
        public List<CellMatrix> m_User2PossibleMove = new List<CellMatrix>();
        public const int k_GameAgainstComputer = 1;
        public const int k_GameAgainstFriend = 2;
        public const int k_NoPossibleMoves = 0;
         
        public Game(int i_BoardSize, bool i_AgainstComputer)
        {
            m_User1 = new Player(eCellValue.yellow);
            m_User2 = new Player(eCellValue.red);
            SetGameMode(i_AgainstComputer);
            r_SizeBoard = i_BoardSize;
            SetBoard(r_SizeBoard, ref m_Board);
        }

        public void GameRunning(int i_row, int i_col, eCellValue i_Color)
        {
            m_User1PossibleMove.Clear();
            m_User2PossibleMove.Clear();

            ScanAndUpdatePossiblMovesList(m_User1.Color, ref m_User1PossibleMove);
            ScanAndUpdatePossiblMovesList(m_User2.Color, ref m_User2PossibleMove);

            if (m_User1PossibleMove.Count != k_NoPossibleMoves || m_User2PossibleMove.Count != k_NoPossibleMoves)
            {
                if (m_User1PossibleMove.Count != k_NoPossibleMoves && m_User1.Color == i_Color)
                {
                    m_Board.SetCellValue(i_Color, i_row, i_col);
                    CheckAndUpdateBoard(i_row, i_col, i_Color, true);
                }

                m_User2PossibleMove.Clear();
                ScanAndUpdatePossiblMovesList(m_User2.Color, ref m_User2PossibleMove);

                if (m_User2PossibleMove.Count != k_NoPossibleMoves && m_User2.Color == i_Color)
                {
                    if (r_GameMode == k_GameAgainstComputer)
                    {
                        int randomCell = RandomMove(m_User2PossibleMove);
                        m_Board.SetCellValue(m_User2.Color, m_User2PossibleMove[randomCell].Row, m_User2PossibleMove[randomCell].Col);
                        CheckAndUpdateBoard(m_User2PossibleMove[randomCell].Row, m_User2PossibleMove[randomCell].Col, m_User2.Color, true);
                    }
                    else
                    {
                        m_Board.SetCellValue(i_Color, i_row, i_col);
                        CheckAndUpdateBoard(i_row, i_col, i_Color, true);
                    }
                }

                m_User1PossibleMove.Clear();
                ScanAndUpdatePossiblMovesList(m_User1.Color, ref m_User1PossibleMove);
                m_User2PossibleMove.Clear();
                ScanAndUpdatePossiblMovesList(m_User2.Color, ref m_User2PossibleMove);
            }
        }

        private void SetGameMode(bool i_AgainstComputer)
        {
            if (i_AgainstComputer)
            {
                r_GameMode = k_GameAgainstComputer;
            }
            else
            {
                r_GameMode = k_GameAgainstFriend;
            }
        }

        public Player FindWinnerOrLoser(bool isWantFindWinner)
        {
            m_User1.Points = CountPoints(m_Board, m_User1.Color);
            m_User2.Points = CountPoints(m_Board, m_User2.Color);
            Player player = null;

            if (m_User1.Points > m_User2.Points)
            {
                player = m_User1;
            }
            else if (m_User1.Points < m_User2.Points)
            {
                player = m_User2;
            }
            else
            {
                player = null;
            }
            
            if (!isWantFindWinner)
            {
                if (player == m_User1)
                {
                    player = m_User2;
                }
                else
                {
                    player = m_User1;
                }
            }

            return player;
        }

        private static void ProceedAccordingToDirection(ref int io_Row, ref int io_Col, eDirection i_Direction)
        {
            switch (i_Direction)
            {
                case eDirection.Down:
                    io_Row++;
                    break;
                case eDirection.Up:
                    io_Row--;
                    break;
                case eDirection.Left:
                    io_Col--;
                    break;
                case eDirection.Right:
                    io_Col++;
                    break;
                case eDirection.UpRight:
                    io_Col++;
                    io_Row--;
                    break;
                case eDirection.UpLeft:
                    io_Col--;
                    io_Row--;
                    break;
                case eDirection.DownRight:
                    io_Col++;
                    io_Row++;
                    break;
                case eDirection.DownLeft:
                    io_Col--;
                    io_Row++;
                    break;
                default:
                    break;
            }
        }

        private void SetBoard(int i_sizeBoard, ref Board io_Board)
        {
            io_Board = new Board(i_sizeBoard);
        }

        private bool CheckIfMoveIsInList(List<CellMatrix> i_ListOfOptionalCell, int i_Row, int i_Col)
        {
            bool result = false;
            for (int k = 0; k < i_ListOfOptionalCell.Count; k++)
            {
                if ((i_ListOfOptionalCell[k].Row == i_Row) && i_ListOfOptionalCell[k].Col == i_Col)
                {
                    result = true;
                }
            }

            return result;
        }

        private int RandomMove(List<CellMatrix> I_ListOfMoves)
        {
            int size = I_ListOfMoves.Count;
            Random random = new Random();
            int randomNumber = random.Next(0, size);
            return randomNumber;
        }

        public void ScanAndUpdatePossiblMovesList(eCellValue i_CellValue, ref List<CellMatrix> i_ListOfOptionalCell)
        {
            i_ListOfOptionalCell.Clear();
            bool isPossible = false;
            for (int i = 0; i < m_Board.Size; i++)
            {
                for (int j = 0; j < m_Board.Size; j++)
                {
                    if (m_Board.GetCellValue(i, j) == eCellValue.empty)
                    {
                        isPossible = CheckAndUpdateBoard(i, j, i_CellValue, false);
                        if (isPossible == true)
                        {
                            i_ListOfOptionalCell.Add(new CellMatrix(i, j));
                        }
                    }

                    isPossible = false;
                }
            }
        }

        private enum eDirection
        {
            Down, Up, Left, Right, UpRight, UpLeft, DownRight, DownLeft
        }

        private void UpdateTableByMove(eCellValue i_Symbol, eCellValue i_RivalSymbol, int i_Row, int i_Col, eDirection i_Direction)
        {
            bool stopUpdate = false;
            ProceedAccordingToDirection(ref i_Row, ref i_Col, i_Direction);

            while (!stopUpdate)
            {
                if (m_Board.GetCellValue(i_Row, i_Col) == i_Symbol)
                {
                    stopUpdate = true;
                }

                if (m_Board.GetCellValue(i_Row, i_Col) == i_RivalSymbol)
                {
                    m_Board.SetCellValue(i_Symbol, i_Row, i_Col);
                }

                if (m_Board.GetCellValue(i_Row, i_Col) == eCellValue.empty)
                {
                    break;
                }

                ProceedAccordingToDirection(ref i_Row, ref i_Col, i_Direction);
            }
        }

        private bool CheckAndUpdateBoard(int i_Row, int i_Col, eCellValue i_CellValue, bool i_UpdateBoard)
        {
            bool check = false;
            eCellValue rivalSymbol;
            bool wasRivalSymbol = false;

            if (i_CellValue == eCellValue.yellow)
            {
                rivalSymbol = eCellValue.red;
            }
            else
            {
                rivalSymbol = eCellValue.yellow;
            }

            int i = i_Row, j = i_Col;

            int countDirectionEnum = Enum.GetNames(typeof(eDirection)).Length;

            for (int k = 0; k < countDirectionEnum; k++)
            {
                for (int q = 0; q < m_Board.Size; q++)
                {
                    ProceedAccordingToDirection(ref i, ref j, (eDirection)k);

                    if (CheckIfExceedingTheBoundariesOfTheMatrix(i, j) == true)
                    {
                        goto NextDirecrtion;
                    }

                    if (m_Board.GetCellValue(i, j) == rivalSymbol)
                    {
                        wasRivalSymbol = true;
                    }

                    if (m_Board.GetCellValue(i, j) == i_CellValue)
                    {
                        if (wasRivalSymbol == true)
                        {
                            check = true;
                            if (i_UpdateBoard == true)
                            {
                                UpdateTableByMove(i_CellValue, rivalSymbol, i_Row, i_Col, (eDirection)k);
                                goto NextDirecrtion;
                            }
                            else
                            {
                                goto Finish;
                            }
                        }
                        else
                        {
                            goto NextDirecrtion;
                        }
                    }

                    if (m_Board.GetCellValue(i, j) == eCellValue.empty)
                    {
                        goto NextDirecrtion;
                    }
                }

            NextDirecrtion:
                i = i_Row;
                j = i_Col;
                wasRivalSymbol = false;
            }

        Finish:
            return check;
        }

        private bool CheckIfExceedingTheBoundariesOfTheMatrix(int i_Row, int i_Col)
        {
            return i_Row >= m_Board.Size || i_Row < 0 || i_Col >= m_Board.Size || i_Col < 0;
        }

        private int CountPoints(Board i_Board, eCellValue i_Symbol)
        {
            int points = 0;
            for (int i = 0; i < i_Board.Size; i++)
            {
                for (int j = 0; j < i_Board.Size; j++)
                {
                    if (i_Board.GetCellValue(i, j) == i_Symbol)
                    {
                        points++;
                    }
                }
            }

            return points;
        }
    }
}
