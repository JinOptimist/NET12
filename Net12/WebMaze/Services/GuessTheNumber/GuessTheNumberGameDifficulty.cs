using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.ResourceLocalization;

namespace WebMaze.Services.GuessTheNumber
{
    public enum GuessTheNumberGameDifficulty
    {
        [Display(Name = "Easy" , ResourceType = typeof (WebMaze.ResourceLocalization.GuessTheNumber))]
        Easy = 1,
        [Display(Name = "Medium", ResourceType = typeof(WebMaze.ResourceLocalization.GuessTheNumber))]
        Medium = 2,
        [Display(Name ="Hard", ResourceType = typeof(WebMaze.ResourceLocalization.GuessTheNumber))]
        Hard = 3
    }    
}
