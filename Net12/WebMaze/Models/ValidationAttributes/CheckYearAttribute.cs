using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.Models.ValidationAttributes
{
    public class CheckYearAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {         
            if (value != null && !(value is int))
            {
                ErrorMessage = "CheckYear Attribute can work only with int";
                return false;
            }

            var number = (int?)value;

            if (!((number >= 0 && number <= 99) || (number >= 1000 && number <= 9999)))
            {
                ErrorMessage = "CheckYear Attribute can work only with 'yyyy' or 'yy' format";
                return false;
            }

            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.IsNullOrEmpty(ErrorMessage)
                ? "Impossible to read year. Please use form as 'yyyy' or 'yy'"
                : ErrorMessage;
        }
    }
}
