using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.Models
{
    public class StuffForHeroIndexViewModel
    {
        public PaggerViewModel<StuffForHeroViewModel> StuffForHeroViewModels { get; set; }
        public List<string> FilteredColumnNames { get; set; }
    }
}
