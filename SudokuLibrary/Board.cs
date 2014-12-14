using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuLibrary
{
    public class Board
    {
        public event EventHandler<CheckingRowEventArgs> CheckingRowHandler;
        public event EventHandler<CheckingColEventArgs> CheckingColHandler;
        public event EventHandler<CheckingSubBoardEventArgs> CheckingSubBoardHandler;
        public event EventHandler<FoundNewNumberEventArgs> FoundNewNumberHandler;
        public event EventHandler<CheckingCellEventArgs> CheckingCellHandler;
        public event EventHandler<RemovedPossibilityEventArgs> RemovedPossibilityHandler;

        private int m_width = 9;
        private int m_height = 9;

        private List<int>[,] m_possibles;

        public Board(string path)
        {
            m_possibles = new List<int>[m_width, m_height];
            string[] lines = File.ReadAllLines(path);
            for (int x = 0; x < m_width; x++)
            {
                for (int y = 0; y < m_height; y++)
                {
                    m_possibles[x, y] = new List<int>();
                    int val = int.Parse(lines[y][x].ToString());
                    if (val != 0)
                    {
                        m_possibles[x, y].Add(val);
                    }
                    else
                    {
                        for (int i = 1; i <= 9; i++)
                        {
                            m_possibles[x, y].Add(i);
                        }
                    }
                }
            }
        }

        public int Solve()
        {
            int changes = 0;
            do
            {
                changes += process();
            } while (!Completed);

            return changes;
        }

        private int process()
        {
            int changes = 0;
            for (int x = 0; x < m_width; x++)
            {
                for (int y = 0; y < m_height; y++)
                {
                    if (m_possibles[x, y].Count == 1)
                    {
                        changes += removeNumberFromBoard(x, y, m_possibles[x, y][0]);
                    }
                    else
                    {
                        foreach(int number in m_possibles[x, y])
                        {
                            changes += checkIfLastNumberInBoard(x, y, number);
                        }
                    }
                }
            }

            return changes;
        }

        /// <summary>
        /// remove possibilities of number from board
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        private int removeNumberFromBoard(int x, int y, int number)
        {
            int changes = 0;

            changes += removeNumberFromRow(y, x, number);
            changes += removeNumberFromCol(x, y, number);
            changes += removeNumberFromSubBoard(x, y, number);

            return changes;
        }

        /// <summary>
        /// remove possibilities of number from row
        /// </summary>
        /// <param name="row"></param>
        /// <param name="ignoreCol"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        private int removeNumberFromRow(int row, int ignoreCol, int number)
        {
            onCheckingRow(row, number);
            int changes = 0;
            for (int x = 0; x < 9; x++)
            {
                onCheckingCell(x, row, number);
                if (x != ignoreCol && m_possibles[x, row].Remove(number))
                {
                    onRemovedPossibility(x, row, number);
                    if (m_possibles[x, row].Count == 1)
                    {
                        onFoundNewNumber(x, row, m_possibles[x, row][0]);
                        changes += removeNumberFromBoard(x, row, m_possibles[x, row][0]);
                    }

                    changes++;
                }
            }

            return changes;
        }

        /// <summary>
        /// remove possibilities of number from col
        /// </summary>
        /// <param name="col"></param>
        /// <param name="ignoreRow"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        private int removeNumberFromCol(int col, int ignoreRow, int number)
        {
            onCheckingCol(col, number);
            int changes = 0;
            for (int y = 0; y < 9; y++)
            {
                onCheckingCell(col, y, number);
                if (y != ignoreRow && m_possibles[col, y].Remove(number))
                {
                    onRemovedPossibility(col, y, number);
                    if (m_possibles[col, y].Count == 1)
                    {
                        onFoundNewNumber(col, y, m_possibles[col, y][0]);
                        changes += removeNumberFromBoard(col, y, m_possibles[col, y][0]);
                    }

                    changes++;
                }
            }

            return changes;
        }

        /// <summary>
        /// remove possibilities of number from sub board
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        private int removeNumberFromSubBoard(int x, int y, int number)
        {
            int subBoardX = x / 3;
            int subBoardY = y / 3;
            int subBoardRealX = subBoardX * 3;
            int subBoardRealY = subBoardY * 3;
            onCheckingSubBoard(subBoardX, subBoardY, number);
            int changes = 0;
            for (int subY = subBoardRealY; subY < subBoardRealY + 3; subY++)
            {
                for (int subX = subBoardRealX; subX < subBoardRealX + 3; subX++)
                {
                    onCheckingCell(subX, subY, number);
                    if ((subX != x || subY != y) && m_possibles[subX, subY].Remove(number))
                    {
                        onRemovedPossibility(subX, subY, number);
                        if (m_possibles[subX, subY].Count == 1)
                        {
                            onFoundNewNumber(subX, subY, m_possibles[subX, subY][0]);
                            changes += removeNumberFromBoard(subX, subY, m_possibles[subX, subY][0]);
                        }

                        changes++;
                    }
                }
            }

            return changes;
        }

        /// <summary>
        /// check if last possibility of number in board
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        private int checkIfLastNumberInBoard(int x, int y, int number)
        {
            int changes = 0;

            changes += checkIfLastNumberInRow(y, x, number);
            changes += checkIfLastNumberInCol(x, y, number);
            //changes += checkIfLastNumberInSubBoard(x, y, number);

            return changes;
        }

        /// <summary>
        /// check if last possibility of number in row
        /// </summary>
        /// <param name="row"></param>
        /// <param name="ignoreCol"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        private int checkIfLastNumberInRow(int row, int ignoreCol, int number)
        {
            int numberOfNs = 0;
            for (int x = 0; x < 9; x++)
            {
                if (x != ignoreCol && m_possibles[x, row].Contains(number))
                {
                    numberOfNs++;
                }
            }

            if (numberOfNs == 0)
            {
                m_possibles[ignoreCol, row] = new List<int> { number };
                onFoundNewNumber(ignoreCol, row, m_possibles[ignoreCol, row][0]);
                return 1 + removeNumberFromBoard(ignoreCol, row, m_possibles[ignoreCol, row][0]);
            }

            return 0;
        }

        /// <summary>
        /// check if last possibility of number in col
        /// </summary>
        /// <param name="col"></param>
        /// <param name="ignoreRow"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        private int checkIfLastNumberInCol(int col, int ignoreRow, int number)
        {
            int numberOfNs = 0;
            for (int y = 0; y < 9; y++)
            {
                if (y != ignoreRow && m_possibles[col, y].Contains(number))
                {
                    numberOfNs++;
                }
            }

            if (numberOfNs == 0)
            {
                m_possibles[col, ignoreRow] = new List<int> { number };
                onFoundNewNumber(col, ignoreRow, m_possibles[col, ignoreRow][0]);
                return 1 + removeNumberFromBoard(col, ignoreRow, m_possibles[col, ignoreRow][0]);
            }

            return 0;
        }

        public bool Completed
        {
            get
            {
                foreach (List<int> possibilities in m_possibles)
                {
                    if (possibilities.Count > 1)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            for (int y = 0; y < m_height; y++)
            {
                for (int x = 0; x < m_width; x++)
                {
                    if (m_possibles[x, y].Count == 1)
                    {
                        builder.Append(m_possibles[x, y][0]);
                    }
                    else
                    {
                        builder.Append(" ");
                    }

                    if (x != m_width - 1)
                    {
                        builder.Append("|");
                    }
                }

                builder.Append("\r\n");
            }

            return builder.ToString();
        }

        public string ToDetailedString()
        {
            StringBuilder builder = new StringBuilder();
            for (int y = 0; y < m_height; y++)
            {
                for (int x = 0; x < m_width; x++)
                {
                    for (int i = 1; i < 10; i++)
                    {
                        if (m_possibles[x, y].Contains(i))
                        {
                            builder.Append(i);
                        }
                        else
                        {
                            builder.Append(" ");
                        }
                    }

                    if (x != m_width - 1)
                    {
                        builder.Append("|");
                    }
                }

                builder.Append("\r\n");
            }

            return builder.ToString();
        }

        private void onCheckingRow(int row, int number)
        {
            if (CheckingRowHandler != null)
            {
                CheckingRowHandler(this, new CheckingRowEventArgs(row, number));
            }
        }

        private void onCheckingCol(int col, int number)
        {
            if (CheckingColHandler != null)
            {
                CheckingColHandler(this, new CheckingColEventArgs(col, number));
            }
        }

        private void onCheckingSubBoard(int subBoardX, int subBoardY, int number)
        {
            if (CheckingSubBoardHandler != null)
            {
                CheckingSubBoardHandler(this, new CheckingSubBoardEventArgs(subBoardX, subBoardY, number));
            }
        }

        private void onFoundNewNumber(int x, int y, int number)
        {
            if (FoundNewNumberHandler != null)
            {
                FoundNewNumberHandler(this, new FoundNewNumberEventArgs(x, y, number));
            }
        }

        private void onCheckingCell(int x, int y, int number)
        {
            if (CheckingCellHandler != null)
            {
                CheckingCellHandler(this, new CheckingCellEventArgs(x, y, number));
            }
        }

        private void onRemovedPossibility(int x, int y, int number)
        {
            if (RemovedPossibilityHandler != null)
            {
                RemovedPossibilityHandler(this, new RemovedPossibilityEventArgs(x, y, number));
            }
        }
    }
}
