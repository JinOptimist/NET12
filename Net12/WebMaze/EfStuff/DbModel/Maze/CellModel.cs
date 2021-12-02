namespace WebMaze.EfStuff.DbModel
{
    public class CellModel : BaseModel
    {
        public CellInfo TypeCell { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int HpCell { get; set; }

        public virtual MazeLevelModel MazeLevel { get; set; }
    }
}
