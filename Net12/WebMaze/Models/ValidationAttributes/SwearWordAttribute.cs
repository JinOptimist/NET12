using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebMaze.Models.ValidationAttributes
{
    public class SwearWordAttribute : ValidationAttribute
    {
        private List<string> _swearWord;
        private List<string> _swearWordBase = new List<string> { "Ass", "Asshole", "Bitch", "Dick", "Faggot", "Fuck", "Nigger", "Shit" };
        public SwearWordAttribute(params string[] swearWord)
        {
            _swearWord = swearWord.ToList();
            _swearWord.AddRange(_swearWordBase);
            _swearWord = _swearWord.Select(x => x.ToLower()).ToList();
        }


        public override bool IsValid(object value)
        {
            if (value is null)
            {
                return true;
            }

            if (!(value is string))
            {
                throw new ArgumentException("SwearWordAttribute can work only with string");
            }

            var line = value as string;

            string[] words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            return words.Any(x => !_swearWord.Contains(x.ToLower()));

        }
        public override string FormatErrorMessage(string name)
        {
            return string.IsNullOrEmpty(ErrorMessage)
                ? $"We don't like you. Cause you use bad word like: \"{string.Join(", ", _swearWord)}\""
                : ErrorMessage;

        }
    }
}
