namespace WebMaze.Models
{
    public class MinerCellViewModel
    {
        public long Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public bool IsOpen { get; set; }
        public bool IsBomb { get; set; }
        public bool IsFlag { get; set; }

        public int NearBombsCount { get; set; }
    }
}