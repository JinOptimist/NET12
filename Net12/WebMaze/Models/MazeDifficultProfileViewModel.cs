using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.Models
{
    public class MazeDifficultProfileViewModel
    {
        public string Name { get; set; }
        public int HeroMoney { get; set; }
        public int HeroMaxHp { get; set; }
        public int HeroMaxFatigue { get; set; }
        public int CoinCount { get; set; }

    }
}
