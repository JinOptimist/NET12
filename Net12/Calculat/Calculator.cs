using System;
using System.Text.RegularExpressions;

namespace Calculat
{
    public class Calculator
    {
        readonly char[] op = new char[] { '(', ')', '*', '/', '+', '-', };
        
        public string Calculate(string equation)
        {
            var eq = equation;
            while (equation.Contains('+'))
            {
                if(equation.Contains('('))
                {
                    var first = equation.IndexOf('(')+1;
                    var last = equation.IndexOf(')');
                    eq = equation[first..last];
                    equation = equation[..first] + Calculate(eq) + equation[last..];
                }

                if (equation.Contains("*"))
                {
                    
                    
                    continue;
                }
                if (equation.Contains("/"))
                {
                   
                    continue;
                }
                if (equation.Contains('+'))
                {
                    var regrex = new Regex(@"\d{1,}[+-]\d{1,}");
                    var match = regrex.Match(equation);
                    

               
     

                    continue;
                }
                if (equation.Contains('-'))
                {
                   
                    continue;
                }

            }
            Console.WriteLine(equation);
            return equation;
        }
     
      
        private string mult(string eq)
        {
            var split = eq.Split('+');
            var res = Convert.ToInt32(split[0]) + Convert.ToInt32(split[1]);
            return res.ToString();

        }
    }
    
}
