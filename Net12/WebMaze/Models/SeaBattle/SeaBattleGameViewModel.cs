using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.Models
{
    public class SeaBattleGameViewModel
    {
        public long Id { get; set; }
        public SeaBattleFieldViewModel MyField { get; set; }
        public SeaBattleFieldViewModel EnemyField { get; set; }
        public int TwoSizeShip { get; set; }
        public int ThreeSizeShip { get; set; }
        public int FourSizeShip { get; set; }
    }
}
