using Net12.Maze;
using Net12.Maze.Enums;
using System.Collections.Generic;

namespace WebMaze.EfStuff.DbModel
{
    public class MazeLevelWeb : BaseModel
    {

        public virtual List<MazeCellWeb> Cells { get; set; }
        public virtual List<MazeEnemyWeb> Enemies { get; set; }
        public virtual User Creator { get; set; }
        public virtual MazeDifficultProfile DifficultProfile { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int HeroMaxHp { get; set; }
        public int HeroMaxFatigure { get; set; }

        public int HeroX { get; set; }
        public int HeroY { get; set; }
        public int HeroNowHp { get; set; }
        public int HeroNowFatigure { get; set; }

        public int HeroMoney { get; set; }

        public string Message { get; set; }
        public bool ExitIsOpen { get; set; }
        public MazeStatusEnum MazeStatus { get; set; }

    }
}
