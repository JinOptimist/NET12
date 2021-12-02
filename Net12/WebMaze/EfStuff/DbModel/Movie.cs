using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.EfStuff.DbModel
{
    public class Movie : BaseModel
    {
        public string TitleGame { get; set; }
        public string TitleMovie { get; set; }
        public int Release { get; set; }
        public string Link { get; set; }
        public string Img { get; set; }

        public virtual Game Game { get; set; }
    }
}
