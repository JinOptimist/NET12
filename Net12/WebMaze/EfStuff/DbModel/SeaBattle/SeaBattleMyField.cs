using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.EfStuff.DbModel.SeaBattle
{
    public class SeaBattleMyField : SeaBattleBaseField
    {
        public long HitInShipId { get; set; }

        public virtual List<SeaBattleMyCell> PossibleShootCells { get; set; }
        public virtual List<SeaBattleMyCell> Cells { get; set; }
    }
}
