using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WebMaze.Models.ValidationAttributes;

namespace WebMaze.Models
{
    public class GameViewModel
    {
        [Required(ErrorMessage ="Mandatory field")]
        [MinLength(2, ErrorMessage = "Name of game is too short")]
        public string Name { get; set; }

        [MinLength(5, ErrorMessage = "Game's genre is too short")]
        public string Genre { get; set; }

        [CheckYear]
        public int YearOfProd { get; set; }

        [MinLength(2, ErrorMessage = "The smallest description's  size is 2 letters")]
        [MaxLength(500, ErrorMessage = "The biggest description's  size is 500 letters")]
        public string Desc { get; set; }

        public int Rating { get; set; }
        public string Username { get; set; }
        public int Age { get; set; }
    }
}
