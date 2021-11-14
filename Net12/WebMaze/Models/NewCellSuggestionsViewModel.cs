using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.Models
{
    public class NewCellSuggestionsViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int MoneyChange { get; set; }
        public int HealtsChange { get; set; }
        public int FatigueChange { get; set; }
        public string UserName { get; set; }
    }
}
