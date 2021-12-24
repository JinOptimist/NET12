using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.EfStuff.DbModel
{
    public class ZumaGameField : BaseModel
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int ColorCount { get; set; }
        public virtual long GamerId { get; set; }
        public virtual List<ZumaGameColor> Palette { get; set; }
        public virtual List<ZumaGameCell> Cells { get; set; }
        public virtual User Gamer { get; set; }
    }
}
