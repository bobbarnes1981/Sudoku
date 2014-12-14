using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuCLISolver
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                new Solver(args[0]).Solve();
            }
            else
            {
                Console.WriteLine("Usage: SudokuCLISolver <filename>");
            }
        }
    }
}
