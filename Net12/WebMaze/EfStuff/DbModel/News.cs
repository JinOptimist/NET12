using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.EfStuff.DbModel
{
    public class News : BaseModel
    {
        public DateTime CreationDate { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public DateTime EventDate { get; set; }
        public bool IsActive { get; set; }
        public virtual User Author { get; set; }
    }
}
