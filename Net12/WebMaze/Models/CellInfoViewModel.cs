using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.Models
{
    public class CellInfoViewModel
    {
        public string ImageUrl { get; set; }
        public string Desc { get; set; }
        public string Remainder { get; set; }
        public string Use { get; set; }
        public bool CanStep { get; set; }
    }
}
