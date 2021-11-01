using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CoolFormulaString
{
    public class Formula
    {
        public string Calc(string formula)
        {
            var regex = new Regex(@"\d{1,}[+-]\d{1,}");
            var match = regex.Match(formula);
            if (match.Success)
            {
                var answer = CalcBasicPhares(match.Value);
                formula = formula.Replace(match.Value, answer);
            }

            return formula;
        }

        private string CalcBasicPhares(string formula)
        {
            var operations = new char[] { '+', '*', '-', '/' };
            var operationIndex = formula.IndexOfAny(operations);
            var operationSymbol = formula[operationIndex];

            var numbers = formula
                .Split(operations)
                .Select(str => int.Parse(str))
                .ToList();

            switch (operationSymbol)
            {
                case '+':
                    return (numbers[0] + numbers[1]).ToString();
                case '-':
                    return (numbers[0] - numbers[1]).ToString();
                case '*':
                    return (numbers[0] * numbers[1]).ToString();
                case '/':
                    return (numbers[0] / numbers[1]).ToString();
            }

            return "Error";
        }
    }
}
