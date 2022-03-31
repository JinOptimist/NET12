using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Models.Enums;

namespace WebMaze.Models
{
    public class SortedBooksViewModel
    {
        public string BookFilter { get; set; }
        public bool Asc { get; set; }
        public virtual List<BookViewModel> Books { get; set; }
    }
}