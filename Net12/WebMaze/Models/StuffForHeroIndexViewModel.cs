using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Models.ValidationAttributes;

namespace WebMaze.Models
{
    public class StuffForHeroIndexViewModel
    {
        public PaggerViewModel<StuffForHeroViewModel> PaggerViewModel { get; set; }
        public bool IsDescending { get; set; }
        public string LastSort { get; set; }

    }
}
