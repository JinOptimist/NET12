using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static WebMaze.Services.SeaBattleService;

namespace WebMaze.EfStuff.DbModel.SeaBattle
{
    public class SeaBattleDifficult : BaseModel
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int TwoSizeShip { get; set; }
        public int ThreeSizeShip { get; set; }
        public int FourSizeShip { get; set; }
    }
}
