using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.EfStuff.DbModel.SeaBattle
{
    public class SeaBattleEnemyField : SeaBattleBaseField
    {
        public virtual List<SeaBattleEnemyCell> Cells { get; set; }
    }
}
