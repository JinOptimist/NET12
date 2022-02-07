using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.EfStuff.DbModel.SeaBattle
{
    public class SeaBattleField : BaseModel
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public virtual User Gamer { get; set; }
        public List<SeaBattleCell> Cells { get; set; }
    }
}
