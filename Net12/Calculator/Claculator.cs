using System;

namespace Calculator
{
    public class Calculator
    {
        readonly char[] op = new char[] { '(', ')', '*', '/', '+', '-', };

        public void Calculate(string equation)
        {
            int k = equation.IndexOf("(");

            Console.WriteLine(k);

        }
    }
}
