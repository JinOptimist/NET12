﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.Models
{
    public class MinerFieldViewModel
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public virtual List<MinerCellViewModel> Cells { get; set; }
    }
}
