using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.EfStuff.DbModel
{
    public class ZumaGameColor : BaseModel
    {
        public string Color { get; set; }

        public virtual ZumaGameField Field { get; set; }

    }
}
