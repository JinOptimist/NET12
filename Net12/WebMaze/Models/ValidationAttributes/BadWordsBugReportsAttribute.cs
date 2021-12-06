using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebMaze.Models.ValidationAttributes
{
    public class BadWordsBugReportsAttribute : ValidationAttribute
    {
        private string[] _stopWords;
        public BadWordsBugReportsAttribute(params string[] stopWord)
        {
            _stopWords = stopWord;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.IsNullOrEmpty(ErrorMessage)
                ? $"You can't use bad words like: \"{string.Join(" ", _stopWords)}\""
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
                throw new ArgumentException("We can work only with string");
            }

           var line = value as string;

            return _stopWords.All(stopWord => !line.ToLower().Contains(stopWord));
        }
    }
}
