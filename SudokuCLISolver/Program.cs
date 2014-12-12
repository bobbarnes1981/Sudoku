using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuLibrary;

namespace SudokuCLISolver
{
    class Program
    {
        static void Main(string[] args)
        {
            Solver s = new Solver();
            s.Solve();
            Console.WriteLine(s.ToString());
        }
    }
}
