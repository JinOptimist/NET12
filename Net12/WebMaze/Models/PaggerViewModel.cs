using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.Models
{
    public class PaggerViewModel<T>
    {
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
        public List<T> Records { get; set; }
    }
}
