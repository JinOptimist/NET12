using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Models.ValidationAttributes;

namespace WebMaze.Models
{
    public class MazeDifficultProfileViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }

        [MazeDifficultLimits(5, 70)]
        public int Width { get; set; }

        [MazeDifficultLimits(10, 150)]
        public int Height { get; set; }
        public int HeroMoney { get; set; }
        public int HeroMaxHp { get; set; }
        public int HeroMaxFatigue { get; set; }
        public int CoinCount { get; set; }
        public string Author { get; set; }
        public int GlobalUserRating { get; set; }

    }
}
