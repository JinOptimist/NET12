using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.Models.ValidationAttributes
{
    public class MaxNewsDataAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime dataTime = Convert.ToDateTime(value);
            return dataTime >= DateTime.Now.AddDays(-7) && dataTime < DateTime.Now;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.IsNullOrEmpty(ErrorMessage)
                ? "There's no need to use an irrelevant news. Seven-day period is only applicable"
                : ErrorMessage;
        }
    }
}
