using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.EfStuff.DbModel
{
    public class SuggestedEnemys : BaseModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public virtual User Creater { get; set; }
        public virtual User Approver { get; set; }
    }
}
