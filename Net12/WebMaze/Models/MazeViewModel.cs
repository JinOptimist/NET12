using Net12.Maze;
using System.Collections.Generic;
using WebMaze.EfStuff.DbModel;

namespace WebMaze.Models
{
    public class MazeViewModel
    {
        public int Id { get; set; }
        public List<CellViewModel> Cells { get; set; }
        public UserViewModel Creator { get; set; }

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
