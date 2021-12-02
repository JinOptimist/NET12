using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.EfStuff.DbModel
{
    public class Game : BaseModel
    {
        public string Name { get; set; }
        public string Genre { get; set; }
        public int YearOfProd { get; set; }
        public string Desc { get; set; }
        public int Rating { get; set; }

        public virtual User Creater { get; set; }
        public virtual List<Movie> Movies { get; set; }
    }
}
