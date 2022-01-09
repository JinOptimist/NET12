using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebMaze.Models.ValidationAttributes
{
    public class ZumaGameLimitsAttribute : ValidationAttribute
    {
        private int _minValue;
        private int _maxValue;

        public ZumaGameLimitsAttribute(int minValue, int maxValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;
        }

        public override bool IsValid(object value)
        {

            if (!(value is int))
            {
                throw new ArgumentException("Only for int");
            }

            var intValue = (int)value;
            return intValue >= _minValue && intValue <= _maxValue;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.IsNullOrEmpty(ErrorMessage)
                ? $"Need min:{_minValue} and max:{_maxValue}"
                : ErrorMessage;
        }

    }
}
