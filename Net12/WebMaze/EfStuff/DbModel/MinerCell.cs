namespace WebMaze.EfStuff.DbModel
{
    public class MinerCell : BaseModel
    {
        public int X { get; set; }
        public int Y { get; set; }

        public bool IsOpen { get; set; }
        public bool IsBomb { get; set; }

        public int NearBombsCount { get; set; }

        public virtual MinerField Field { get; set; }
    }
}