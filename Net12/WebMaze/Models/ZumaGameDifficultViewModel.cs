using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Models.ValidationAttributes;

namespace WebMaze.Models
{
    public class ZumaGameDifficultViewModel
    {
        public long Id { get; set; }

        [ZumaGameLimits(5, 20)]
        public int Width { get; set; }

        [ZumaGameLimits(5, 20)]
        public int Height { get; set; }

        [ZumaGameLimits(2, 7)]
        public int ColorCount { get; set; }
        public int Price { get; set; }
        public string Author { get; set; }
    }
}
