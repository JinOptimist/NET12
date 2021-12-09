using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.EfStuff.DbModel
{
    public class NewsComment : BaseModel
    {        
        public DateTime CreationDate { get; set; }
        public string Text { get; set; }
        public virtual User Author { get; set; }
        public virtual News News { get; set; }
    }
}
