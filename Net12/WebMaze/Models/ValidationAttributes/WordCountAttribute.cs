using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebMaze.Models.ValidationAttributes
{
    public class WordCountAttribute : ValidationAttribute
    {
        private int _wordCound;

        public WordCountAttribute(int wordCound)
        {
            _wordCound = wordCound;
        }

        public override bool IsValid(object value)
        {
            if (value is null)
            {
                return false;
            }

            if (!(value is string))
            {
                throw new ArgumentException("WordCountAttribute can work only with string");
            }

            var line = value as string;
            string[] words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return words.Length >= _wordCound; ;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.IsNullOrEmpty(ErrorMessage)
                ? $"Description must contain at least {_wordCound} words."
                : ErrorMessage;
        }
    }
}
