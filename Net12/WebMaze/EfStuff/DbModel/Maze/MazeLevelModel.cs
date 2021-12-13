using Net12.Maze;
using System.Collections.Generic;

namespace WebMaze.EfStuff.DbModel
{
    public class MazeLevelModel : BaseModel
    {

        public virtual List<CellModel> Cells { get; set; }
        public virtual User Creator { get; set; }

        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int HeroMaxHp { get; set; }
        public int HeroMaxFatigure { get; set; }

        public int HeroX { get; set; }
        public int HeroY { get; set; }
        public int HeroNowHp { get; set; }
        public int HeroNowFatigure { get; set; }


    }
}
