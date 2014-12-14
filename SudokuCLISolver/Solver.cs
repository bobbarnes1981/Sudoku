using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuLibrary;

namespace SudokuCLISolver
{
    class Solver
    {
        private Board m_board;

        public Solver(string path)
        {
            m_board = new Board(path);
        }

        public void Solve()
        {
            m_board.CheckingRowHandler += onCheckingRow;
            m_board.CheckingColHandler += onCheckingCol;
            m_board.CheckingSubBoardHandler += onCheckingSubBoard;
            m_board.FoundNewNumberHandler += onFoundNewNumber;
            m_board.RemovedPossibilityHandler += onRemovedPossibility;
            
            int changes = m_board.Solve();

            Console.WriteLine("Changes: {0}", changes);
            Console.WriteLine(m_board.ToString());
        }

        public void onCheckingRow(object sender, CheckingRowEventArgs e)
        {
            Console.WriteLine("Checking {0} in row {1}", e.Number, e.Row);
        }

        public void onCheckingCol(object sender, CheckingColEventArgs e)
        {
            Console.WriteLine("Checking {0} in col {1}", e.Number, e.Col);
        }

        public void onCheckingSubBoard(object sender, CheckingSubBoardEventArgs e)
        {
            Console.WriteLine("Checking {0} in sub board {1},{2}", e.Number, e.SubBoardX, e.SubBoardY);
        }

        public void onFoundNewNumber(object sender, FoundNewNumberEventArgs e)
        {
            Console.WriteLine("Found new number: {0} at {1},{2}", e.Number, e.X, e.Y);
        }

        public void onCheckingCell(object sender, CheckingCellEventArgs e)
        {
            Console.WriteLine("Checking cell {0},{1} to remove possibility of {2}", e.X, e.Y, e.Number);
        }

        public void onRemovedPossibility(object sender, RemovedPossibilityEventArgs e)
        {
            Console.WriteLine("Removed possibility of {0} at {1},{2}", e.Number, e.X, e.Y);
        }
    }
}
