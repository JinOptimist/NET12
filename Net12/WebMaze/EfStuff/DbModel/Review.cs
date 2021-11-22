using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.EfStuff.DbModel
{
    public class Review : BaseModel
    {
        public int Rate { get; set; }
        public string Text { get; set; }
        public virtual User Creator { get; set; }
    }
}
