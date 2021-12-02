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
                return false;
                //throw new ArgumentException("CheckYear Attribute can work only with int");
            }

            var number = (int?)value;

            if (!(number >= 0 && number <= 99 || number >= 1000 && number <= 9999))
            {
                return false;
                //throw new ArgumentException("CheckYear Attribute can work only with 'yyyy' or 'yy' format");
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
