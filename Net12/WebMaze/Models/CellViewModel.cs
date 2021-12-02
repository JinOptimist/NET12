using WebMaze.EfStuff;
using WebMaze.EfStuff.DbModel;

namespace WebMaze.Models
{
    public class CellViewModel
    {
        public int Id { get; set; }
        public CellInfo TypeCell { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int HpCell { get; set; }

        public virtual MazeLevelModel MazeLevel { get; set; }
    }
}
