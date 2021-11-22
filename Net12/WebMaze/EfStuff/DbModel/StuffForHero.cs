using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.EfStuff.DbModel
{
    public class StuffForHero: BaseModel
    {
        public string Description { get; set; }
        public int Price { get; set; }
        public string PictureLink { get; set; }
        public virtual User Proposer { get; set; }
    }
}
