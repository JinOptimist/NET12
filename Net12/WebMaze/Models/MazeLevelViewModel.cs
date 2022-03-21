using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.Models
{
    public class MazeLevelViewModel
    {
        public int Id { get; set; }

        public List<MazeCellViewModel> Cells { get; set; }
    }
}
