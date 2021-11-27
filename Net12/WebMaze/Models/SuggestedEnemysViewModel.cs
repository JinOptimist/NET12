﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.Models
{
    public class SuggestedEnemysViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }

    }
}