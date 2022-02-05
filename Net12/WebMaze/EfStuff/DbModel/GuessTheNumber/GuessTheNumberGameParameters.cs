using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using WebMaze.Services.GuessTheNumber;

namespace WebMaze.EfStuff.DbModel.GuessTheNumber
{
    public class GuessTheNumberGameParameters : BaseModel
    {        
        public GuessTheNumberGameDifficulty Difficulty { get; set; }
        public int RewardForWinningTheGame { get; set; }
        public int GameCost { get; set; }
        public int MaxAttempts { get; set; }
        public int MinRangeNumber { get; set; }
        public int MaxRangeNumber { get; set; }
        public virtual List<GuessTheNumberGame> Games { get; set; }
    }
}
