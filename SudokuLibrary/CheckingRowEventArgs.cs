using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuLibrary
{
    public class CheckingRowEventArgs : EventArgs
    {
        public CheckingRowEventArgs(int row, int number)
        {
            Row = row;
            Number = number;
        }

        public int Row { get; private set; }

        public int Number { get; private set; }
    }
}
