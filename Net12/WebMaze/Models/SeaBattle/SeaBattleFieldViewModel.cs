using System.Collections.Generic;

namespace WebMaze.Models
{
    public class SeaBattleFieldViewModel
    {
        public long Id { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int ShipLength { get; set; }

        /// <summary>
        /// 0(false) - My Field
        /// 1(true) - Enemy Field
        /// </summary>
        public bool IsField { get; set; }
        public List<SeaBattleCellViewModel> Cells { get; set; }
    }
}