﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;

namespace WebMaze.Models
{
    public class FeedBackUserViewModel
    {
        public long Id { get; set; }
        public UserViewModel Creator { get; set; }
        public int Rate { get; set; }
        public string TextInfo { get; set; }
    }
}
