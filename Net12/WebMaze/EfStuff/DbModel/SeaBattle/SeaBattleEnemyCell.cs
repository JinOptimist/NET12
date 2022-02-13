using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.EfStuff.DbModel.SeaBattle
{
    public class SeaBattleEnemyCell : SeaBattleBaseCell
    {
        public virtual SeaBattleEnemyField Field { get; set; }
    }
}
