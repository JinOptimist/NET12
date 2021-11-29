using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.Models
{
    public class BugReportViewModel
    {
        public string CreaterName { get; set; }

        [MinLength(10, ErrorMessage = "This description is too short, write more please :)")]
        public string Description { get; set; }
    }
}
