using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.EfStuff.DbModel.SeaBattle
{
    public class SeaBattleCell : BaseModel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int ShipLength { get; set; }
        /// <summary>
        /// 0 - my field
        /// 1 - enemy field
        /// </summary>
        public bool SideField { get; set; }
        public bool ShipHere { get; set; }

        public virtual SeaBattleField Field { get; set; }
    }
}
