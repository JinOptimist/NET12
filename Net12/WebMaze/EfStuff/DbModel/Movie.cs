using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.EfStuff.DbModel
{
    public class Movie
    {
        public long Id { get; set; }
        public string TitleGame { get; set; }
        public string TitleMovie { get; set; }
        public int Release { get; set; }
        public string Link { get; set; }
        public string Img { get; set; }

        public virtual List<Game> Games { get; set; } = new List<Game>();
    }
}
