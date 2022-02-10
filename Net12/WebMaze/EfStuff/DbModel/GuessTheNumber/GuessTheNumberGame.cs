using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Services.GuessTheNumber;

namespace WebMaze.EfStuff.DbModel.GuessTheNumber
{
    public class GuessTheNumberGame : BaseModel
    {
        public int GuessedNumber { get; set; }
        public DateTime StartDateGame { get; set; }
        public int AttemptNumber { get; set; }
        public GuessTheNumberGameStatus GameStatus { get; set; }
        public virtual User Player { get; set; }
        public virtual List<GuessTheNumberGameAnswer> Answers { get; set; }
        public virtual GuessTheNumberGameParameters Parameters { get; set; }
    }
}
