using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Models.ValidationAttributes;

namespace WebMaze.Models
{
    public class StuffForHeroViewModel
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Too short. Give us more information please")]
        public string Description { get; set; }

        [Required]
        [HeroStuffPrice(5, 500, 5)]
        public int Price { get; set; }

        [Required]
        public string PictureLink { get; set; }

        public string Proposer { get; set; }
    }
}
