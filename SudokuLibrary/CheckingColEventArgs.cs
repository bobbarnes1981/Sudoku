using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuLibrary
{
    public class CheckingColEventArgs : EventArgs
    {
        public CheckingColEventArgs(int col, int number)
        {
            Col = col;
            Number = number;
        }

        public int Col { get; private set; }

        public int Number { get; private set; }
    }
}
