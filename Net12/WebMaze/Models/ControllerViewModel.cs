using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.Models
{
    public class ControllerViewModel
    {
        public string Name { get; set; }
        public int ActionCount { get; set; }
        public List<string> AttributeNames { get; set; }

        public List<ActionViewModel> Actions { get; set; }
    }
}
