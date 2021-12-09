using Net12.Maze;

namespace WebMaze.Models
{
    public class MazeLevelViewId
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public MazeLevel MazeLevel {  get; set; }

        public UserViewModel Creator { get; set; }

    }
}
