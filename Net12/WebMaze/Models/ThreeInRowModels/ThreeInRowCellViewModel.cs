using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.Models.ThreeInRow
{
    public class ThreeInRowCellViewModel
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Color { get; set; }
    }
}