using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.EfStuff.DbModel
{
    public class MinerField : BaseModel
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsOver { get; set; }
        public bool IsWon { get; set; }
        public bool IsPlayingNow { get; set; }

        public virtual List<MinerCell> Cells { get; set; }
        public virtual User Gamer { get; set; }
    }
}
