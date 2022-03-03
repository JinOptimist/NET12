using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.EfStuff.DbModel.SeaBattle
{
    public class SeaBattleGame : BaseModel
    {
        public long UserId { get; set; }
        public virtual User User { get; set; }
        public virtual List<SeaBattleField> Fields { get; set; }
    }
}
