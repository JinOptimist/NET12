using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.EfStuff.DbModel.ThreeInRow
{
    public class ThreeInRowGameField : BaseModel
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Score { get; set; }
        public string NextColor { get; set; }

        public virtual List<ThreeInRowCell> Cells { get; set; }   

        public virtual User Player { get; set; }
    }
}