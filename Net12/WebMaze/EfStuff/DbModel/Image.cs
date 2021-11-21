using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.EfStuff.DbModel
{
    public class Image : BaseModel
    {
        public string Description { get; set; }
        public string Picture { get; set; }
        public int Assessment { get; set; }
        public virtual User Author { get; set; }
    }
}
