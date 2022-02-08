using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.Models
{
    public class SeaBattleGameViewModel
    {
        public SeaBattleFieldViewModel MyField { get; set; }
        public SeaBattleFieldViewModel EnemyField { get; set; }
    }
}
