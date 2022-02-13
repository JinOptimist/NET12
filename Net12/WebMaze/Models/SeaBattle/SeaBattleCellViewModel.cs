namespace WebMaze.Models
{
    public class SeaBattleCellViewModel
    {
        public long Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int ShipLength { get; set; }
        public bool ShipHere { get; set; }
        public bool Hit { get; set; }
        public int ShipNumber { get; set; }
    }
}