using System;
using System.ComponentModel.DataAnnotations;

namespace WebMaze.Models.ValidationAttributes
{
    public class DiapazoneAttribute : ValidationAttribute
    {
        private int _min;
        private int _max;

        public DiapazoneAttribute(int min, int max)
        {
            _max = max;
            _min = min;
        }
        public override string FormatErrorMessage(string name)
        {
            return string.IsNullOrEmpty(ErrorMessage)
                ? $"You can use rating diapazone only from {_min} to {_max}"
                : ErrorMessage;
        }
        public override bool IsValid(object value)
        {
            if (value is null)
            {
                return false;
            }

            if (!(value is int))
            {
                throw new ArgumentException("DiapazoneAttribute work only with int values");
            }
            var num = (int)value;
            if (_min <= num && num <= _max)
            {
                return true;
            }

            return false;
        }

    }
}
