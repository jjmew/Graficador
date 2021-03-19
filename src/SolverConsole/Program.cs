using System;

namespace SolverConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            MathLibrary.PostFixExpression a;

            string infix = "(x ^ 2) + 1 + (5/2)*";

            a = new MathLibrary.PostFixExpression();
            a.TryParsePostFixExpression(infix);
            double result=0;
            a.TryToEvaluateForVariable("X", 1, ref result);
            Console.WriteLine("result: ", result);
        }
    }
}
