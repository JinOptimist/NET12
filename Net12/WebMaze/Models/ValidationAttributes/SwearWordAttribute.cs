using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebMaze.Models.ValidationAttributes
{
    public class SwearWordAttribute : ValidationAttribute
    {
        private string[] _swearWord;
        private string[] _swearWordBase = { "Ass", "Asshole", "Bitch", "Dick", "Faggot", "Fuck", "Nigger", "Shit" };
        private string[] _swearWordAll;
        public SwearWordAttribute(params string[] swearWord)
        {
            _swearWord = swearWord;
            _swearWordAll = Foo(_swearWord, _swearWordBase);
        }

        private string[] Foo(string[] arr1, string[] arr2)
        {
            var arr3 = arr1.Concat(arr2).ToArray();
            return arr3;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.IsNullOrEmpty(ErrorMessage)
                ? $"We don't like you. Cause you use bad word like: \"{string.Join(", ", _swearWordAll)}\""
                : ErrorMessage;
        }

        public override bool IsValid(object value)
        {
            if (value is null)
            {
                return true;
            }

            if (!(value is string))
            {
                throw new ArgumentException("NotFlatAttribute can work only with string");
            }

            var line = value as string;

            return _swearWordAll.All(swearWord => !line.ToLower().Contains(swearWord.ToLower()));

        }
    }
}
