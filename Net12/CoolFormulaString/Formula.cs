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
            formula = String.Concat(formula.Split(" "));

            var normalSymbol = new Regex(@"[^0-9\+\-\*\/()^]").Match(formula);
            if (!normalSymbol.Success)
            {
                var bra = new Regex(@"\([^()]+\)");
                var match = bra.Match(formula);
                while (match.Success)
                {
                    var phrase = match.Value.Substring(1, match.Value.Length - 2);
                    var answer = CalcWithoutBreakets(phrase);
                    formula = formula.Replace(match.Value, answer);
                    match = bra.Match(formula);
                }

                formula = CalcWithoutBreakets(formula);

                return formula;
            }
            else return "Недопустимый символ";
        }

        private string CalcWithoutBreakets(string formula)
        {
            formula = CalcComplex(formula, @"\d+\^\d+");
            formula = CalcComplex(formula, @"\d{1,}[\\*/]\d{1,}");
            formula = CalcComplex(formula, @"-{0,1}\d{1,}[+-]\d{1,}");
            return formula;
        }

        private string CalcComplex(string formula, string regexPattern)
        {
            var regex = new Regex(regexPattern);
            var match = regex.Match(formula);
            while (match.Success)
            {
                var answer = CalcBasicPhares(match.Value);
                formula = formula.Replace(match.Value, answer);
                match = regex.Match(formula);
            }
            return formula;
        }

        private string CalcBasicPhares(string formula)
        {
            var operations = new char[] { '+', '*', '-', '/', '^' };
            var operationIndex = formula.IndexOfAny(operations, 1);
            var operationSymbol = formula[operationIndex];

            var isNegative = false;
            if (formula[0] == '-')
            {
                formula = formula.Substring(1);
                isNegative = true;
            }

            var numbers = formula
                .Split(operations)
                .Select(str => int.Parse(str))
                .ToList();
            numbers[0] *= isNegative ? -1 : 1;

            switch (operationSymbol)
            {
                case '+':
                    return (numbers[0] + numbers[1]).ToString();
                case '-':
                    return (numbers[0] - numbers[1]).ToString();
                case '*':
                    return (numbers[0] * numbers[1]).ToString();
                case '/':
                    if (numbers[1] == 0)
                    {
                        return "Деление на 0";
                    }
                    return (numbers[0] / numbers[1]).ToString();
                case '^':
                    return Math.Pow(numbers[0], numbers[1]).ToString();
            }

            return "Error";
        }
    }
}
