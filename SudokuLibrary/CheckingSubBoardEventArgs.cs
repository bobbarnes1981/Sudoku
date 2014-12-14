using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuLibrary
{
    public class CheckingSubBoardEventArgs : EventArgs
    {
        public CheckingSubBoardEventArgs(int subBoardX, int subBoardY, int number)
        {
            SubBoardX = subBoardX;
            SubBoardY = subBoardY;
            Number = number;
        }

        public int SubBoardX { get; private set; }

        public int SubBoardY { get; private set; }

        public int Number { get; private set; }
    }
}
