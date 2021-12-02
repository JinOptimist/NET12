using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebMaze.Models.ValidationAttributes
{
    public class HeroStuffPriceAttribute : ValidationAttribute
    {
        private int _min;
        private int _max;
        private int _step;

        public HeroStuffPriceAttribute(int min, int max, int step)
        {
            _max = max;
            _min = min;
            _step = step;
        }

        public override bool IsValid(object value)
        {
            if (value is null)
            {
                return false;
            }
            return (int)value >= _min && (int)value <= _max && (int)value % _step == 0;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.IsNullOrEmpty(ErrorMessage)
                ? $"Value must be greater than {_min - 1} less than {_max + 1} and equal to {_step}"
                : ErrorMessage;
        }
    }
}
