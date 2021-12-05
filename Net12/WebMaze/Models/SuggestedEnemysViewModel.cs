using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Models.ValidationAttributes;

namespace WebMaze.Models
{
    public class SuggestedEnemysViewModel
    {
        public long Id { get; set; }

        [SwearWord("lox")]
        public string Name { get; set; }
        [Url()]
        public string Url { get; set; }
        [SwearWord("lox")]
        [WordCount(3)]
        public string Description { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }

    }
}
