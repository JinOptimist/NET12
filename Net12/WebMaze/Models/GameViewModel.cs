using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebMaze.Models
{
    public class GameViewModel
    {
        [MinLength(2, ErrorMessage = "Name of game is too short")]
        public string Name { get; set; }

        [MinLength(5, ErrorMessage = "Game's genre is too short")]
        public string Genre { get; set; }

        [CheckYearAttribute]
        public int YearOfProd { get; set; }
        public string Desc { get; set; }
        public int Rating { get; set; }
        public string Username { get; set; }
        public int Age { get; set; }
    }
}
