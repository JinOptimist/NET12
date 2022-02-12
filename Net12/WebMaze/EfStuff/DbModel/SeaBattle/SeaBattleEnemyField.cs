using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.EfStuff.DbModel.SeaBattle
{
    public class SeaBattleEnemyField : BaseModel
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public long GameId { get; set; }
        public virtual SeaBattleGame Game { get; set; }
        public virtual List<SeaBattleEnemyCell> Cells { get; set; }

    }
}
