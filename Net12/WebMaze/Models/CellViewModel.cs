using Net12.Maze;
using WebMaze.EfStuff;
using WebMaze.EfStuff.DbModel;

namespace WebMaze.Models
{
    public class CellViewModel
    {
        public MazeCellInfo TypeCell { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int HpCell { get; set; }

        public MazeViewModel MazeLevel { get; set; }

    }
}
