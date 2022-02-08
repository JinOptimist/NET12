using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.EfStuff.DbModel.SeaBattle
{
    public class SeaBattleGame : BaseModel
    {
        public virtual User Gamer { get; set; }

        public virtual SeaBattleMyField MyField { get; set; }
        public virtual SeaBattleEnemyField EnemyField { get; set; }

    }
}
