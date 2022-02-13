using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.EfStuff.DbModel.SeaBattle
{
    public class SeaBattleBaseCell : BaseModel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool ShipHere { get; set; }
        public bool Hit { get; set; }
        public int ShipLength { get; set; }
        public int ShipNumber { get; set; }
        public int ShipDirection { get; set; }
    }
}
