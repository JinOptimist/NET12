namespace WebMaze.EfStuff.DbModel
{
    public class ZumaGameCell : BaseModel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Color { get; set; }

        public virtual ZumaGameField Field { get; set; }
    }
}