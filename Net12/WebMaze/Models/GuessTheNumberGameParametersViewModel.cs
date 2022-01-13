using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Services.GuessTheNumber.Enums;

namespace WebMaze.Models.GuessTheNumber
{
    public class GuessTheNumberGameParametersViewModel
    {
        public GuessTheNumberGameDifficulty Difficulty { get; set; }        
        public int MaxAttempts { get; set; }        
        public int MinRangeNumber { get; set; }
        public int MaxRangeNumber { get; set; }        
    }
}
