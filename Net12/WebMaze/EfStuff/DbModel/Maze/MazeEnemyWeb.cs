using WebMaze.EfStuff.DbModel.Maze;

namespace WebMaze.EfStuff.DbModel
{
    public class MazeEnemyWeb : BaseModel
    {
        public MazeEnemyInfo TypeEnemy { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public int Obj1{ get; set; }
        public int Obj2{ get; set; }
        public virtual MazeLevelWeb MazeLevel { get; set; }

    }
}
