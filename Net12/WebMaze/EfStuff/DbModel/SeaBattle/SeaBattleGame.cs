using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.EfStuff.DbModel.SeaBattle
{
    public class SeaBattleGame : BaseModel
    {
        public long HitInShipId { get; set; }

        /// <summary>
        /// 1 - left
        /// 2 - right
        /// 3 - up
        /// 4 - down
        /// </summary>
        public int DirectionToShoot { get; set; }
        public long UserId { get; set; }
        public virtual User User { get; set; }
        public virtual List<SeaBattleField> Fields { get; set; }
    }
}
