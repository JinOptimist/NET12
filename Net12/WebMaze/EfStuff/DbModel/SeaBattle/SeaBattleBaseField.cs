using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.EfStuff.DbModel.SeaBattle
{
    public class SeaBattleBaseField<T> : BaseModel where T: SeaBattleBaseCell
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public virtual SeaBattleGame Game { get; set; }
        public virtual List<T> Cells { get; set; }

    }
}
