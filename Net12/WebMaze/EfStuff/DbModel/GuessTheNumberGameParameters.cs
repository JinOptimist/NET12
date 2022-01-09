using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Services.GuessTheNumber.Enums;

namespace WebMaze.EfStuff.DbModel.GuessTheNumberDbModel
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
