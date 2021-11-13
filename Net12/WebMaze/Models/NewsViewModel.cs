using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.Models
{
    public class NewsViewModel
    {
        public DateTime DatNow { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public DateTime DateOfNew { get; set; }
        public string NameOfAthor { get; set; }
    }
}
