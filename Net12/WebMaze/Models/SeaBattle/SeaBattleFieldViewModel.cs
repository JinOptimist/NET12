using System.Collections.Generic;

namespace WebMaze.Models
{
    public class SeaBattleFieldViewModel
    {
        public long Id { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public List<SeaBattleCellViewModel> Cells { get; set; }
    }
}