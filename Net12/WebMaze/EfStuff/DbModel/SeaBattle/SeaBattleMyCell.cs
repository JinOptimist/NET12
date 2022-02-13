using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.EfStuff.DbModel.SeaBattle
{
    public class SeaBattleMyCell : SeaBattleBaseCell
    {
        public virtual SeaBattleMyField Field { get; set; }
    }
}
