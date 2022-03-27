using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.Models
{
    public class PaggerViewModel<T>
    {
        public const int AdditionalElementsNearCurrent = 5;
        public int TotalRecordsCount { get; set; }

        public int PerPage { get; set; }

        public List<int> PerPageOptions
            => new List<int> { 7, 13, 50 };

        public int TotalPageCount
        {
            get
            {
                return (int)Math.Ceiling(
                    (decimal)TotalRecordsCount
                    / (decimal)PerPage);
            }
        }

        public int CurrPage { get; set; }

        public int StartIndex
        {
            get
            {
                var start = CurrPage - AdditionalElementsNearCurrent;
                if (start < 0)
                {
                    start = 0;
                }
                return start;
            }
        }

        public int EndIndex
        {
            get
            {
                var end = CurrPage + AdditionalElementsNearCurrent;
                if (end > TotalPageCount)
                {
                    end = TotalPageCount;
                }
                return end;
            }
        }

        public List<T> Records { get; set; }
    }
}
