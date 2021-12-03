using Net12.Maze;

namespace WebMaze.Models
{
    public class MazeLevelViewId
    {
        public long idMaze;
        public MazeLevel MazeLevel {  get; set; }

        public MazeLevelViewId(long idCreator, MazeLevel maze)
        {
            idMaze = idCreator;
            MazeLevel = maze;
        }
    }
}
