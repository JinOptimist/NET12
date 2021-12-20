using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.Models
{
    public class ActionViewModel
    {
        public string Name { get; set; }
        public List<string> AttributeNames { get; set; }
        public List<string> ParamsNames { get; set; }
    }
}
