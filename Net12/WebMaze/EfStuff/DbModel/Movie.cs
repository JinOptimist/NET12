﻿using System;
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
        public int release { get; set; }
        public string Link { get; set; }


    }
}
