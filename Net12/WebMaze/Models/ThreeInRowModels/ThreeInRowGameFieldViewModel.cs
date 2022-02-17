using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.Models.ThreeInRow
{
    public class ThreeInRowGameFieldViewModel
    {
        public long Id { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Score { get; set; }
        public bool IsActive { get; set; }


        public string NextColor { get; set; }

        public virtual List<ThreeInRowCellViewModel> Cells { get; set; }
    }
}