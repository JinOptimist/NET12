using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;

namespace WebMaze.Models
{
    public class ImageViewModel
    {
        public long Id { get; set; }
        public User Author { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public int Assessment { get; set; }
        
    }
}
