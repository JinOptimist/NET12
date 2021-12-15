using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.Models
{
    public class ZumaGameFieldViewModel
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int ColorCount { get; set; }
        public virtual List<ZumaGameCellViewModel> Cells { get; set; }
    }
}
