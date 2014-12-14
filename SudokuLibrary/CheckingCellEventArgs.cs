using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuLibrary
{
    public class CheckingCellEventArgs : EventArgs
    {
        public CheckingCellEventArgs(int x, int y, int number)
        {
            X = x;
            Y = y;
            Number = number;
        }

        public int X { get; private set; }

        public int Y { get; private set; }

        public int Number { get; private set; }
    }
}
