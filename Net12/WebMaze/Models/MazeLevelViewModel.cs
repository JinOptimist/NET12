using Net12.Maze.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Models.Enums;

namespace WebMaze.Models
{
    public class MazeLevelViewModel
    {
        public int Id { get; set; }

        public int HeroNowHp { get; set; }
        public int HeroMaxHp { get; set; }
        public int HeroNowFatigure { get; set; }
        public int HeroMaxFatigure { get; set; }
        public int HeroMoney { get; set; }
        public string Message { get; set; }
        public bool ExitIsOpen { get; set; }
        public MazeStatusEnum MazeStatus { get; set; }
        public List<MazeCellViewModel> Cells { get; set; }
    }
}
