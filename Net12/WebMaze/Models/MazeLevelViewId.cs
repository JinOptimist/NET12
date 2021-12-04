using Net12.Maze;

namespace WebMaze.Models
{
    public class MazeLevelViewId
    {
        public long Id;
        public MazeLevel MazeLevel {  get; set; }

        public MazeLevelViewId(long idCreator, MazeLevel maze)
        {
            Id = idCreator;
            MazeLevel = maze;
        }
    }
}
