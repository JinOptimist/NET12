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
        private string[] _swearWordBase = { "Asshole", "Bitch", "Dick", "Faggot", "Fuck", "Nigger", "Shit" };
        public SwearWordAttribute(params string[] swearWord)
        {
            _swearWord = swearWord;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.IsNullOrEmpty(ErrorMessage)
                ? $"We don't like you. Cause you use bad word like: \"{string.Join(" ", _swearWordBase)}\""
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

            return _swearWordBase.All(swearWord => !line.Contains(swearWord));

            //&& _swearWordBase.All(swearWord => !line.ToLower().Contains(swearWord));
        }
    }
}
