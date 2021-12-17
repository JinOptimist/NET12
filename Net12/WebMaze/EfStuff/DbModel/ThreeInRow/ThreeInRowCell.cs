using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.EfStuff.DbModel.ThreeInRow
{
    public class ThreeInRowCell : BaseModel
    {
        public int X { get; set; }
        public int Y { get; set; }

        public virtual ThreeInRowGameField GameField { get; set; }
    }
}
