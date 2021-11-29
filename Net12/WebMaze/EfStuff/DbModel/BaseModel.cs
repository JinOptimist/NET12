using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.EfStuff.DbModel
{
    public abstract class BaseModel
    {
        public long Id { get; set; }
        public bool IsActive { get; set; }
    }
}
