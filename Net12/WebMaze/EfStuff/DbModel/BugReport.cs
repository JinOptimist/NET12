using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.EfStuff.DbModel
{
    public class BugReport : BaseModel
    {
        public string Description { get; set; }
        public virtual User Creater { get; set; }
    }
}
