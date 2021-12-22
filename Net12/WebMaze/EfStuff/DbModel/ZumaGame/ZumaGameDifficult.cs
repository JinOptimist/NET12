using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.EfStuff.DbModel
{
    public class ZumaGameDifficult : BaseModel
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int ColorCount { get; set; }
        public virtual User Author { get; set; }
    }
}
