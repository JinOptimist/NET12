using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.EfStuff.DbModel
{
    public class MazeDifficultProfile
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int HeroMoney { get; set; }
        public int HeroMaxHp { get; set; }
        public int HeroMaxFatigue { get; set; }
        public int CoinCount { get; set; }



    }
}
