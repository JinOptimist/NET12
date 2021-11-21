using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.Models
{
    public class MazeDifficultProfileViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int HeroMoney { get; set; }
        public int HeroMaxHp { get; set; }
        public int HeroMaxFatigue { get; set; }
        public int CoinCount { get; set; }
        public string Author { get; set; }

    }
}
