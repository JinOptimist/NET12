using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.EfStuff.DbModel
{
    public class Review
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Rate { get; set; }
        public string Text { get; set; }
    }
}
