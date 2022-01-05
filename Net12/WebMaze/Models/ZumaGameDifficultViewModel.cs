using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.Models
{
    public class ZumaGameDifficultViewModel
    {
        public long Id { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int ColorCount { get; set; }
        public int Price { get; set; }
        public string Author { get; set; }
    }
}
