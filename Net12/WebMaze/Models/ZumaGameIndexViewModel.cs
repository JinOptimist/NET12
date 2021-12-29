using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.Models
{
    public class ZumaGameIndexViewModel
    {
        public List<ZumaGameDifficultViewModel> viewModels { get; set; }
        public bool Continue { get; set; }
    }
}
